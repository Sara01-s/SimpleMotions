using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IShapeComponentViewModel {
        void SetColor(Color color);
    }

    public class ShapeComponentViewModel : ComponentViewModel, IShapeComponentViewModel {

        private readonly IEntitySelector _entitySelector;

        public ShapeComponentViewModel(IEntitySelector entitySelector, IVideoCanvas videoCanvas) : base(videoCanvas) {
            _entitySelector = entitySelector;
        }

        public void SetColor(Color color) {
            var selectedEntity = _entitySelector.SelectedEntity;

            if (selectedEntity.Id == Entity.Invalid.Id) {
                return;
            }
            
            ChangeEntityColor(selectedEntity.Id, color);
        }

    }
}
