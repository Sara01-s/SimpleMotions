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

        public VideoTimelineViewModel(IVideoPlayer videoPlayer, IVideoPlayerData videoPlayerData, 
									  ITransformComponentViewModel transformComponentViewModel, IKeyframeStorage keyframeStorage,
									  IShapeComponentViewModel shapeComponentViewModel) 
		{
			videoPlayerData.CurrentFrame.Subscribe(currentFrame => {
				if (keyframeStorage.FrameHasKeyframeOfTypeAt<Transform>(currentFrame) && !videoPlayerData.IsPlaying.Value){
					transformComponentViewModel.OnFrameHasTransformKeyframe.Execute(true);
				}
				else {
					transformComponentViewModel.OnFrameHasTransformKeyframe.Execute(false);
				}

				if  (keyframeStorage.FrameHasKeyframeOfTypeAt<Shape>(currentFrame) && !videoPlayerData.IsPlaying.Value) {
					shapeComponentViewModel.OnFrameHasShapeKeyframe.Execute(true);
				}
				else {
					shapeComponentViewModel.OnFrameHasShapeKeyframe.Execute(false);
				}

				CurrentFrame.Value = currentFrame;
			});

			OnFrameChanged.Subscribe(newFrame => videoPlayer.SetCurrentFrame(newFrame));

			transformComponentViewModel.OnDrawTransfromKeyframe.Subscribe(OnDrawTransformKeyframe.Execute);
			transformComponentViewModel.OnTransformKeyframeDeleted.Subscribe(OnTransfromKeyframeDeleted.Execute);

			shapeComponentViewModel.OnDrawShapeKeyframe.Subscribe(OnDrawShapeKeyframe.Execute);
			shapeComponentViewModel.OnShapeKeyframeDeleted.Subscribe(OnShapeKeyframeDeleted.Execute);

			_videoPlayerData = videoPlayerData;
        }

    }
}