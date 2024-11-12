using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvas {

		void DisplayEntity(Entity entity);
		void DisplayEntity(int entityId);
		bool EntityHasComponent<T>(int entityId) where T : Component;
		T GetEntityComponent<T>(int entityId) where T : Component;
		ReactiveValue<(int, string)> EntityDisplayInfo { get; }

	}

	public sealed class VideoCanvas : IVideoCanvas {

		private readonly IComponentStorage _componentStorage;
		private readonly IEntityStorage _entityStorage;

        public ReactiveValue<(int, string)> EntityDisplayInfo { get; } = new();

        public VideoCanvas(IComponentStorage componentStorage, IEntityStorage entityStorage) {
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;
		}

		public bool EntityHasComponent<T>(int entityId) where T : Component {
			return _componentStorage.HasComponent<T>(entityId);
		}

		public T GetEntityComponent<T>(int entityId) where T : Component {
			return _componentStorage.GetComponent<T>(entityId);
		}

		public void DisplayEntity(int entityId) {
			var entity = _entityStorage.GetEntity(entityId);
			DisplayEntity(entity);
		}

		public void DisplayEntity(Entity entity) {
			EntityDisplayInfo.Value = (entity.Id, entity.Name);
		}

	}
}