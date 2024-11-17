using System.Runtime.InteropServices;
using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IComponentViewModel {

		ReactiveCommand<(int, string)> OnCanvasUpdate { get; }
		ReactiveCommand<int> OnEntitySelected { get; }
		ReactiveCommand<int> OnEntityRemoved { get; }
		ReactiveCommand OnEntityDeselected { get; }
		ReactiveCommand<(float r, float g, float b, float a)> BackgroundColorUpdated { get; }

		void ChangeBackgroundColor(Color color);

	}

    public sealed class VideoCanvasViewModel : ComponentViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<(int, string)> OnCanvasUpdate { get; } = new();
		public ReactiveCommand<int> OnEntitySelected { get; } = new();
		public ReactiveCommand<int> OnEntityRemoved { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();
		public ReactiveCommand<(float r, float g, float b, float a)> BackgroundColorUpdated { get; } = new();

		private IVideoCanvas _videoCanvas;

		public VideoCanvasViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator, IEntitySelector entitySelector) : base(videoCanvas) {
			_videoCanvas = videoCanvas;

			_videoCanvas.EntityDisplayInfo.Subscribe(UpdateCanvas);
			_videoCanvas.OnEntityRemoved.Subscribe(entityId => OnEntityRemoved.Execute(entityId));
			videoAnimator.EntityDisplayInfo.Subscribe(UpdateCanvas);
			
			entitySelector.OnEntitySelected.Subscribe(entity => OnEntitySelected.Execute(entity.Id));
			entitySelector.OnEntityDeselected.Subscribe(() => OnEntityDeselected.Execute());
        }

		private void UpdateCanvas((int id, string name) entity) {
			OnCanvasUpdate.Execute(entity);
		}

        public void ChangeBackgroundColor(Color color) {
			_videoCanvas.SetBackgroundColor(color);
			BackgroundColorUpdated.Execute((color.R, color.G, color.B, color.A));
        }


    }
}