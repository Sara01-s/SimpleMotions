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

		public void SetCurrentTime(float seconds) {
			_videoData.CurrentTime = seconds;
		}

		public VideoData GetVideoData() {
			return _videoData;
		}

		private async void IncreaseCurrentTime() {
			while (_videoData.IsPlaying) {
				_videoData.CurrentFrame++;

				UnityEngine.Debug.Log(_videoData.CurrentFrame);

				await Task.Yield();
			}
		}

	}
}