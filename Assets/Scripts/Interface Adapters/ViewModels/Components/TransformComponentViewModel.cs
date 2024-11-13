using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

	public interface ITransformComponentViewModel {

		ReactiveCommand<((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees)> SaveTransformKeyframe { get; }
		ReactiveCommand<string> PositionX { get; }
		ReactiveCommand<string> PositionY { get; }
		ReactiveCommand<string> ScaleW { get; }
		ReactiveCommand<string> ScaleH { get; }
		ReactiveCommand<string> Roll { get; }

	}

    public class TransformComponentViewModel : ITransformComponentViewModel {

		public ReactiveCommand<((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees)> SaveTransformKeyframe { get; } = new();
		public ReactiveCommand<string> PositionX { get; } = new();
		public ReactiveCommand<string> PositionY { get; } = new();
		public ReactiveCommand<string> ScaleW { get; } = new();
		public ReactiveCommand<string> ScaleH { get; } = new();
		public ReactiveCommand<string> Roll { get; } = new();

		private readonly Func<Transform> _getSelectedEntityTransform;
		private readonly Action _updateSelectedEntityDisplay;

		public TransformComponentViewModel(IComponentStorage componentStorage, IEntitySelector entitySelector, IVideoCanvas videoCanvas, 
										   IKeyframeStorage keyframeStorage, IVideoPlayerData videoPlayerData)
		{
			_getSelectedEntityTransform = () => {
				int selectedEntityId = entitySelector.SelectedEntity.Id;
				return componentStorage.GetComponent<Transform>(selectedEntityId);
			};

			_updateSelectedEntityDisplay = () => {
				videoCanvas.DisplayEntity(entitySelector.SelectedEntity.Id);
			};

			void saveTransformKeyframe(Transform transform) {
				int selectedEntityId = entitySelector.SelectedEntity.Id;
				var currentFrame = videoPlayerData.CurrentFrame.Value;
				var keyframe = new Keyframe<Transform>(selectedEntityId, currentFrame, transform);

				keyframeStorage.AddKeyframe(selectedEntityId, keyframe);

				UnityEngine.Debug.Log($"Keyframe guardado: {keyframe}");
			}

			SaveTransformKeyframe.Subscribe(transformView => saveTransformKeyframe(ParseTransformView(transformView)));
			PositionX.Subscribe(ModifyEntityPositionX);
			PositionY.Subscribe(ModifyEntityPositionY);
			ScaleW.Subscribe(ModifyEntityScaleWidth);
			ScaleH.Subscribe(ModifyEntityScaleHeight);
			Roll.Subscribe(ModifyEntityRollAngleDegrees);
		}

		private Transform ParseTransformView(((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees) transformView) {
			return new Transform (
				position: new Position(float.Parse(transformView.pos.x), float.Parse(transformView.pos.y)),
				scale: new Scale(float.Parse(transformView.scale.w), float.Parse(transformView.scale.h)),
				roll: new Roll(float.Parse(transformView.rollAngleDegrees))
			);
		}

		private void ModifyEntityPositionX(string positionX) {
			if (float.TryParse(positionX, out float x)) {
				_getSelectedEntityTransform().Position.X = x;
				_updateSelectedEntityDisplay();
			}
		}

		private void ModifyEntityPositionY(string positionY) {
			_getSelectedEntityTransform().Position.Y = float.Parse(positionY);
			_updateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleWidth(string scaleWidth) {
			_getSelectedEntityTransform().Scale.Width = float.Parse(scaleWidth);
			_updateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleHeight(string scaleHeight) {
			_getSelectedEntityTransform().Scale.Height = float.Parse(scaleHeight);
			_updateSelectedEntityDisplay();
		}

		private void ModifyEntityRollAngleDegrees(string angleDegrees) {
			_getSelectedEntityTransform().Roll.AngleDegrees = float.Parse(angleDegrees);
			_updateSelectedEntityDisplay();
		}

    }
}