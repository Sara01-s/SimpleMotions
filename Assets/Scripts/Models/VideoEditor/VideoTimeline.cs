namespace SimpleMotions {

	public interface IVideoTimeline {
		
		void SetCurrentFrame(int frame);

	}

	public sealed class VideoTimeline : IVideoTimeline {

		private readonly IVideoPlayer _videoPlayer;

		public VideoTimeline(IVideoPlayer videoPlayer) {
			_videoPlayer = videoPlayer;
		}

		public void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
		}
	}
}