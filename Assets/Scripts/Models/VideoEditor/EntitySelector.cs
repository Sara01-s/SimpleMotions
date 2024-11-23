using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IEntitySelector {

		Entity SelectedEntity { get; }
		bool HasSelectedEntity { get; }

		void SelectEntity(int entityId);
		void DeselectEntity();
		bool TryGetSelectedEntityId(out int selectedEntityId);

		ReactiveCommand<Entity> OnEntitySelected { get; }
		ReactiveCommand OnEntityDeselected { get; }
		
    }

	public class EntitySelector : IEntitySelector {
		
		public Entity SelectedEntity { get; private set; }
		public ReactiveCommand<Entity> OnEntitySelected { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();

		public bool HasSelectedEntity => SelectedEntity.Id != Entity.Invalid.Id;

		private readonly IEntityViewModel _entityViewModel;

		public EntitySelector(EntitiesData entitiesData, IEntityViewModel entityViewModel) {
			_entityViewModel = entityViewModel;
			SelectedEntity = entitiesData.SelectedEntity;
		}

		public void SelectEntity(int entityId) {
			SelectedEntity = _entityViewModel.GetEntity(entityId);
			OnEntitySelected.Execute(SelectedEntity);
			UnityEngine.Debug.Log($"Entidad {entityId} seleccionada.");
		}

		public void DeselectEntity() {
			SelectedEntity = Entity.Invalid;
			OnEntityDeselected.Execute();
		}

		public bool TryGetSelectedEntityId(out int selectedEntityId) {
			int entityId = SelectedEntity.Id;
			int invalidId = Entity.Invalid.Id;

			if (entityId == invalidId) {
				selectedEntityId = invalidId;
				return false;
			}

			selectedEntityId = entityId;
			return true;
		}

	}
}