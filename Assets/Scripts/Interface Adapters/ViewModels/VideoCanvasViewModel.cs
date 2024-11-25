using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IComponentViewModel {

		ReactiveCommand<EntityDTO> OnCanvasUpdate { get; }
		ReactiveCommand<int> OnEntitySelected { get; }
		ReactiveCommand<int> OnEntityRemoved { get; }
		ReactiveCommand OnEntityDeselected { get; }
		ReactiveCommand<int, string> OnDisplayEntityImage { get; }

		// TODO - Use DTO.
		ReactiveValue<(float r, float g, float b, float a)> BackgroundColor { get; }

	}

    public sealed class VideoCanvasViewModel : ComponentViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<EntityDTO> OnCanvasUpdate { get; } = new();
		public ReactiveCommand<int> OnEntitySelected { get; } = new();
		public ReactiveCommand<int> OnEntityRemoved { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();
		public ReactiveCommand<int, string> OnDisplayEntityImage { get; } = new();
		public ReactiveValue<(float r, float g, float b, float a)> BackgroundColor { get; } = new();

		private readonly IVideoCanvas _videoCanvas;

		public VideoCanvasViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator, IEntitySelector entitySelector) : base(videoCanvas) {
			videoCanvas.EntityDisplayInfo.Subscribe(entity => UpdateCanvas(new EntityDTO(entity.Id, entity.Name)));
			videoCanvas.OnEntityRemoved.Subscribe(entityId => OnEntityRemoved.Execute(entityId));
			videoCanvas.OnSetEntityImage.Subscribe(OnDisplayEntityImage.Execute);

			videoAnimator.EntityDisplayInfo.Subscribe(entity => UpdateCanvas(new EntityDTO(entity.Id, entity.Name)));
			
			entitySelector.OnEntitySelected.Subscribe(entity => OnEntitySelected.Execute(entity.Id));
			entitySelector.OnEntityDeselected.Subscribe(() => OnEntityDeselected.Execute());

			BackgroundColor.Subscribe(SetBackgroundColor);
			
			_videoCanvas = videoCanvas;
        }

		private void UpdateCanvas(EntityDTO entityDTO) {
			OnCanvasUpdate.Execute(entityDTO);
		}

        private void SetBackgroundColor((float r, float g, float b, float a) color) {
			var newColor = new Color(color.r, color.g, color.b, color.a);
			_videoCanvas.SetBackgroundColor(newColor);
        }

    }
}