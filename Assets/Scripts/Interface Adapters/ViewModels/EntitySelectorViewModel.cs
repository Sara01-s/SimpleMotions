namespace SimpleMotions {

	public interface IEntitySelectorViewModel : IEntityViewModel {
        ReactiveCommand<EntityDisplayInfo> OnEntitySelected { get; }
        ReactiveCommand<Void> OnEntityDeselected { get; }
		((float x, float y) min, (float x, float y) max) GetEntityBounds(int entityId);
	}

	public class EntitySelectorViewModel : EntityViewModel, IEntitySelectorViewModel {

        public ReactiveCommand<EntityDisplayInfo> OnEntitySelected { get; } = new();
		public ReactiveCommand<Void> OnEntityDeselected { get; } = new();

		public EntitySelectorViewModel(IVideoCanvas videoCanvas, IEventService eventService) : base(videoCanvas) {
            eventService.Subscribe<EntityDisplayInfo>(UpdateRectSelection);
        }

        private void UpdateRectSelection(EntityDisplayInfo info) {
            OnEntitySelected.Execute(info);
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