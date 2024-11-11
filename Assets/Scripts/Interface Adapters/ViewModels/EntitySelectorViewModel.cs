using SimpleMotions.Internal;

namespace SimpleMotions {


	public interface IEntitySelectorViewModel : IEntityViewModel {

		ReactiveCommand<(int, string)> OnShowSelectionGizmo { get; }
		ReactiveCommand<Void> OnHideSelectionGizmo { get; }

	}

	public class EntitySelectorViewModel : EntityViewModel, IEntitySelectorViewModel {

		public ReactiveCommand<(int, string)> OnShowSelectionGizmo { get; } = new();
		public ReactiveCommand<Void> OnHideSelectionGizmo { get; } = new();

		public EntitySelectorViewModel(IEntitySelector entitySelector, IVideoCanvas videoCanvas) : base(videoCanvas) {
			entitySelector.OnEntitySelected.Subscribe(ShowSelectionGizmo);
			entitySelector.OnEntityDeselected.Subscribe(HideSelectionGizmo);
        }

        private void ShowSelectionGizmo(Entity entity) {
            OnShowSelectionGizmo.Execute((entity.Id, entity.Name));
        }

		private void HideSelectionGizmo(Void _) {
			OnHideSelectionGizmo.Execute(value: null);
		}

	}
}