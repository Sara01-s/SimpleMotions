using System.Threading.Tasks;

namespace SimpleMotions {

	public sealed class VideoPlayer : IVideoPlayer {

		private readonly VideoData _videoData;
		private readonly IVideoAnimator _videoAnimator;

		private bool _shouldPlay = false;

		public VideoPlayer(VideoData videoData, IVideoAnimator videoAnimator) {
			_videoData = videoData;
			_videoAnimator = videoAnimator;
		}

		public void TogglePlay() {
			_shouldPlay = !_shouldPlay;

            if (_shouldPlay) {
                Play();
            }
            else {
                Pause();
            }
		}

		public void Play() {
			_videoData.IsPlaying = true;
			UnityEngine.Debug.Log("Play");
			IncreaseCurrentTime();
		}

		public void Pause() {
			UnityEngine.Debug.Log("Pause");
			_videoData.IsPlaying = false;
		}

		public void Reset() {
			_videoData.IsPlaying = false;
			_videoData.CurrentFrame = 0;
			_videoData.CurrentTime = 0.0f;
		}

		public void SetCurrentTime(float seconds) {
			_videoData.CurrentTime = seconds;
		}

		public VideoData GetVideoData() {
			return _videoData;
		}

		private bool CursorIsAtTheEnd() {
			return !_videoData.IsLooping && _videoData.CurrentFrame == _videoData.TotalFrames;
		}

		private async void IncreaseCurrentTime() {
			if (CursorIsAtTheEnd()) {
				_videoData.CurrentFrame = TimelineData.FIRST_KEYFRAME;
			}

			_videoAnimator.GenerateVideoCache();

			while (_videoData.IsPlaying) {			
				// Avisar a IVideoAnimator que interpole los keyframes de todas las entidades
				_videoAnimator.InterpolateKeyframes(_videoData.CurrentFrame);

				// End behaviour
				if (_videoData.IsLooping) {
					_videoData.CurrentFrame %= _videoData.TotalFrames;
				}
				else if (_videoData.CurrentFrame >= _videoData.TotalFrames) {
					_videoData.CurrentFrame = _videoData.TotalFrames;
					TogglePlay();
					break;
				}

				_videoData.CurrentFrame++;

				await Task.Yield();
			}
		}

	}
}