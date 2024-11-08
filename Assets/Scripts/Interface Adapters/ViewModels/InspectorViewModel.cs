namespace SimpleMotions {

    public interface IInspectorViewModel : IEntityViewModel {
        ReactiveCommand<EntityDisplayInfo> OnEntitySelected { get; }
    }

    public class InspectorViewModel : EntityViewModel, IInspectorViewModel {

        public ReactiveCommand<EntityDisplayInfo> OnEntitySelected { get; } = new();

        public InspectorViewModel(IVideoCanvas videoCanvas, IEventService eventService) : base(videoCanvas) {
            eventService.Subscribe<EntityDisplayInfo>(UpdateSelectedEntityInspector);
        }

        private void UpdateSelectedEntityInspector(EntityDisplayInfo info) {
            OnEntitySelected.Execute(info);
        }

    }
}