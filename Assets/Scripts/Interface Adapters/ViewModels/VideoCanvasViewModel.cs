using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvasViewModel : IEntityViewModel {

		ReactiveCommand<(int, string)> OnCanvasUpdate { get; }
		ReactiveCommand<int> OnEntitySelected { get; }
		ReactiveCommand<Void> OnEntityDeselected { get; }

	}

    public sealed class VideoCanvasViewModel : EntityViewModel, IVideoCanvasViewModel {

		public ReactiveCommand<(int, string)> OnCanvasUpdate { get; } = new();
		public ReactiveCommand<int> OnEntitySelected { get; } = new();
		public ReactiveCommand<Void> OnEntityDeselected { get; } = new();

		public VideoCanvasViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator, IEntitySelector entitySelector) : base(videoCanvas) {
			videoCanvas.EntityDisplayInfo.Subscribe(UpdateCanvas);
			videoAnimator.EntityDisplayInfo.Subscribe(UpdateCanvas);
			
			entitySelector.OnEntitySelected.Subscribe(SelectEntity);
			entitySelector.OnEntityDeselected.Subscribe(DeselectEntity);
        }

		private void SelectEntity(Entity entity) {
			OnEntitySelected.Execute(entity.Id);
		}

		private void DeselectEntity(Void _) {
			OnEntityDeselected.Execute(value: null);
		}

		private void UpdateCanvas((int id, string name) entity) {
			OnCanvasUpdate.Execute(entity);
		}

	}
}