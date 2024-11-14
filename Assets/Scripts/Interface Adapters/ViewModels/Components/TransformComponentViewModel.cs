using SimpleMotions.Internal;
using System;
using Unity.VisualScripting;

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

		private readonly IEntitySelector _entitySelector;
		private readonly IVideoPlayerData _videoPlayerData;
		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IVideoCanvas _videoCanvas;
		private readonly IComponentStorage _componentStorage;

		public TransformComponentViewModel(IComponentStorage componentStorage, IEntitySelector entitySelector, IVideoCanvas videoCanvas, 
										   IKeyframeStorage keyframeStorage, IVideoPlayerData videoPlayerData)
		{
			SaveTransformKeyframe.Subscribe(transformView => SaveKeyframe(ParseTransformView(transformView)));
			PositionX.Subscribe(ModifyEntityPositionX);
			PositionY.Subscribe(ModifyEntityPositionY);
			ScaleW.Subscribe(ModifyEntityScaleWidth);
			ScaleH.Subscribe(ModifyEntityScaleHeight);
			Roll.Subscribe(ModifyEntityRollAngleDegrees);

			_entitySelector = entitySelector;
			_videoPlayerData = videoPlayerData;
			_keyframeStorage = keyframeStorage;
			_videoCanvas = videoCanvas;
			_componentStorage = componentStorage;
		}

		private Transform GetSelectedEntityTransform() {
			return _componentStorage.GetComponent<Transform>(GetSelectedEntityId());
		}

		private void UpdateSelectedEntityDisplay() {
			_videoCanvas.DisplayEntity(GetSelectedEntityId());
		}

		private void SaveKeyframe(Transform transform) {
			var keyframe = new Keyframe<Transform>(GetSelectedEntityId(), GetCurrentFrame(), transform);
			_keyframeStorage.AddKeyframe(keyframe);
			UnityEngine.Debug.Log($"Keyframe guardado: {keyframe}");
		}

		private int GetCurrentFrame() {
			UnityEngine.Debug.Log(_videoPlayerData.CurrentFrame.Value);
			return _videoPlayerData.CurrentFrame.Value;
		}

		private int GetSelectedEntityId() {
			return _entitySelector.SelectedEntity.Id;
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
				GetSelectedEntityTransform().Position.X = x;
				UpdateSelectedEntityDisplay();
			}
		}

		private void ModifyEntityPositionY(string positionY) {
			GetSelectedEntityTransform().Position.Y = float.Parse(positionY);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleWidth(string scaleWidth) {
			GetSelectedEntityTransform().Scale.Width = float.Parse(scaleWidth);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleHeight(string scaleHeight) {
			GetSelectedEntityTransform().Scale.Height = float.Parse(scaleHeight);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityRollAngleDegrees(string angleDegrees) {
			UnityEngine.Debug.Log(angleDegrees);

			GetSelectedEntityTransform().Roll.AngleDegrees = float.Parse(angleDegrees);
			UpdateSelectedEntityDisplay();
		}

    }
}