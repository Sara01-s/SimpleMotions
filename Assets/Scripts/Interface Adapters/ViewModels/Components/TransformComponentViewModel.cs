using static StringExtensions;
using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface ITransformComponentViewModel {

		ReactiveCommand<TransformDTO> OnSaveTransformKeyframe { get; }
		ReactiveCommand<TransformDTO> OnUpdateTransformKeyframe { get; }

		ReactiveCommand OnDeleteTransformKeyframe { get; }

		ReactiveCommand OnFirstKeyframe { get; }
		ReactiveCommand OnDrawTransfromKeyframe { get; }
		ReactiveCommand<bool> OnFrameHasTransformKeyframe { get; }

		ReactiveValue<(string x, string y)> PositionX { get; }
		ReactiveValue<(string y, string x)> PositionY { get; }
		ReactiveValue<(string w, string h)> ScaleW { get; }
		ReactiveValue<(string h, string w)> ScaleH { get; }
		ReactiveValue<string> Roll { get; }
		ReactiveValue<int> EaseDropdown { get; }

		ReactiveCommand<int> OnEntityCreated { get; }

	}

    public class TransformComponentViewModel : InspectorComponentViewModel, ITransformComponentViewModel {

		public ReactiveCommand<TransformDTO> OnSaveTransformKeyframe { get; } = new();
		public ReactiveCommand<TransformDTO> OnUpdateTransformKeyframe { get; } = new();

        public ReactiveCommand OnDeleteTransformKeyframe { get; } = new();

		public ReactiveCommand OnFirstKeyframe { get; } = new();
		public ReactiveCommand OnDrawTransfromKeyframe { get; } = new();
		public ReactiveCommand<bool> OnFrameHasTransformKeyframe { get; } = new();

		public ReactiveValue<(string x, string y)> PositionX { get; } = new() { Value = ("0", "0") };
		public ReactiveValue<(string y, string x)> PositionY { get; } = new() { Value = ("0", "0") };
		public ReactiveValue<(string w, string h)> ScaleW { get; } = new() { Value = ("1", "1") };
		public ReactiveValue<(string h, string w)> ScaleH { get; } = new() { Value = ("1", "1") };
		public ReactiveValue<string> Roll { get; } = new() { Value = "0" };
		public ReactiveValue<int> EaseDropdown { get; } = new();

		public ReactiveCommand<int> OnEntityCreated { get; } = new();

		private readonly IKeyframeStorage _keyframeStorage;

		public TransformComponentViewModel(IComponentStorage componentStorage, IEntitySelectorViewModel entitySelectorViewModel, 
										   IVideoPlayerData videoPlayerData, IKeyframeStorage keyframeStorage, IVideoEntities videoEntities,
										   IVideoCanvas videoCanvas) : base(entitySelectorViewModel, componentStorage, videoPlayerData, videoCanvas)
		{
			PositionX.Subscribe(position => SetEntityComponentProperty<Transform, Position> (_SelectedEntityId, t => t.Position, new Position(ParseFloat(position.x), ParseFloat(position.y))));
			PositionY.Subscribe(position => SetEntityComponentProperty<Transform, Position> (_SelectedEntityId, t => t.Position, new Position(ParseFloat(position.x), ParseFloat(position.y))));
			ScaleW.Subscribe(scale=> SetEntityComponentProperty<Transform, Scale> (_SelectedEntityId, t => t.Scale, new Scale(ParseFloat(scale.w), ParseFloat(scale.h))));
			ScaleH.Subscribe(scale => SetEntityComponentProperty<Transform, Scale> (_SelectedEntityId, t => t.Scale, new Scale(ParseFloat(scale.w), ParseFloat(scale.h))));
			Roll.Subscribe(degrees => SetEntityComponentProperty<Transform, Roll>(_SelectedEntityId, t => t.Roll, new Roll(ParseFloat(degrees))));

			OnSaveTransformKeyframe.Subscribe(transformDTO => {
				SaveKeyframe(ParseTransformDTO(transformDTO));
				OnDrawTransfromKeyframe.Execute();
			});
			
			OnDeleteTransformKeyframe.Subscribe(() => {
				var previousTransform = GetPreviousTransformKeyframe(keyframeStorage);
				var position = new Position(previousTransform.Position);
				var scale = new Scale(previousTransform.Scale);
				var rollDegrees = new Roll(previousTransform.RollDegrees);

				SetEntityComponentProperty<Transform, Position>(_SelectedEntityId, t => t.Position, position);
				SetEntityComponentProperty<Transform, Scale>(_SelectedEntityId, t => t.Scale, scale);
				SetEntityComponentProperty<Transform, Roll>(_SelectedEntityId, t => t.Roll, rollDegrees);
				
				videoCanvas.DisplayEntity(_SelectedEntityId);
			});

			OnUpdateTransformKeyframe.Subscribe(transformDTO => {
				UpdateKeyframe(ParseTransformDTO(transformDTO));
			});

			videoEntities.OnCreateEntity.Subscribe(() => {
				OnEntityCreated.Execute(_CurrentFrame);
			});

			_keyframeStorage = keyframeStorage;
		}

		private void SaveKeyframe(Transform transform) {
			var transformKeyframe = new Keyframe<Transform>(_SelectedEntityId, _CurrentFrame, transform);
			_keyframeStorage.AddKeyframe(transformKeyframe);
		}

		private void UpdateKeyframe(Transform transform) {
			var keyframe = _keyframeStorage.GetEntityKeyframeOfType<Transform>(_SelectedEntityId, _CurrentFrame);
			_keyframeStorage.SetKeyframeValue(keyframe.EntityId, keyframe.Frame, transform, keyframe.Ease);
		}

		private Transform ParseTransformDTO(TransformDTO transformDTO) {
			return new Transform (
				new Position(transformDTO.Position.x, transformDTO.Position.y),
				new Scale(transformDTO.Scale.w, transformDTO.Scale.h),
				new Roll(transformDTO.RollDegrees)
			);
		}

		private TransformDTO GetPreviousTransformKeyframe(IKeyframeStorage keyframeStorage) {
			for (int frame = _CurrentFrame - 1; frame >= TimelineData.FIRST_FRAME; frame--) {
				if (!keyframeStorage.EntityHasKeyframeAtFrameOfType<Transform>(_SelectedEntityId, frame)) {
					continue;
				}

				var previousTransformKeyframe = keyframeStorage.GetEntityKeyframeOfType<Transform>(_SelectedEntityId, frame);
				if (previousTransformKeyframe is null) {
					continue;
				}

				// Previous keyframe found!
				var transfrom = previousTransformKeyframe.Value;
				var position = (transfrom.Position.X, transfrom.Position.Y);
				var scale = (transfrom.Scale.Width, transfrom.Scale.Height);
				float rollDegrees = transfrom.Roll.AngleDegrees;

				return new TransformDTO(position, scale, rollDegrees); 
			}

			return new TransformDTO();
		}

    }
}