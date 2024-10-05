namespace SimpleMotions {

    public sealed class VideoCanvasViewModel : IVideoCanvasViewModel {

        public ReactiveCommand<EntityDisplayInfo> UpdateCanvas { get; set; } = new();

        public VideoCanvasViewModel(IEventService eventService) {
			eventService.Subscribe<EntityDisplayInfo>(OnCanvasUpdate);
        }

		private void OnCanvasUpdate(EntityDisplayInfo entityDisplayInfo) {
			UpdateCanvas.Execute(entityDisplayInfo);
		}

	}
}