using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IEntitySelector {

		Entity SelectedEntity { get; }

		void SelectEntity(int entityId);
		void DeselectEntity();
		bool HasSelectedEntity();

		ReactiveCommand<Entity> OnEntitySelected { get; }
		ReactiveCommand OnEntityDeselected { get; }
		
    }

	public class EntitySelector : IEntitySelector {
		public Entity SelectedEntity { get; private set; }

		private readonly IEntityViewModel _entityViewModel;

		public ReactiveCommand<Entity> OnEntitySelected { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();

		public EntitySelector(EntitiesData entitiesData, IEntityViewModel entityViewModel) {
			_entityViewModel = entityViewModel;
			SelectedEntity = entitiesData.SelectedEntity;
		}

		public void SelectEntity(int entityId) {
			SelectedEntity = _entityViewModel.GetEntity(entityId);
			UnityEngine.Debug.Log($"Entidad {entityId} seleccionada.");
			OnEntitySelected.Execute(SelectedEntity);
		}

		public void DeselectEntity() {
			SelectedEntity = Entity.Invalid;
			OnEntityDeselected.Execute();
		}

		public bool HasSelectedEntity() {
			return SelectedEntity.Id != Entity.Invalid.Id;
		}

	}
}