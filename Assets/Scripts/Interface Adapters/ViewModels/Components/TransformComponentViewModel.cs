using SimpleMotions.Internal;
using static StringExtensions;

namespace SimpleMotions {

	public interface ITransformComponentViewModel {

		ReactiveCommand<((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees)> SaveTransformKeyframe { get; }
		ReactiveCommand<bool> OnFrameHasTransformKeyframe { get; }
		ReactiveCommand OnDrawTransfromKeyframe { get; }
		ReactiveCommand OnTransformKeyframeDeleted { get; }
		ReactiveCommand<string> PositionX { get; }
		ReactiveCommand<string> PositionY { get; }
		ReactiveCommand<string> ScaleW { get; }
		ReactiveCommand<string> ScaleH { get; }
		ReactiveCommand<string> Roll { get; }

	}

	// TODO - Ojo sara
    public class TransformComponentViewModel : InspectorComponentViewModel, ITransformComponentViewModel {

		public ReactiveCommand<((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees)> SaveTransformKeyframe { get; } = new();
		public ReactiveCommand<bool> OnFrameHasTransformKeyframe { get; } = new();
		public ReactiveCommand OnDrawTransfromKeyframe { get; } = new();
        public ReactiveCommand OnTransformKeyframeDeleted { get; } = new();
		public ReactiveCommand<string> PositionX { get; } = new();
		public ReactiveCommand<string> PositionY { get; } = new();
		public ReactiveCommand<string> ScaleW { get; } = new();
		public ReactiveCommand<string> ScaleH { get; } = new();
		public ReactiveCommand<string> Roll { get; } = new();

		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IVideoCanvas _videoCanvas;

		public TransformComponentViewModel(IComponentStorage componentStorage, IEntitySelectorViewModel entitySelectorViewModel, 
										   IVideoPlayerData videoPlayerData, IKeyframeStorage keyframeStorage, 
										   IVideoCanvas videoCanvas) : 
										   base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas)
		{
			SaveTransformKeyframe.Subscribe(transformView => SaveKeyframe(ParseTransformView(transformView)));
			PositionX.Subscribe(ModifyEntityPositionX);
			PositionY.Subscribe(ModifyEntityPositionY);
			ScaleW.Subscribe(ModifyEntityScaleWidth);
			ScaleH.Subscribe(ModifyEntityScaleHeight);
			Roll.Subscribe(ModifyEntityRollAngleDegrees);

			OnTransformKeyframeDeleted.Subscribe(() => {
				keyframeStorage.RemoveKeyframe<Transform>(GetSelectedEntityId(), GetCurrentFrame());
				// TODO - Hacer que se actualice su posición en pantalla.
			});

			_keyframeStorage = keyframeStorage;
			_videoCanvas = videoCanvas;
		}

		private void UpdateSelectedEntityDisplay() {
			_videoCanvas.DisplayEntity(GetSelectedEntityId());
		}

		private void SaveKeyframe(Transform transform) {
			var transformKeyframe = new Keyframe<Transform>(GetSelectedEntityId(), GetCurrentFrame(), transform);
			_keyframeStorage.AddKeyframe(transformKeyframe);
			UnityEngine.Debug.Log($"Keyframe de Transform guardado: {transformKeyframe}");
		}

		private Transform ParseTransformView(((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees) transformView) {
			return new Transform (
				position: new Position(ParseFloat(transformView.pos.x), ParseFloat(transformView.pos.y)),
				scale: new Scale(ParseFloat(transformView.scale.w), ParseFloat(transformView.scale.h)),
				roll: new Roll(ParseFloat(transformView.rollAngleDegrees))
			);
		}

		private void ModifyEntityPositionX(string positionX) {
			GetSelectedEntityComponent<Transform>().Position.X = ParseFloat(positionX);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityPositionY(string positionY) {
			GetSelectedEntityComponent<Transform>().Position.Y = ParseFloat(positionY);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleWidth(string scaleWidth) {
			GetSelectedEntityComponent<Transform>().Scale.Width = ParseFloat(scaleWidth);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleHeight(string scaleHeight) {
			GetSelectedEntityComponent<Transform>().Scale.Height = ParseFloat(scaleHeight);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityRollAngleDegrees(string angleDegrees) {
			GetSelectedEntityComponent<Transform>().Roll.AngleDegrees = ParseFloat(angleDegrees);
			UpdateSelectedEntityDisplay();
		}

    }
}