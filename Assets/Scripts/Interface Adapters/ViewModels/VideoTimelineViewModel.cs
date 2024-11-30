using SimpleMotions.Internal;

namespace SimpleMotions {
	
	public interface IVideoTimelineViewModel {
		int TotalFrameCount { get; } 

		ReactiveValue<int> CurrentFrame { get; }
		ReactiveCommand<int> OnFrameChanged { get; }

		ReactiveCommand<KeyframeDTO> OnDrawTransformKeyframe { get; }
		ReactiveCommand<KeyframeDTO> OnTransfromKeyframeDeleted { get; }

		ReactiveCommand<KeyframeDTO> OnDrawShapeKeyframe { get; }
		ReactiveCommand<KeyframeDTO> OnShapeKeyframeDeleted { get; }
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoPlayerData.TotalFrames.Value;

		public ReactiveValue<int> CurrentFrame { get; } = new();
		public ReactiveCommand<int> OnFrameChanged { get; } = new();
		
        public ReactiveCommand<KeyframeDTO> OnDrawTransformKeyframe { get; } = new();
		public ReactiveCommand<KeyframeDTO> OnTransfromKeyframeDeleted { get; } = new();

		public ReactiveCommand<KeyframeDTO> OnDrawShapeKeyframe { get; } = new();
		public ReactiveCommand<KeyframeDTO> OnShapeKeyframeDeleted { get; } = new();

		private readonly IVideoPlayerData _videoPlayerData;
		private readonly IEntitySelector _entitySelector;
		private int SelectedEntityId => _entitySelector.SelectedEntity.Id;

		private IKeyframeStorage _keyframeStorage;

        public VideoTimelineViewModel(IVideoPlayer videoPlayer, IVideoPlayerData videoPlayerData, IVideoEntities videoEntities,
									  ITransformComponentViewModel transformComponentViewModel, IKeyframeStorage keyframeStorage,
									  IShapeComponentViewModel shapeComponentViewModel, IEntitySelector entitySelector) 
		{
			videoPlayerData.CurrentFrame.Subscribe(currentFrame => {
				CurrentFrame.Value = currentFrame;

				if (currentFrame == TimelineData.FIRST_FRAME) {
					transformComponentViewModel.OnFirstKeyframe.Execute();
					shapeComponentViewModel.OnFirstKeyframe.Execute();
				}
				else {
					if (keyframeStorage.EntityHasKeyframeAtFrameOfType<Transform>(SelectedEntityId, currentFrame)) {
						transformComponentViewModel.OnFrameHasTransformKeyframe.Execute(true);
					}
					else {
						transformComponentViewModel.OnFrameHasTransformKeyframe.Execute(false);
					}
					if  (keyframeStorage.EntityHasKeyframeAtFrameOfType<Shape>(SelectedEntityId, currentFrame)) {
						shapeComponentViewModel.OnFrameHasShapeKeyframe.Execute(true);
					}
					else {
						shapeComponentViewModel.OnFrameHasShapeKeyframe.Execute(false);
					}
				}
			});

			OnFrameChanged.Subscribe(newFrame => videoPlayer.SetCurrentFrame(newFrame));

			transformComponentViewModel.OnDrawTransfromKeyframe.Subscribe(() => {
				OnDrawTransformKeyframe.Execute(GetTransformKeyframeDTO());
			});

			transformComponentViewModel.OnDeleteTransformKeyframe.Subscribe(() => {
				keyframeStorage.RemoveKeyframeOfType(typeof(Transform), _entitySelector.SelectedEntity.Id, CurrentFrame.Value);
				OnTransfromKeyframeDeleted.Execute(GetTransformKeyframeDTO());
			});

			transformComponentViewModel.OnUpdateTransformKeyframe.Subscribe(_ => {
				OnTransfromKeyframeDeleted.Execute(GetTransformKeyframeDTO());
				OnDrawTransformKeyframe.Execute(GetTransformKeyframeDTO());
			});

			shapeComponentViewModel.OnDrawShapeKeyframe.Subscribe(() => {
				OnDrawShapeKeyframe.Execute(GetShapeKeyframeDTO());
			});

			shapeComponentViewModel.OnDeleteShapeKeyframe.Subscribe(() => {
				keyframeStorage.RemoveKeyframeOfType(typeof(Shape), _entitySelector.SelectedEntity.Id, CurrentFrame.Value);
				OnShapeKeyframeDeleted.Execute(GetShapeKeyframeDTO());
			});

			shapeComponentViewModel.OnUpdateShapeKeyframe.Subscribe(_ => {
				OnShapeKeyframeDeleted.Execute(GetShapeKeyframeDTO());
				OnDrawShapeKeyframe.Execute(GetShapeKeyframeDTO());
			});

			videoEntities.OnCreateEntity.Subscribe(() => {
				OnDrawTransformKeyframe.Execute(GetTransformKeyframeDTO());
				OnDrawShapeKeyframe.Execute(GetShapeKeyframeDTO());
			});

			_keyframeStorage = keyframeStorage;
			_videoPlayerData = videoPlayerData;
			_entitySelector = entitySelector;
        }

		private KeyframeDTO GetTransformKeyframeDTO() {
			var keyframe = _keyframeStorage.GetEntityKeyframeOfType<Transform>(_entitySelector.SelectedEntity.Id, CurrentFrame.Value);
			return new KeyframeDTO(keyframe.EntityId, keyframe.Frame, ComponentDTO.Transform, keyframe.Ease);
		}

		private KeyframeDTO GetShapeKeyframeDTO() {
			var keyframe = _keyframeStorage.GetEntityKeyframeOfType<Shape>(_entitySelector.SelectedEntity.Id, CurrentFrame.Value);
			return new KeyframeDTO(keyframe.EntityId, keyframe.Frame, ComponentDTO.Shape, keyframe.Ease);
		}

    }
}