
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {
		int TotalFrameCount { get; } 
		
		ReactiveValue<int> CurrentFrame { get; }
		ReactiveCommand<int> OnFrameChanged { get; }

		ReactiveCommand DrawTransformKeyframe { get; }
		ReactiveCommand OnKeyframeDeleted { get; }
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoPlayerData.TotalFrames.Value;

		public ReactiveValue<int> CurrentFrame { get; } = new();
		public ReactiveCommand<int> OnFrameChanged { get; } = new();
		
        public ReactiveCommand DrawTransformKeyframe { get; } = new();
		public ReactiveCommand OnKeyframeDeleted { get; } = new();

		private readonly IVideoPlayerData _videoPlayerData;
		private readonly IVideoPlayer _videoPlayer;

        public VideoTimelineViewModel(IVideoPlayer videoPlayer, IVideoPlayerData videoPlayerData, ITransformComponentViewModel transformComponentViewModel, IKeyframeStorage keyframeStorage) {
			transformComponentViewModel.OnDrawTransfromKeyframe.Subscribe(DrawTransfromKeyFrame);
			
			videoPlayerData.CurrentFrame.Subscribe(currentFrame => {
				if  (keyframeStorage.FrameHasKeyframe(currentFrame) && !_videoPlayerData.IsPlaying.Value) {
					transformComponentViewModel.OnFrameHasKeyframe.Execute(true);
				}
				else {
					transformComponentViewModel.OnFrameHasKeyframe.Execute(false);
				}

				UpdateCursorPosition(currentFrame);
			});

			OnFrameChanged.Subscribe(newFrame => SetCurrentFrame(newFrame));

			transformComponentViewModel.OnKeyframeDeleted.Subscribe(OnKeyframeDeleted.Execute);

			_videoPlayer = videoPlayer;
			_videoPlayerData = videoPlayerData;
        }

		private void UpdateCursorPosition(int currentFrame) {
			CurrentFrame.Value = currentFrame;
		}

		private void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
		}

		public void DrawTransfromKeyFrame() {
			DrawTransformKeyframe.Execute();
		}

    }
}