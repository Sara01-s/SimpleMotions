namespace SimpleMotions {

	public interface IEntitySelectorViewModel : IEntityViewModel {

		ReactiveCommand<(int, string)> OnEntitySelected { get; }
        ReactiveCommand<Void> OnEntityDeselected { get; }
		((float x, float y) min, (float x, float y) max) GetEntityBounds(int entityId);

	}

	public class EntitySelectorViewModel : EntityViewModel, IEntitySelectorViewModel {

		public ReactiveCommand<(int, string)> OnEntitySelected { get; } = new();
		public ReactiveCommand<Void> OnEntityDeselected { get; } = new(); // ¿Qué es esto?

		public EntitySelectorViewModel(IVideoCanvas videoCanvas) : base(videoCanvas) {
			videoCanvas.EntityDisplayInfo.Subscribe(UpdateRectSelection);
        }

        private void UpdateRectSelection((int id, string name) entity) {
            OnEntitySelected.Execute(entity);
        }

		public ((float x, float y) min, (float x, float y) max) GetEntityBounds(int entityId) {
			if (EntityHasTransform(entityId, out var transformData)) {
				var (pos, scale, angle) = transformData;
				return SmMath.calculateBounds(pos, scale, angle);
			}

			return ((-1.0f, -1.0f), (1.0f, 1.0f));
		}

	}
}