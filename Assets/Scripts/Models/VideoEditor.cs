namespace SimpleMotions {

	internal sealed class VideoEditor {

		private readonly IVideoTimeline _videoTimeline;
		private readonly VideoPlayer _videoPlayer;
		private readonly VideoCanvas _videoCanvas;

		internal VideoEditor(IVideoTimeline videoTimeline, VideoPlayer videoPlayer, VideoCanvas videoCanvas) {
			_videoTimeline = videoTimeline;
			_videoPlayer = videoPlayer;
			_videoCanvas = videoCanvas;
		}

		internal IVideoTimeline GetVideoTimeline() {
			return _videoTimeline;
		}

	}
	
}