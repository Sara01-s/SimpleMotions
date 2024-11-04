namespace SimpleMotions {

	public interface IVideoTimeline {
		
		int DefaultKeyframes { get; }
		void SetCurrentFrame(int frame);

	}

	public sealed class VideoTimeline : IVideoTimeline {

		private TimelineData _timelineData;
		private readonly IVideoPlayer _videoPlayer;

        public int DefaultKeyframes => _timelineData.DefaultKeyframes;

        public VideoTimeline(TimelineData timelineData, IVideoPlayer videoPlayer) {
			_timelineData = timelineData;
			_videoPlayer = videoPlayer;
		}

		public void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
		}

	}
}