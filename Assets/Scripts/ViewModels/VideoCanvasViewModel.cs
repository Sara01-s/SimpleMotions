namespace SimpleMotions {

    internal sealed class VideoCanvasViewModel : IVideoCanvasViewModel {

        public ReactiveCommand<Entity> UpdateCanvas { get; set; } = new();

        public VideoCanvasViewModel(IEventService eventService) {
			eventService.Subscribe<Entity>(OnCanvasUpdate);
        }

		private void OnCanvasUpdate(Entity entity) {
			UpdateCanvas.Execute(entity);
		}

	}
}