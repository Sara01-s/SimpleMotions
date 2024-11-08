namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IEntityViewModel {

		ReactiveCommand<EntityDisplayInfo> OnCanvasUpdate { get; }

	}

    public sealed class VideoCanvasViewModel : EntityViewModel, IVideoCanvasViewModel {

        public ReactiveCommand<EntityDisplayInfo> OnCanvasUpdate { get; } = new();

        public VideoCanvasViewModel(IVideoCanvas videoCanvas, IEventService eventService) : base(videoCanvas) {
			eventService.Subscribe<EntityDisplayInfo>(UpdateCanvas);
        }

		private void UpdateCanvas(EntityDisplayInfo entityDisplayInfo) {
			OnCanvasUpdate.Execute(entityDisplayInfo);
		}

	}
}