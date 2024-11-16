namespace SimpleMotions {

	public interface ITimelinePanelViewModel {

		public ReactiveCommand ShowMaxEntitiesWarning { get; }
		public ReactiveCommand<int, string> OnEntityNameChanged { get ; }
		public ReactiveCommand<int> DeleteEntity { get; }

		void ChangeEntityName(int entityId, string newName);
		void ToggleEntityActive(int entityId, bool active);
		bool TryCreateEntity(out int createdEntityId);
		string GetEntityName(int entityId);

	}

	public class TimelinePanelViewModel : ITimelinePanelViewModel {
		
		public ReactiveCommand ShowMaxEntitiesWarning { get; } = new();
		public ReactiveCommand<int> DeleteEntity { get; } = new();
		public ReactiveValue<string> EntityName { get; } = new();
		public ReactiveCommand<int, string> OnEntityNameChanged { get; } = new();

		private readonly IVideoEntities _videoEntities;
		private readonly IEntityViewModel _entityViewModel;
		private readonly IEntitySelector _entitySelector;

		public TimelinePanelViewModel(IVideoEntities videoEntities, IEntityViewModel entityViewModel, IEntitySelector entitySelector) {
			videoEntities.ShowMaxEntitiesWarning.Subscribe(ShowMaxEntitiesWarning.Execute);
			DeleteEntity.Subscribe(videoEntities.DeleteEntity);
			entityViewModel.OnEntityNameChanged.Subscribe((id, name) => OnEntityNameChanged.Execute(id, name));

			_entityViewModel = entityViewModel;
			_videoEntities = videoEntities;
			_entitySelector = entitySelector;
		}

		public void CreateEntity() {
			_videoEntities.CreateEntity();
		}

        public bool TryCreateEntity(out int createdEntityId) {
			createdEntityId = _videoEntities.TryCreateEntity();
			return createdEntityId != Internal.Entity.Invalid.Id;
        }

		public void ToggleEntityActive(int entityId, bool active) {
			_entityViewModel.ToggleEntityActive(entityId, active);
		}

		public string GetEntityName(int entityId) {
			return _entityViewModel.GetEntity(entityId).Name;
		}

		public void ChangeEntityName(int entityId, string newName) {
			_entitySelector.SelectEntity(entityId);
			_entityViewModel.ChangeEntityName(entityId, newName);
		}

	}
}