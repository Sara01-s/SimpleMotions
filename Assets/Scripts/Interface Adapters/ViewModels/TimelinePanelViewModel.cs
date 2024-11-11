namespace SimpleMotions {

	public interface ITimelinePanelViewModel {
		void CreateEntity();
	}

	public class TimelinePanelViewModel : ITimelinePanelViewModel {

		private readonly IVideoEntities _videoEntities;

		public TimelinePanelViewModel(IVideoEntities videoEntities) {
			_videoEntities = videoEntities;
		}

		public void CreateEntity() {
			_videoEntities.CreateEntity();
		}

	}
}