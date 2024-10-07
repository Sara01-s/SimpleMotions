namespace SimpleMotions {

	public sealed class VideoTimeline : IVideoTimeline {

		private readonly IVideoPlayer _videoPlayer;

		public VideoTimeline(IVideoPlayer videoPlayer) {
			_videoPlayer = videoPlayer;
		}

		public void Play() {
			UnityEngine.Debug.Log("Play");
			_videoPlayer.Play();
		}

		public void Resume() {
			Play();
		}

		public void Pause() {
			UnityEngine.Debug.Log("Pause");
			_videoPlayer.Pause();
		}

		public void SetTime(float newCurrentTime) {
			_videoPlayer.SetCurrentTime(newCurrentTime);
		}
		
	}
}