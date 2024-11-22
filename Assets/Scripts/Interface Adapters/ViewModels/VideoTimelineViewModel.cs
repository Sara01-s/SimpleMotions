
using System.Windows.Forms;

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
		private readonly IVideoPlayer _videoPlayer;

        public VideoTimelineViewModel(IVideoPlayer videoPlayer, IVideoPlayerData videoPlayerData, 
									  ITransformComponentViewModel transformComponentViewModel, IKeyframeStorage keyframeStorage,
									  IShapeComponentViewModel shapeComponentViewModel) 
		{
			videoPlayerData.CurrentFrame.Subscribe(currentFrame => {
				if  (keyframeStorage.FrameHasKeyframe(currentFrame) && !_videoPlayerData.IsPlaying.Value) {
					transformComponentViewModel.OnFrameHasKeyframe.Execute(true);
					shapeComponentViewModel.OnFrameHasKeyframe.Execute(true);
				}
				else {
					transformComponentViewModel.OnFrameHasKeyframe.Execute(false);
					shapeComponentViewModel.OnFrameHasKeyframe.Execute(false);
				}

				UpdateCursorPosition(currentFrame);
			});

			OnFrameChanged.Subscribe(newFrame => SetCurrentFrame(newFrame));

			transformComponentViewModel.OnDrawTransfromKeyframe.Subscribe(OnDrawTransformKeyframe.Execute);
			transformComponentViewModel.OnTransformKeyframeDeleted.Subscribe(OnTransfromKeyframeDeleted.Execute);

			shapeComponentViewModel.OnDrawShapeKeyframe.Subscribe(OnDrawShapeKeyframe.Execute);
			shapeComponentViewModel.OnShapeKeyframeDeleted.Subscribe(OnShapeKeyframeDeleted.Execute);

			_videoPlayer = videoPlayer;
			_videoPlayerData = videoPlayerData;
        }

		private void UpdateCursorPosition(int currentFrame) {
			CurrentFrame.Value = currentFrame;
		}

		private void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
		}

    }
}