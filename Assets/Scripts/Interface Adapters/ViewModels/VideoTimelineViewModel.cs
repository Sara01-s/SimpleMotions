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
				var transformEntityKeyframe = new KeyframeDTO(ComponentType.Transform, _entitySelector.SelectedEntity.Id, _videoPlayerData.CurrentFrame.Value);
				OnDrawTransformKeyframe.Execute(transformEntityKeyframe);
			});

			transformComponentViewModel.OnDeleteTransformKeyframe.Subscribe(() => {
				var transformEntityKeyframe = new KeyframeDTO(ComponentType.Transform, _entitySelector.SelectedEntity.Id, _videoPlayerData.CurrentFrame.Value);
				OnTransfromKeyframeDeleted.Execute(transformEntityKeyframe);
			});

			transformComponentViewModel.OnUpdateTransformKeyframe.Subscribe(_ => { 
				var transformEntityKeyframe = new KeyframeDTO(ComponentType.Transform, _entitySelector.SelectedEntity.Id, _videoPlayerData.CurrentFrame.Value);
				OnTransfromKeyframeDeleted.Execute(transformEntityKeyframe);
				OnDrawTransformKeyframe.Execute(transformEntityKeyframe);
			});

			shapeComponentViewModel.OnDrawShapeKeyframe.Subscribe(() => {
				var shapeEntityKeyframe = new KeyframeDTO(ComponentType.Shape, _entitySelector.SelectedEntity.Id, _videoPlayerData.CurrentFrame.Value);
				OnDrawShapeKeyframe.Execute(shapeEntityKeyframe);
			});

			shapeComponentViewModel.OnDeleteShapeKeyframe.Subscribe(() => {
				var shapeEntityKeyframe = new KeyframeDTO(ComponentType.Shape, _entitySelector.SelectedEntity.Id, _videoPlayerData.CurrentFrame.Value);
				OnShapeKeyframeDeleted.Execute(shapeEntityKeyframe);
			});
			shapeComponentViewModel.OnUpdateShapeKeyframe.Subscribe(_ => {
				var shapeEntityKeyframe = new KeyframeDTO(ComponentType.Shape, _entitySelector.SelectedEntity.Id, _videoPlayerData.CurrentFrame.Value);
				OnShapeKeyframeDeleted.Execute(shapeEntityKeyframe);
				OnDrawShapeKeyframe.Execute(shapeEntityKeyframe);
			});

			videoEntities.OnCreateEntity.Subscribe(() => {
				var transformEntityKeyframe = new KeyframeDTO(ComponentType.Transform, _entitySelector.SelectedEntity.Id, 0);
				OnDrawTransformKeyframe.Execute(transformEntityKeyframe);

				var shapeEntityKeyframe = new KeyframeDTO(ComponentType.Shape, _entitySelector.SelectedEntity.Id, 0);
				OnDrawShapeKeyframe.Execute(shapeEntityKeyframe);
			});

			_videoPlayerData = videoPlayerData;
			_entitySelector = entitySelector;
        }

    }
}