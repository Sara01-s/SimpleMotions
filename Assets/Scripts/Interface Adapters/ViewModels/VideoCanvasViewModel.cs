using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IComponentViewModel {

		ReactiveCommand<(int, string)> OnCanvasUpdate { get; }
		ReactiveCommand<int> OnEntitySelected { get; }
		ReactiveCommand<int> OnEntityRemoved { get; }
		ReactiveCommand OnEntityDeselected { get; }

		ReactiveValue<(float r, float g, float b, float a)> BackgroundColor { get; }

	}

    public sealed class VideoCanvasViewModel : ComponentViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<(int, string)> OnCanvasUpdate { get; } = new();
		public ReactiveCommand<int> OnEntitySelected { get; } = new();
		public ReactiveCommand<int> OnEntityRemoved { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();

		public ReactiveValue<(float r, float g, float b, float a)> BackgroundColor { get; } = new();

		private readonly IVideoCanvas _videoCanvas;

		public VideoCanvasViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator, IEntitySelector entitySelector) : base(videoCanvas) {
			_videoCanvas = videoCanvas;

			_videoCanvas.EntityDisplayInfo.Subscribe(UpdateCanvas);
			_videoCanvas.OnEntityRemoved.Subscribe(entityId => OnEntityRemoved.Execute(entityId));
			videoAnimator.EntityDisplayInfo.Subscribe(UpdateCanvas);
			
			entitySelector.OnEntitySelected.Subscribe(entity => OnEntitySelected.Execute(entity.Id));
			entitySelector.OnEntityDeselected.Subscribe(() => OnEntityDeselected.Execute());

			BackgroundColor.Subscribe(SetBackgroundColor);
        }

		private void UpdateCanvas((int id, string name) entity) {
			OnCanvasUpdate.Execute(entity);
		}

        private void SetBackgroundColor((float r, float g, float b, float a) color) {
			var newColor = new Color(color.r, color.g, color.b, color.a);
			_videoCanvas.SetBackgroundColor(newColor);
        }

    }
}