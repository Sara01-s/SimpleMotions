using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

    public interface IShapeComponentViewModel {

        ReactiveCommand<ShapeDTO> OnSaveShapeKeyframe { get; }
        ReactiveCommand<ShapeDTO> OnUpdateShapeKeyframe { get; }

        ReactiveCommand OnDeleteShapeKeyframe { get; }

        ReactiveCommand OnFirstKeyframe { get; }
        ReactiveCommand OnDrawShapeKeyframe { get; }
        ReactiveCommand<bool> OnFrameHasShapeKeyframe { get; }

		ReactiveCommand<string> OnImageSelected { get; }

        ReactiveCommand<Color> OnSelectedEntity { get; }

        void SetColor(Color color);
		void SetShape(string shapeName);
    }

    public class ShapeComponentViewModel : InspectorComponentViewModel, IShapeComponentViewModel {

        public ReactiveCommand<ShapeDTO> OnSaveShapeKeyframe { get; } = new();
        public ReactiveCommand<ShapeDTO> OnUpdateShapeKeyframe { get; } = new();

        public ReactiveCommand OnDeleteShapeKeyframe { get; } = new();

        public ReactiveCommand OnFirstKeyframe { get; } = new();
        public ReactiveCommand OnDrawShapeKeyframe { get; } = new();
        public ReactiveCommand<bool> OnFrameHasShapeKeyframe { get; } = new();

		public ReactiveCommand<string> OnImageSelected { get; } = new();

        public ReactiveCommand<Color> OnSelectedEntity { get; } = new();

        private readonly IEntitySelector _entitySelector;
        private readonly IKeyframeStorage _keyframeStorage;
        private readonly IVideoCanvas _videoCanvas;

        public ShapeComponentViewModel(IEntitySelectorViewModel entitySelectorViewModel, IComponentStorage componentStorage, 
                                       IVideoPlayerData videoPlayerData, IKeyframeStorage keyframeStorage, 
                                       IVideoCanvas videoCanvas, IEntitySelector entitySelector) : 
                                       base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas) 
        {
            OnSaveShapeKeyframe.Subscribe(shapeDTO => { 
                SaveKeyframe(ParseShapeDTOColor(shapeDTO), shapeDTO.PrimitiveShape);
                OnDrawShapeKeyframe.Execute();
            });

			OnDeleteShapeKeyframe.Subscribe(() => {
				var (type, color) = GetPreviousShapeKeyframeName(keyframeStorage);

				SetShape(type);
                SetColor(color);

				videoCanvas.DisplayEntity(_SelectedEntityId);
			});

            OnUpdateShapeKeyframe.Subscribe(shapeView => {
				UpdateKeyframe(ParseShapeDTOColor(shapeView), shapeView.PrimitiveShape);
			});

			OnImageSelected.Subscribe(imageFilePath => {
				if (string.IsNullOrEmpty(imageFilePath)) {
					return;
				}

				videoCanvas.OnSetEntityImage.Execute(_SelectedEntityId, imageFilePath);
			});

            entitySelectorViewModel.OnEntitySelected.Subscribe(entity => {
                var shapeComponent = componentStorage.GetComponent<Shape>(_SelectedEntityId);
                OnSelectedEntity.Execute(shapeComponent.Color);
            });

            _keyframeStorage = keyframeStorage;
            _entitySelector = entitySelector;
            _videoCanvas = videoCanvas;
        }

        public void SaveKeyframe(Color color, string shapeName) {
            var shape = new Shape((Shape.Primitive)Enum.Parse(typeof(Shape.Primitive), shapeName), color);
            var shapeKeyframe = new Keyframe<Shape>(_SelectedEntityId, _CurrentFrame, shape);

            _keyframeStorage.AddKeyframe(shapeKeyframe);
        }

         public void UpdateKeyframe(Color color, string shapeName) {
            var shape = new Shape((Shape.Primitive)Enum.Parse(typeof(Shape.Primitive), shapeName), color);
            var keyframe = _keyframeStorage.GetEntityKeyframeOfType<Shape>(_SelectedEntityId, _CurrentFrame);

			_keyframeStorage.SetKeyframeValue(keyframe.EntityId, keyframe.Frame, shape, keyframe.Ease);
        }

        public Color ParseShapeDTOColor(ShapeDTO shapeDTO) {
            return new Color() {
                R = shapeDTO.Color.R,
                G = shapeDTO.Color.G,
                B = shapeDTO.Color.B,
                A = shapeDTO.Color.A
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
