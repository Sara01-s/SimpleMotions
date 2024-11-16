namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IComponentViewModel {

		ReactiveCommand<(int, string)> OnCanvasUpdate { get; }
		ReactiveCommand<int> OnEntitySelected { get; }
		ReactiveCommand<int> OnEntityRemoved { get; }
		ReactiveCommand OnEntityDeselected { get; }

	}

    public sealed class VideoCanvasViewModel : ComponentViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<(int, string)> OnCanvasUpdate { get; } = new();
		public ReactiveCommand<int> OnEntitySelected { get; } = new();
		public ReactiveCommand<int> OnEntityRemoved { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();

		public VideoCanvasViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator, IEntitySelector entitySelector) : base(videoCanvas) {
			videoCanvas.EntityDisplayInfo.Subscribe(UpdateCanvas);
			videoCanvas.OnEntityRemoved.Subscribe(entityId => OnEntityRemoved.Execute(entityId));
			videoAnimator.EntityDisplayInfo.Subscribe(UpdateCanvas);
			
			entitySelector.OnEntitySelected.Subscribe(entity => OnEntitySelected.Execute(entity.Id));
			entitySelector.OnEntityDeselected.Subscribe(() => OnEntityDeselected.Execute());
        }

		private void UpdateCanvas((int id, string name) entity) {
			OnCanvasUpdate.Execute(entity);
		}

	}
}