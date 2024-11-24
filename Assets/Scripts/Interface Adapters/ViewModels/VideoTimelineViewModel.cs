using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoTimelineViewModel {
		int TotalFrameCount { get; } 

		ReactiveValue<int> CurrentFrame { get; }
		ReactiveCommand<int> OnFrameChanged { get; }

		ReactiveCommand OnDrawTransformKeyframe { get; }
		ReactiveCommand OnTransfromKeyframeDeleted { get; }

		ReactiveCommand OnDrawShapeKeyframe { get; }
		ReactiveCommand OnShapeKeyframeDeleted { get; }
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoPlayerData.TotalFrames.Value;

		public ReactiveValue<int> CurrentFrame { get; } = new();
		public ReactiveCommand<int> OnFrameChanged { get; } = new();
		
        public ReactiveCommand OnDrawTransformKeyframe { get; } = new();
		public ReactiveCommand OnTransfromKeyframeDeleted { get; } = new();

		public ReactiveCommand OnDrawShapeKeyframe { get; } = new();
		public ReactiveCommand OnShapeKeyframeDeleted { get; } = new();

		private readonly IVideoPlayerData _videoPlayerData;

        public VideoTimelineViewModel(IVideoPlayer videoPlayer, IVideoPlayerData videoPlayerData, IVideoEntities videoEntities,
									  ITransformComponentViewModel transformComponentViewModel, IKeyframeStorage keyframeStorage,
									  IShapeComponentViewModel shapeComponentViewModel) 
		{
			videoPlayerData.CurrentFrame.Subscribe(currentFrame => {
				CurrentFrame.Value = currentFrame;

				if (currentFrame == TimelineData.FIRST_FRAME) {
					transformComponentViewModel.OnFirstKeyframe.Execute();
					shapeComponentViewModel.OnFirstKeyframe.Execute();
				}
				else {
					if (keyframeStorage.FrameHasKeyframeOfTypeAt<Transform>(currentFrame)) {
						transformComponentViewModel.OnFrameHasTransformKeyframe.Execute(true);
					}
					else {
						transformComponentViewModel.OnFrameHasTransformKeyframe.Execute(false);
					}
					if  (keyframeStorage.FrameHasKeyframeOfTypeAt<Shape>(currentFrame)) {
						shapeComponentViewModel.OnFrameHasShapeKeyframe.Execute(true);
					}
					else {
						shapeComponentViewModel.OnFrameHasShapeKeyframe.Execute(false);
					}
				}
			});

			OnFrameChanged.Subscribe(newFrame => videoPlayer.SetCurrentFrame(newFrame));

			transformComponentViewModel.OnDrawTransfromKeyframe.Subscribe(OnDrawTransformKeyframe.Execute);
			transformComponentViewModel.OnDeleteTransformKeyframe.Subscribe(OnTransfromKeyframeDeleted.Execute);
			transformComponentViewModel.OnUpdateTransformKeyframe.Subscribe(_ => { 
				OnTransfromKeyframeDeleted.Execute();
				OnDrawTransformKeyframe.Execute();
			});

			shapeComponentViewModel.OnDrawShapeKeyframe.Subscribe(OnDrawShapeKeyframe.Execute);
			shapeComponentViewModel.OnDeleteShapeKeyframe.Subscribe(OnShapeKeyframeDeleted.Execute);
			shapeComponentViewModel.OnUpdateShapeKeyframe.Subscribe(_ => {
				OnShapeKeyframeDeleted.Execute();
				OnDrawShapeKeyframe.Execute();
			});

			videoEntities.OnCreateEntity.Subscribe(() => {
				OnDrawTransformKeyframe.Execute();
				OnDrawShapeKeyframe.Execute();
			});

			_videoPlayerData = videoPlayerData;
        }

    }
}