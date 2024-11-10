
namespace SimpleMotions {

    public interface IInspectorViewModel : IEntityViewModel {

        ReactiveCommand<(int id, string name)> OnEntitySelected { get; }

    }

    public class InspectorViewModel : EntityViewModel, IInspectorViewModel {

        public ReactiveCommand<(int id, string name)> OnEntitySelected { get; } = new();

        public InspectorViewModel(IVideoCanvas videoCanvas, IVideoAnimator videoAnimator) : base(videoCanvas) {
            videoCanvas.EntityDisplayInfo.Subscribe(UpdateEntityId);
            videoAnimator.EntityDisplayInfo.Subscribe(UpdateEntityId);
        }

        private void UpdateEntityId((int id, string name) entity) {
            UnityEngine.Debug.Log(entity);
            OnEntitySelected.Execute(entity);
        }

    }
}