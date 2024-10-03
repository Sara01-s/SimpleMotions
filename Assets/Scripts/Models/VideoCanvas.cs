using System.Linq;

namespace SimpleMotions {

	internal sealed class VideoCanvas {

		private readonly IEntityStorage _entityStorage;
		private readonly IEventService _eventService;

		internal VideoCanvas(IEntityStorage entityStorage, IEventService eventService) {
			_entityStorage = entityStorage;
			_eventService = eventService;
		}

		public void UpdateCanvas(Entity entity) {
			_eventService.Dispatch(entity);
		}

	}
}