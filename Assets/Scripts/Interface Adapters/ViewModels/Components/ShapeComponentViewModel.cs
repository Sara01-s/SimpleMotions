using SimpleMotions.Internal;
using System;

#nullable enable

namespace SimpleMotions {

    public interface IShapeComponentViewModel {
        ReactiveCommand<(string shapeName, float r, float g, float b, float a)> SaveShapeKeyframe { get; }
        ReactiveCommand<bool> OnFrameHasKeyframe { get; }
        ReactiveCommand OnDrawShapeKeyframe { get; }
        ReactiveCommand OnShapeKeyframeDeleted { get; }

        void SetColor(Color color);
		void SetShape(string shapeName);
    }

    public class ShapeComponentViewModel : InspectorComponentViewModel, IShapeComponentViewModel {

        public ReactiveCommand<(string shapeName, float r, float g, float b, float a)> SaveShapeKeyframe { get; } = new();
        public ReactiveCommand<bool> OnFrameHasKeyframe { get; } = new();
        public ReactiveCommand OnDrawShapeKeyframe { get; } = new();
        public ReactiveCommand OnShapeKeyframeDeleted { get; } = new();

        private readonly IEntitySelector _entitySelector;
        private readonly IKeyframeStorage _keyframeStorage;

        public ShapeComponentViewModel(IEntitySelectorViewModel entitySelectorViewModel, IComponentStorage componentStorage, 
                                       IVideoPlayerData videoPlayerData, IKeyframeStorage keyframeStorage, 
                                       IVideoCanvas videoCanvas, IEntitySelector entitySelector) 
                                       : base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas) 
        {
            SaveShapeKeyframe.Subscribe(shapeView => SaveKeyframe(ParseShapeColorView(shapeView), shapeView.shapeName));

			OnShapeKeyframeDeleted.Subscribe(() => {
				string previousShapeName = GetPreviousKeyframeShapeName(keyframeStorage);
				SetShape(previousShapeName);

				keyframeStorage.RemoveKeyframe<Shape>(GetSelectedEntityId(), GetCurrentFrame());
				videoCanvas.DisplayEntity(GetSelectedEntityId());
			});

            _keyframeStorage = keyframeStorage;
            _entitySelector = entitySelector;
        }

		private string GetPreviousKeyframeShapeName(IKeyframeStorage keyframeStorage) {
			for (int frame = GetCurrentFrame() - 1; frame >= TimelineData.FIRST_FRAME; frame--) {
				if (keyframeStorage.FrameHasKeyframeOfTypeAt<Shape>(frame)) {
					var previousShape = keyframeStorage.GetKeyframeOfTypeAt<Shape>(frame);

					if (previousShape is not null) {
						return previousShape.Value.PrimitiveShape.ToString();
					}
				}
			}

			return string.Empty;
		}

		public void SaveKeyframe(Color color, string shapeName) {
            var shape = new Shape((Shape.Primitive)Enum.Parse(typeof(Shape.Primitive), shapeName), color);
            var shapeKeyframe = new Keyframe<Shape>(GetSelectedEntityId(), GetCurrentFrame(), shape);

            _keyframeStorage.AddKeyframe(shapeKeyframe);
            UnityEngine.Debug.Log($"Keyframe de Shape guardado: {shapeKeyframe}");
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
