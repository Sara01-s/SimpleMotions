using static StringExtensions;
using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

	public interface ITransformComponentViewModel {

		ReactiveCommand<((string x, string y) pos, 
						 (string w, string h) scale, 
						 string rollAngleDegrees)> OnSaveTransformKeyframe { get; }

		ReactiveCommand<((string x, string y) pos, 
						 (string w, string h) scale, 
						 string rollAngleDegrees)> OnUpdateTransformKeyframe { get; }

		ReactiveCommand OnDeleteTransformKeyframe { get; }

		ReactiveCommand OnFirstKeyframe { get; }
		ReactiveCommand OnDrawTransfromKeyframe { get; }
		ReactiveCommand<bool> OnFrameHasTransformKeyframe { get; }

		ReactiveCommand<string> PositionX { get; }
		ReactiveCommand<string> PositionY { get; }
		ReactiveCommand<string> ScaleW { get; }
		ReactiveCommand<string> ScaleH { get; }
		ReactiveCommand<string> Roll { get; }
		ReactiveCommand<int> EaseDropdown { get; }

	}

	// TODO - Ojo sara
    public class TransformComponentViewModel : InspectorComponentViewModel, ITransformComponentViewModel {

		public ReactiveCommand<((string x, string y) pos, 
								(string w, string h) scale, 
								string rollAngleDegrees)> OnSaveTransformKeyframe { get; } = new();

		public ReactiveCommand<((string x, string y) pos, 
								(string w, string h) scale, 
								string rollAngleDegrees)> OnUpdateTransformKeyframe { get; } = new();

        public ReactiveCommand OnDeleteTransformKeyframe { get; } = new();

		public ReactiveCommand OnFirstKeyframe { get; } = new();
		public ReactiveCommand OnDrawTransfromKeyframe { get; } = new();
		public ReactiveCommand<bool> OnFrameHasTransformKeyframe { get; } = new();

		public ReactiveCommand<string> PositionX { get; } = new();
		public ReactiveCommand<string> PositionY { get; } = new();
		public ReactiveCommand<string> ScaleW { get; } = new();
		public ReactiveCommand<string> ScaleH { get; } = new();
		public ReactiveCommand<string> Roll { get; } = new();
		public ReactiveCommand<int> EaseDropdown { get; } = new();

		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IVideoCanvas _videoCanvas;

		public TransformComponentViewModel(IComponentStorage componentStorage, IEntitySelectorViewModel entitySelectorViewModel, 
										   IVideoPlayerData videoPlayerData, IKeyframeStorage keyframeStorage, 
										   IVideoCanvas videoCanvas) : 
										   base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas)
		{
			PositionX.Subscribe(positionX => ModifyEntityProperty(positionX, 
																  transform => transform.Position,
																  (position, parsedValue) => position.X = parsedValue));

			PositionY.Subscribe(positionY => ModifyEntityProperty(positionY, 
																  transform => transform.Position,
																  (position, parsedValue) => position.Y = parsedValue));

			ScaleW.Subscribe(scaleW => ModifyEntityProperty(scaleW, 
															transform => transform.Scale,
															(scale, parsedValue) => scale.Width = parsedValue));

			ScaleH.Subscribe(scaleH => ModifyEntityProperty(scaleH, 
															transform => transform.Scale,
															(scale, parsedValue) => scale.Height = parsedValue));

			Roll.Subscribe(rollAnglesDegrees => ModifyEntityProperty(rollAnglesDegrees, 
																	 transform => transform.Roll,
																	 (roll, parsedValue) => roll.AngleDegrees = parsedValue));

			OnSaveTransformKeyframe.Subscribe(transformView => {
				SaveKeyframe(ParseTransformView(transformView));
				OnDrawTransfromKeyframe.Execute();
			});
			
			OnDeleteTransformKeyframe.Subscribe(() => {
				var previousTransform = GetPreviousTransformKeyframe(keyframeStorage);

				ModifyEntityProperty(previousTransform.posX.ToString(), 
									 transform => transform.Position,
									 (position, parsedValue) => position.X = parsedValue);

				ModifyEntityProperty(previousTransform.posY.ToString(), 
									 transform => transform.Position,
									 (position, parsedValue) => position.Y = parsedValue);

				ModifyEntityProperty(previousTransform.scaleW.ToString(), 
									 transform => transform.Scale,
									 (scale, parsedValue) => scale.Width = parsedValue);

				ModifyEntityProperty(previousTransform.scaleH.ToString(),
									 transform => transform.Scale,
									 (scale, parsedValue) => scale.Height = parsedValue);

				ModifyEntityProperty(previousTransform.roll.ToString(),
									 transform => transform.Roll,
									 (roll, parsedValue) => roll.AngleDegrees = parsedValue);

				keyframeStorage.RemoveKeyframeOfType(typeof(Transform), _SelectedEntityId, _CurrentFrame);
				videoCanvas.DisplayEntity(_SelectedEntityId);
			});

			OnUpdateTransformKeyframe.Subscribe(transformView => {
				SaveKeyframe(ParseTransformView(transformView));
			});

			_keyframeStorage = keyframeStorage;
			_videoCanvas = videoCanvas;
		}

		private void SaveKeyframe(Transform transform) {
			UnityEngine.Debug.Log(transform);
			var transformKeyframe = new Keyframe<Transform>(_SelectedEntityId, _CurrentFrame, transform);
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

		private void UpdateSelectedEntityDisplay() {
			_videoCanvas.DisplayEntity(_SelectedEntityId);
		}

		private (float posX, float posY, float scaleW, float scaleH, float roll) GetPreviousTransformKeyframe(IKeyframeStorage keyframeStorage) {
			for (int frame = _CurrentFrame - 1; frame >= TimelineData.FIRST_FRAME; frame--) {
				if (keyframeStorage.EntityHasKeyframeAtFrameOfType<Transform>(_SelectedEntityId, frame)) {
					var previousTransform = keyframeStorage.GetEntityKeyframeOfType<Transform>(_SelectedEntityId, frame);

					if (previousTransform is not null) {
						return (previousTransform.Value.Position.X, previousTransform.Value.Position.Y,
								previousTransform.Value.Scale.Width, previousTransform.Value.Scale.Height,
								previousTransform.Value.Roll.AngleDegrees);
					}
				}
			}

			return (0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
		}

		private void ModifyEntityProperty<T>(string value, Func<Transform, T> getter, Action<T, float> setter) {
    		Transform transform = GetSelectedEntityComponent<Transform>();
    		T property = getter(transform);
    		setter(property, ParseFloat(value));
    		UpdateSelectedEntityDisplay();
		}

    }
}