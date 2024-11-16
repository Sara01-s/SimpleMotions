using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IEntityViewModel {

		void ChangeEntityName(int entityId, string newName);
		void ToggleEntityActive(int entityId, bool isActive);
		Entity GetEntity(int entityId);

		ReactiveCommand<int, string> OnEntityNameChanged { get; }

	}

	public class EntityViewModel : IEntityViewModel {

		private readonly IEntityStorage _entityStorage;
		public ReactiveCommand<int, string> OnEntityNameChanged { get; } = new();

		public EntityViewModel(IEntityStorage entityStorage) {
			_entityStorage = entityStorage;
		}

		public Entity GetEntity(int entityId) {
			return _entityStorage.GetEntity(entityId);
		}

		public void ChangeEntityName(int entityId, string newName) {
			_entityStorage.GetEntity(entityId).Name = newName;
			OnEntityNameChanged.Execute(entityId, newName);
		}

		public void ToggleEntityActive(int entityId, bool isActive) {
			_entityStorage.GetEntity(entityId).IsActive = isActive;
		}
	}

}