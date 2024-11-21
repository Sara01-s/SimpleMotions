using System.Windows.Forms;
using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface ITransformComponentViewModel {

		ReactiveCommand<((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees)> SaveTransformKeyframe { get; }
		ReactiveCommand<bool> OnFrameHasKeyframe { get; }
		ReactiveCommand OnDrawTransfromKeyframe { get; }
		ReactiveCommand OnKeyframeDeleted { get; }
		ReactiveCommand<string> PositionX { get; }
		ReactiveCommand<string> PositionY { get; }
		ReactiveCommand<string> ScaleW { get; }
		ReactiveCommand<string> ScaleH { get; }
		ReactiveCommand<string> Roll { get; }

	}

	// TODO - Ojo sara
    public class TransformComponentViewModel : ComponentViewModel, ITransformComponentViewModel {

		public ReactiveCommand<((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees)> SaveTransformKeyframe { get; } = new();
		public ReactiveCommand<bool> OnFrameHasKeyframe { get; } = new();
		public ReactiveCommand OnDrawTransfromKeyframe { get; } = new();
        public ReactiveCommand OnKeyframeDeleted { get; } = new();
		public ReactiveCommand<string> PositionX { get; } = new();
		public ReactiveCommand<string> PositionY { get; } = new();
		public ReactiveCommand<string> ScaleW { get; } = new();
		public ReactiveCommand<string> ScaleH { get; } = new();
		public ReactiveCommand<string> Roll { get; } = new();

        private readonly IEntitySelectorViewModel _entitySelectorViewModel;
		private readonly IVideoPlayerData _videoPlayerData;
		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IVideoCanvas _videoCanvas;
		private readonly IComponentStorage _componentStorage;

		public TransformComponentViewModel(IComponentStorage componentStorage, IEntitySelectorViewModel entitySelectorViewModel, IVideoCanvas videoCanvas, 
										   IKeyframeStorage keyframeStorage, IVideoPlayerData videoPlayerData) : base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas)
		{
			SaveTransformKeyframe.Subscribe(transformView => SaveKeyframe(ParseTransformView(transformView)));
			PositionX.Subscribe(ModifyEntityPositionX);
			PositionY.Subscribe(ModifyEntityPositionY);
			ScaleW.Subscribe(ModifyEntityScaleWidth);
			ScaleH.Subscribe(ModifyEntityScaleHeight);
			Roll.Subscribe(ModifyEntityRollAngleDegrees);

			OnKeyframeDeleted.Subscribe(() => {
				keyframeStorage.RemoveKeyframe<Transform>(GetSelectedEntityId(), GetCurrentFrame());
				// TODO - Hacer que se actualice su posición en pantalla.
			});

			_entitySelectorViewModel = entitySelectorViewModel;
			_videoPlayerData = videoPlayerData;
			_keyframeStorage = keyframeStorage;
			_videoCanvas = videoCanvas;
			_componentStorage = componentStorage;
		}

		private void UpdateSelectedEntityDisplay() {
			_videoCanvas.DisplayEntity(GetSelectedEntityId());
		}

		private void SaveKeyframe(Transform transform) {
			var keyframe = new Keyframe<Transform>(GetSelectedEntityId(), GetCurrentFrame(), transform);
			_keyframeStorage.AddKeyframe(keyframe);
			UnityEngine.Debug.Log($"Keyframe guardado: {keyframe}");
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
				GetSelectedSEntityComponent<Transform>().Position.X = x;
				UpdateSelectedEntityDisplay();
			}
		}

		private void ModifyEntityPositionY(string positionY) {
			GetSelectedSEntityComponent<Transform>().Position.Y = float.Parse(positionY);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleWidth(string scaleWidth) {
			GetSelectedSEntityComponent<Transform>().Scale.Width = float.Parse(scaleWidth);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityScaleHeight(string scaleHeight) {
			GetSelectedSEntityComponent<Transform>().Scale.Height = float.Parse(scaleHeight);
			UpdateSelectedEntityDisplay();
		}

		private void ModifyEntityRollAngleDegrees(string angleDegrees) {
			GetSelectedSEntityComponent<Transform>().Roll.AngleDegrees = float.Parse(angleDegrees);
			UpdateSelectedEntityDisplay();
		}

    }
}