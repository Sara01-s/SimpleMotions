using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvas {

		void DisplayEntity(Entity entity);
		bool EntityHasComponent<T>(int entityId) where T : Component;
		T GetEntityComponent<T>(int entityId) where T : Component;
		ReactiveValue<(int, string)> EntityDisplayInfo { get; }

	}

	public sealed class VideoCanvas : IVideoCanvas {

		private readonly IComponentStorage _componentStorage;

        public ReactiveValue<(int, string)> EntityDisplayInfo { get; } = new();

        public VideoCanvas(IComponentStorage componentStorage) {
			_componentStorage = componentStorage;
		}

		public bool EntityHasComponent<T>(int entityId) where T : Component {
			return _componentStorage.HasComponent<T>(entityId);
		}

		public T GetEntityComponent<T>(int entityId) where T : Component {
			return _componentStorage.GetComponent<T>(entityId);
		}

		public void DisplayEntity(Entity entity) {
			EntityDisplayInfo.Value = (entity.Id, entity.Name);
		}

	}
}