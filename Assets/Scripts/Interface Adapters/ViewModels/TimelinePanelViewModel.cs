namespace SimpleMotions {

	public interface ITimelinePanelViewModel {
		public ReactiveCommand ShowMaxEntitiesWarning { get; }
		public ReactiveCommand<int> DeleteEntity { get; }
		void TryCreateEntity();
		void CreateEntity();
	}

	public class TimelinePanelViewModel : ITimelinePanelViewModel {
		
		public ReactiveCommand ShowMaxEntitiesWarning { get; } = new();
		public ReactiveCommand<int> DeleteEntity { get; } = new();

		private readonly IVideoEntities _videoEntities;

		public TimelinePanelViewModel(IVideoEntities videoEntities) {
			_videoEntities = videoEntities;
			videoEntities.ShowMaxEntitiesWarning.Subscribe(ShowMaxEntitiesWarning.Execute);
			DeleteEntity.Subscribe(videoEntities.DeleteEntity);
		}

		public void CreateEntity() {
			_videoEntities.CreateEntity();
		}

        public void TryCreateEntity() {
			_videoEntities.TryCreateEntity();
        }
    }
}