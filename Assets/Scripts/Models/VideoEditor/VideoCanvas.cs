using SimpleMotions.Internal;

namespace SimpleMotions {

	public struct EntityDisplayInfo {
		public int EntityId;
		public string EntityName;
	}

	public interface IVideoCanvas {

		void UpdateCanvas(Entity entity);
		bool EntityHasComponent<T>(int entityId) where T : Component;
		T GetEntityComponent<T>(int entityId) where T : Component;

	}

	public sealed class VideoCanvas : IVideoCanvas {

		private readonly IComponentStorage _componentStorage;
		private readonly IEventService _eventService;

		public VideoCanvas(IComponentStorage componentStorage, IEventService eventService) {
			_componentStorage = componentStorage;
			_eventService = eventService;
		}

		public bool EntityHasComponent<T>(int entityId) where T : Component {
			return _componentStorage.HasComponent<T>(entityId);
		}

		public T GetEntityComponent<T>(int entityId) where T : Component {
			return _componentStorage.GetComponent<T>(entityId);
		}

		public void UpdateCanvas(Entity entity) {
			var entityDisplayInfo = new EntityDisplayInfo {
				EntityId = entity.Id,
				EntityName = entity.Name,
			};

			_eventService.Dispatch(entityDisplayInfo);
		}

	}
}