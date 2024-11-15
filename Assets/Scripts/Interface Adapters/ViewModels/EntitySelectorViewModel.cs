using SimpleMotions.Internal;

namespace SimpleMotions {


	public interface IEntitySelectorViewModel : IEntityViewModel {

		ReactiveCommand<(int, string)> OnShowSelectionGizmo { get; }
		ReactiveCommand OnHideSelectionGizmo { get; }

	}

	public class EntitySelectorViewModel : EntityViewModel, IEntitySelectorViewModel {

		public ReactiveCommand<(int, string)> OnShowSelectionGizmo { get; } = new();
		public ReactiveCommand OnHideSelectionGizmo { get; } = new();

		public EntitySelectorViewModel(IEntitySelector entitySelector, IVideoCanvas videoCanvas) : base(videoCanvas) {
			entitySelector.OnEntitySelected.Subscribe(ShowSelectionGizmo);
			entitySelector.OnEntityDeselected.Subscribe(HideSelectionGizmo);
        }

        private void ShowSelectionGizmo(Entity entity) {
            OnShowSelectionGizmo.Execute((entity.Id, entity.Name));
        }

		private void HideSelectionGizmo() {
			OnHideSelectionGizmo.Execute();
		}

	}
}