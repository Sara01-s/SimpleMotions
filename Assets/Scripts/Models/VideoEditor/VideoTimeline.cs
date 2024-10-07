namespace SimpleMotions {

	public sealed class VideoTimeline : IVideoTimeline {

		private readonly IVideoPlayer _videoPlayer;

		public VideoTimeline(IVideoPlayer videoPlayer) {
			_videoPlayer = videoPlayer;
		}

		public void SetTime(float newCurrentTime) {
			_videoPlayer.SetCurrentTime(newCurrentTime);
		}
		
	}
}