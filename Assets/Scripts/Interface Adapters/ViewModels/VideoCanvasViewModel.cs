namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IEntityViewModel {

		ReactiveCommand<(int, string)> OnCanvasUpdate { get; }

	}

    public sealed class VideoCanvasViewModel : EntityViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<(int, string)> OnCanvasUpdate { get; } = new();

        public VideoCanvasViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator) : base(videoCanvas) {
			videoCanvas.EntityDisplayInfo.Subscribe(UpdateCanvas);
			videoAnimator.EntityDisplayInfo.Subscribe(UpdateCanvas);
        }

		private void UpdateCanvas((int id, string name) entity) {
			OnCanvasUpdate.Execute(entity);
		}

	}
}