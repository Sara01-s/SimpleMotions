namespace SimpleMotions {

	public struct EntityDisplayInfo {
		public Entity Entity;
		public Component[] Components;
	}

	public sealed class VideoCanvas {

		private readonly IComponentStorage _componentStorage;
		private readonly IEventService _eventService;

		public VideoCanvas(IComponentStorage componentStorage, IEventService eventService) {
			_componentStorage = componentStorage;
			_eventService = eventService;
		}

		public void UpdateCanvas(Entity entity) {
			var components = _componentStorage.GetAllComponents(entity.Id);
			var entityDisplayInfo = new EntityDisplayInfo {
				Entity = entity,
				Components = components
			};

			_eventService.Dispatch<EntityDisplayInfo>(entityDisplayInfo);
		}

	}
}