using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

    public interface IShapeComponentViewModel {

        ReactiveCommand<(string shapeName, 
                         float r, float g, float b, float a)> OnSaveShapeKeyframe { get; }

        ReactiveCommand<(string shapeName, 
                         float r, float g, float b, float a)> OnUpdateShapeKeyframe { get; }

        ReactiveCommand OnDeleteShapeKeyframe { get; }

        ReactiveCommand OnFirstKeyframe { get; }
        ReactiveCommand OnDrawShapeKeyframe { get; }
        ReactiveCommand<bool> OnFrameHasShapeKeyframe { get; }

		ReactiveCommand<string> OnImageSelected { get; }

        void SetColor(Color color);
		void SetShape(string shapeName);
    }

    public class ShapeComponentViewModel : InspectorComponentViewModel, IShapeComponentViewModel {

        public ReactiveCommand<(string shapeName, 
                                float r, float g, float b, float a)> OnSaveShapeKeyframe { get; } = new();

        public ReactiveCommand<(string shapeName, 
                                float r, float g, float b, float a)> OnUpdateShapeKeyframe { get; } = new();

        public ReactiveCommand OnDeleteShapeKeyframe { get; } = new();

        public ReactiveCommand OnFirstKeyframe { get; } = new();
        public ReactiveCommand OnDrawShapeKeyframe { get; } = new();
        public ReactiveCommand<bool> OnFrameHasShapeKeyframe { get; } = new();

		public ReactiveCommand<string> OnImageSelected { get; } = new();

        private readonly IEntitySelector _entitySelector;
        private readonly IKeyframeStorage _keyframeStorage;
        private readonly IVideoCanvas _videoCanvas;

        public ShapeComponentViewModel(IEntitySelectorViewModel entitySelectorViewModel, IComponentStorage componentStorage, 
                                       IVideoPlayerData videoPlayerData, IKeyframeStorage keyframeStorage, 
                                       IVideoCanvas videoCanvas, IEntitySelector entitySelector) : 
                                       base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas) 
        {
            OnSaveShapeKeyframe.Subscribe(shapeView => { 
                SaveKeyframe(ParseShapeColorView(shapeView), shapeView.shapeName);
                OnDrawShapeKeyframe.Execute();
            });

			OnDeleteShapeKeyframe.Subscribe(() => {
				var (type, color) = GetPreviousShapeKeyframeName(keyframeStorage);

				SetShape(type);
                SetColor(color);

				keyframeStorage.RemoveKeyframeOfType(typeof(Shape), _SelectedEntityId, _CurrentFrame);
				videoCanvas.DisplayEntity(_SelectedEntityId);
			});

            OnUpdateShapeKeyframe.Subscribe(shapeView => {
				SaveKeyframe(ParseShapeColorView(shapeView), shapeView.shapeName);
			});

			OnImageSelected.Subscribe(imageFilePath => {
				if (string.IsNullOrEmpty(imageFilePath)) {
					return;
				}

				videoCanvas.OnSetEntityImage.Execute(_SelectedEntityId, imageFilePath);
			});

            _keyframeStorage = keyframeStorage;
            _entitySelector = entitySelector;
            _videoCanvas = videoCanvas;
        }

        public void SaveKeyframe(Color color, string shapeName) {
            var shape = new Shape((Shape.Primitive)Enum.Parse(typeof(Shape.Primitive), shapeName), color);
            var shapeKeyframe = new Keyframe<Shape>(_SelectedEntityId, _CurrentFrame, shape);

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

        private void UpdateSelectedEntityDisplay() {
			_videoCanvas.DisplayEntity(_SelectedEntityId);
		}

		private (string type, Color color) GetPreviousShapeKeyframeName(IKeyframeStorage keyframeStorage) {
			for (int frame = _CurrentFrame - 1; frame >= TimelineData.FIRST_FRAME; frame--) {
				if (keyframeStorage.EntityHasKeyframeAtFrameOfType<Shape>(_SelectedEntityId, frame)) {
					var previousShape = keyframeStorage.GetEntityKeyframeOfType<Shape>(_SelectedEntityId, frame);

					if (previousShape is not null) {
						return (previousShape.Value.PrimitiveShape.ToString(), previousShape.Value.Color);
					}
				}
			}

			return (string.Empty, Color.White);
		}

		public void SetShape(string shapeName) {
			if (_entitySelector.TryGetSelectedEntityId(out int selectedEntity)) {
				SetEntityShape(selectedEntity, shapeName);
                UpdateSelectedEntityDisplay();
			}
		}

        public void SetColor(Color color) {
			if (_entitySelector.TryGetSelectedEntityId(out int selectedEntity)) {
            	SetEntityColor(selectedEntity, color);
                UpdateSelectedEntityDisplay();
			}
        }

    }
}
