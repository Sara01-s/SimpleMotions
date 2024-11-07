namespace SimpleMotions {

    public interface IInspectorViewModel {
        ReactiveCommand<string> OnSelectedEntityNameUpdated { get; }
    }

    public class InspectorViewModel : IInspectorViewModel {

        public ReactiveCommand<string> OnSelectedEntityNameUpdated { get; } = new();
        

        public InspectorViewModel(IEventService eventService) {
            eventService.Subscribe<EntityDisplayInfo>(UpdateSelectedEntityName);
        }

        private void UpdateSelectedEntityName(EntityDisplayInfo info) {
            OnSelectedEntityNameUpdated.Execute(info.EntityName);
        }
    }
}