using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

    public interface IShapeComponentViewModel {
        ReactiveCommand<(string shapeName, float r, float g, float b, float a)> SaveShapeKeyframe { get; }

        void SetColor(Color color);
		void SetShape(string shapeName);
    }

    public class ShapeComponentViewModel : ComponentViewModel, IShapeComponentViewModel {

        public ReactiveCommand<(string shapeName, float r, float g, float b, float a)> SaveShapeKeyframe { get; } = new();

        private readonly IEntitySelector _entitySelector;

        public ShapeComponentViewModel(IEntitySelectorViewModel entitySelectorViewModel, IComponentStorage componentStorage, IVideoPlayerData videoPlayerData, IVideoCanvas videoCanvas, IKeyframeStorage keyframeStorage) : base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas) {
            SaveShapeKeyframe.Subscribe(shapeView => SaveKeyframe(ParseShapeColorView(shapeView), shapeView.shapeName));
        }

        public void SaveKeyframe(Color color, string shapeName) {
            var shape = new Shape((Shape.Primitive)Enum.Parse(typeof(Shape.Primitive), shapeName), color);
            var keyframe = new Keyframe<Shape>(GetSelectedEntityId(), GetCurrentFrame(), shape);
        }

        public Color ParseShapeColorView((string shapeName, float r, float g, float b, float a) shapeView) {
            return new Color() {
                R = shapeView.r,
                G = shapeView.g,
                B = shapeView.b,
                A = shapeView.a
            };
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
