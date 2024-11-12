using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IEntitySelector {

		Entity SelectedEntity { get; }

		void SelectEntity(int entityId);
		void DeselectEntity();
		bool HasSelectedEntity();

		ReactiveCommand<Entity> OnEntitySelected { get; }
		ReactiveCommand<Void> OnEntityDeselected { get; }
		
    }

	public class EntitySelector : IEntitySelector {
		public Entity SelectedEntity { get; private set; }

		private readonly IEntityStorage _entityStorage;

		public ReactiveCommand<Entity> OnEntitySelected { get; } = new();
		public ReactiveCommand<Void> OnEntityDeselected { get; } = new();

		public EntitySelector(EntitiesData entitiesData, IEntityStorage entityStorage) {
			_entityStorage = entityStorage;
			SelectedEntity = entitiesData.SelectedEntity;
		}

		public void SelectEntity(int entityId) {
			SelectedEntity = _entityStorage.GetEntity(entityId);
			UnityEngine.Debug.Log($"Entidad {entityId} seleccionada.");
			OnEntitySelected.Execute(SelectedEntity);
		}

		public void DeselectEntity() {
			SelectedEntity = Entity.Invalid;
			OnEntityDeselected.Execute(value: null);
		}

		public bool HasSelectedEntity() {
			return SelectedEntity.Id != Entity.Invalid.Id;
		}

	}
}