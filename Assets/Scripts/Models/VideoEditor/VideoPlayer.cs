using System.Threading.Tasks;

namespace SimpleMotions {

	public sealed class VideoPlayer : IVideoPlayer {

		private readonly VideoData _videoData;
		private readonly IVideoAnimator _videoAnimator;

		public VideoPlayer(VideoData videoData, IVideoAnimator videoAnimator) {
			_videoData = videoData;
			_videoAnimator = videoAnimator;
		}

		public void TogglePlay() {
            if (!_videoData.IsPlaying) {
                Play();
            }
            else {
                Pause();
            }
		}

		public void Play() {
			_videoData.IsPlaying = true;
			UnityEngine.Debug.Log("Play");
			PlayVideo();
		}

		public void Pause() {
			UnityEngine.Debug.Log("Pause");
			_videoData.IsPlaying = false;
		}

		public void Reset() {
			_videoData.CurrentFrame = TimelineData.FIRST_KEYFRAME;
			_videoData.CurrentTime = 0.0f;
		}

		public void StopAndReset() {
			Pause();
			Reset();
		}

		public void ResetAndPlay() {
			Reset();
			Play();
		}

		public void SetCurrentTime(float seconds) {
			_videoData.CurrentTime = seconds;
		}

		public VideoData GetVideoData() {
			return _videoData;
		}

		private bool IsAtTheEnd() {
			return !_videoData.IsLooping && _videoData.CurrentFrame == _videoData.TotalFrames;
		}

		private async void PlayVideo() {
			if (IsAtTheEnd()) {
				ResetAndPlay();
			}

			_videoAnimator.GenerateVideoCache();

			while (_videoData.IsPlaying) {
				// Avisar a IVideoAnimator que interpole los keyframes de todas las entidades
				_videoAnimator.InterpolateAllEntities(_videoData.CurrentFrame);

				_videoData.CurrentTime += 1.0f / _videoData.TargetFrameRate;
				_videoData.CurrentFrame++;

				// Last frame behaviour
				UnityEngine.Debug.Log(_videoData.CurrentFrame);
				if (_videoData.CurrentFrame >= _videoData.TotalFrames) {
					if (!_videoData.IsLooping) {
						Pause();
						break;
					}

					ResetAndPlay();				
				}

				await Task.Yield();
			}
		}

	}
}