namespace SimpleMotions {

	public sealed class VideoCanvas {

		private readonly IEntityStorage _entityStorage;
		private readonly IEventService _eventService;

		public VideoCanvas(IEntityStorage entityStorage, IEventService eventService) {
			_entityStorage = entityStorage;
			_eventService = eventService;
		}

		public void UpdateCanvas(Entity entity) {
			_eventService.Dispatch(entity);
		}

	}
}