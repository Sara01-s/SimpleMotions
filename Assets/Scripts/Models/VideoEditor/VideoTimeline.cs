using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoTimeline {
		
		int TotalFrames { get; }
		void SetCurrentFrame(int frame);

	}

	public sealed class VideoTimeline : IVideoTimeline {

		private readonly VideoData _videoData;
		private readonly IVideoPlayer _videoPlayer;

		public int TotalFrames => _videoData.TotalFrames;

		public VideoTimeline(VideoData videoData, IVideoPlayer videoPlayer) {
			_videoData = videoData;
			_videoPlayer = videoPlayer;
		}

		public void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
		}

	}
}