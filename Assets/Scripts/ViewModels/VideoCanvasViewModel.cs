namespace SimpleMotions {

	public interface IVideoCanvasViewModel {

		ReactiveCommand<EntityDisplayInfo> UpdateCanvas { get; set; }

	}

    public sealed class VideoCanvasViewModel : IVideoCanvasViewModel {

        public ReactiveCommand<EntityDisplayInfo> UpdateCanvas { get; set; } = new();

        public VideoCanvasViewModel(IEventService eventService) {
			eventService.Subscribe<EntityDisplayInfo>(OnCanvasUpdate); // TODO - Refactor event/command names
        }

		private void OnCanvasUpdate(EntityDisplayInfo entityDisplayInfo) {
			UpdateCanvas.Execute(entityDisplayInfo);
		}

	}
}