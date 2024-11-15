
namespace SimpleMotions {

	public interface IVideoTimeline {
		
		int TotalFrames { get; }
		void SetCurrentFrame(int frame);

	}

	public sealed class VideoTimeline : IVideoTimeline {

		private readonly IVideoPlayer _videoPlayer;
		private readonly IVideoPlayerData _videoPlayerData;

		public int TotalFrames => _videoPlayerData.TotalFrames.Value;

		public VideoTimeline(IVideoPlayer videoPlayer, IVideoPlayerData videoPlayerData) {
			_videoPlayer = videoPlayer;
			_videoPlayerData = videoPlayerData;
		}

		public void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
		}

	}
}