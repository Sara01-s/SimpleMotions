namespace SimpleMotions {

	internal sealed class VideoEditor {

		private readonly IVideoTimeline _videoTimeline;
		private readonly VideoPlayer _videoPlayer;
		private readonly VideoCanvas _videoCanvas;

		internal VideoEditor() {
			var componentDatabse = Services.Instance.GetService<IComponentDatabase>();
			var videoDatabase = Services.Instance.GetService<IVideoDatabase>();

			_videoTimeline = new VideoTimeline(videoDatabase);
			_videoPlayer = new VideoPlayer(componentDatabse);
			_videoCanvas = new VideoCanvas(componentDatabse);
		}

	}
	
}