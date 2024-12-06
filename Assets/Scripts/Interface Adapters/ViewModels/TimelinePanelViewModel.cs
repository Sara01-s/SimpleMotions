#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
using System.Windows.Forms;
#endif

namespace SimpleMotions {

	public interface ITimelinePanelViewModel {

		public ReactiveCommand ShowMaxEntitiesWarning { get; }
		public ReactiveCommand<EntityDTO> OnEntityNameChanged { get ; }
		public ReactiveCommand<int> DeleteEntity { get; }
		public ReactiveCommand OnEntityCreated { get; }

		void ChangeEntityName(int entityId, string newName);
		void ToggleEntityActive(int entityId, bool active);
		bool TryCreateEntity(out int createdEntityId);
		string GetEntityName(int entityId);

	}

	public class TimelinePanelViewModel : ITimelinePanelViewModel {
		
		public ReactiveCommand ShowMaxEntitiesWarning { get; } = new();
		public ReactiveCommand<int> DeleteEntity { get; } = new();
		public ReactiveValue<string> EntityName { get; } = new();
		public ReactiveCommand<EntityDTO> OnEntityNameChanged { get; } = new();
		public ReactiveCommand OnEntityCreated { get; set; } = new();

		private readonly IVideoEntities _videoEntities;
		private readonly IEntityViewModel _entityViewModel;
		private readonly IEntitySelector _entitySelector;

		public TimelinePanelViewModel(IVideoEntities videoEntities, IEntityViewModel entityViewModel, 
									  IEntitySelector entitySelector, IInspectorViewModel inspectorViewModel) {
			videoEntities.ShowMaxEntitiesWarning.Subscribe(ShowMaxEntitiesWarning.Execute);
			entityViewModel.OnEntityNameChanged.Subscribe(OnEntityNameChanged.Execute);

			DeleteEntity.Subscribe(videoEntities.DeleteEntity);
			DeleteEntity.Subscribe(_ => inspectorViewModel.OnClearInspector.Execute());

			_entityViewModel = entityViewModel;
			_videoEntities = videoEntities;
			_entitySelector = entitySelector;
		}

		public void CreateEntity() {
			_videoEntities.CreateEntity();
		}

        public bool TryCreateEntity(out int createdEntityId) {
			createdEntityId = _videoEntities.TryCreateEntity();

			
			if (createdEntityId != Internal.Entity.Invalid.Id) {
				OnEntityCreated.Execute();
				return true;
			}

			return false;
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