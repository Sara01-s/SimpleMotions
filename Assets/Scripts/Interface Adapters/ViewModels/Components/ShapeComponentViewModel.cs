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
                                       IVideoCanvas videoCanvas, IEntitySelector entitySelector) 
									   : base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas) 
        {
            OnSaveShapeKeyframe.Subscribe(shapeDTO => {
				UnityEngine.Debug.Log("SOY LA VIEW ESTOY GUARDANDO ESTE SHAPE: " + shapeDTO.PrimitiveShape);
                SaveKeyframe(ParseShapeDTOColor(shapeDTO), shapeDTO.PrimitiveShape);
                OnDrawShapeKeyframe.Execute();
            });

			OnDeleteShapeKeyframe.Subscribe(() => {
				var previousShapeDTO = GetPreviousShapeKeyframeDTO();

				SetShape(previousShapeDTO.PrimitiveShape);
                SetColor(ParseShapeDTOColor(previousShapeDTO));

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
			var primitiveShape = (Shape.Primitive)Enum.Parse(typeof(Shape.Primitive), shapeName);
			UnityEngine.Debug.Log("SOY EL VIEWMODEL, RESULTADO DEL PARSE: " + primitiveShape);

            var shape = new Shape(primitiveShape, color);
            var shapeKeyframe = new Keyframe<Shape>(_SelectedEntityId, _CurrentFrame, shape);

            _keyframeStorage.AddKeyframe(shapeKeyframe);
        }

         public void UpdateKeyframe(Color color, string shapeName) {
			var primitiveShape = (Shape.Primitive)Enum.Parse(typeof(Shape.Primitive), shapeName);
            var keyframe = _keyframeStorage.GetEntityKeyframeOfType<Shape>(_SelectedEntityId, _CurrentFrame);
            var shape = new Shape(primitiveShape, color);

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

		private ShapeDTO GetPreviousShapeKeyframeDTO() {
			var entityShapeSpline = _keyframeStorage.GetEntityKeyframeSplineOfType<Shape>(_SelectedEntityId);
			var previousShapeKeyframe = entityShapeSpline.GetPreviousKeyframe(_CurrentFrame);
			var shapeColor = previousShapeKeyframe.Value.Color;
			var colorDTO = new ColorDTO(shapeColor.R, shapeColor.G, shapeColor.B, shapeColor.A);
			return new ShapeDTO(previousShapeKeyframe.Value.PrimitiveShape.ToString(), colorDTO);
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
