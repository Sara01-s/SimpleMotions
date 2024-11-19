using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IShapeComponentViewModel {
        void SetColor(Color color);
		void SetShape(string shapeName);
    }

    public class ShapeComponentViewModel : ComponentViewModel, IShapeComponentViewModel {

        private readonly IEntitySelector _entitySelector;

        public ShapeComponentViewModel(IEntitySelector entitySelector, IVideoCanvas videoCanvas) : base(videoCanvas) {
            _entitySelector = entitySelector;
        }

		public void SetShape(string shapeName) {
			if (_entitySelector.TryGetSelectedEntityId(out int selectedEntity)) {
				SetEntityShape(selectedEntity, shapeName);
			}
		}

        public void SetColor(Color color) {
			if (_entitySelector.TryGetSelectedEntityId(out int selectedEntity)) {
            	SetEntityColor(selectedEntity, color);
			}
        }

    }
}
