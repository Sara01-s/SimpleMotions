using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IComponentViewModel {

		ReactiveCommand<int, int> SetEntitySortingIndex { get; } // int entityId, int sortingIndex.
		ReactiveCommand<EntityDTO> OnCanvasUpdate { get; }
		ReactiveCommand<int> OnEntitySelected { get; }
		ReactiveCommand<int> OnEntityRemoved { get; }
		ReactiveCommand OnEntityDeselected { get; }
		ReactiveCommand<int, string> OnDisplayEntityImage { get; }

		ReactiveValue<ColorDTO> BackgroundColor { get; }

	}

    public sealed class VideoCanvasViewModel : ComponentViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<int, int> SetEntitySortingIndex { get; } = new(); // int entityId, int sortingIndex
		public ReactiveCommand<int, int> DecreaseEntityLayer { get; } = new(); // int entityId, int sortingIndex
		public ReactiveCommand<EntityDTO> OnCanvasUpdate { get; } = new();
		public ReactiveCommand<int> OnEntitySelected { get; } = new();
		public ReactiveCommand<int> OnEntityRemoved { get; } = new();
		public ReactiveCommand OnEntityDeselected { get; } = new();
		public ReactiveCommand<int, string> OnDisplayEntityImage { get; } = new();
		public ReactiveValue<ColorDTO> BackgroundColor { get; } = new();

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

        private void SetBackgroundColor(ColorDTO color) {
			var newColor = new Color(color.R, color.G, color.B, color.A);
			_videoCanvas.SetBackgroundColor(newColor);
        }

    }
}