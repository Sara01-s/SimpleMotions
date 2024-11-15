namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IEntityViewModel {

		ReactiveCommand<(int, string)> OnCanvasUpdate { get; }
		ReactiveCommand<int> OnEntitySelected { get; }
		ReactiveCommand OnEntityDeselected { get; }
		ReactiveCommand OnEntityRemoved { get; }

	}

    public sealed class VideoCanvasViewModel : EntityViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<(int, string)> OnCanvasUpdate { get; } = new();
		public ReactiveCommand<int> OnEntitySelected { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();
		public ReactiveCommand OnEntityRemoved { get; } = new();

		public VideoCanvasViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator, IEntitySelector entitySelector) : base(videoCanvas) {
			videoCanvas.EntityDisplayInfo.Subscribe(UpdateCanvas);
			videoCanvas.OnEntityRemoved.Subscribe(() => OnEntityRemoved.Execute());
			videoAnimator.EntityDisplayInfo.Subscribe(UpdateCanvas);
			
			entitySelector.OnEntitySelected.Subscribe(entity => OnEntitySelected.Execute(entity.Id));
			entitySelector.OnEntityDeselected.Subscribe(() => OnEntityDeselected.Execute());
        }

		private void UpdateCanvas((int id, string name) entity) {
			OnCanvasUpdate.Execute(entity);
		}

	}
}