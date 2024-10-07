using System.Threading.Tasks;

namespace SimpleMotions {

	public sealed class VideoPlayer : IVideoPlayer {

		private readonly VideoData _videoData;

		public VideoPlayer(VideoData videoData) {
			_videoData = videoData;
		}

		public void Play() {
			_videoData.IsPlaying = true;
			IncreaseCurrentTime();
		}

		public void Resume() {
			_videoData.IsPlaying = true;
		}

		public void Pause() {
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
				_videoData.CurrentFrame = 0;
			}

			while (_videoData.IsPlaying) {
				_videoData.CurrentFrame++;
				
				UnityEngine.Debug.Log($"({_videoData.CurrentFrame} / {_videoData.TotalFrames})");

				if (_videoData.IsLooping) {
					_videoData.CurrentFrame %= _videoData.TotalFrames;
				}
				else if (_videoData.CurrentFrame >= _videoData.TotalFrames) {
					_videoData.CurrentFrame = _videoData.TotalFrames;
					Pause();
					break;
				}

				await Task.Yield();
			}
		}

	}
}