
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {

		int TotalFrameCount { get; } 
		ReactiveValue<int> CurrentFrame { get; }
		ReactiveCommand<int> OnFrameChanged { get; }
		ReactiveCommand DrawTrasnformKeyframe { get; }

	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoTimeline.TotalFrames;
		public ReactiveValue<int> CurrentFrame { get; } = new();
		public ReactiveCommand<int> OnFrameChanged { get; } = new();
        public ReactiveCommand DrawTrasnformKeyframe { get; } = new();

        private readonly IVideoTimeline _videoTimeline;
		private readonly IVideoPlayerData _videoPlayerData;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoPlayerData videoPlayerData, ITransformComponentViewModel transformComponentViewModel) {
			videoPlayerData.CurrentFrame.Subscribe(UpdateCursorPosition);
			OnFrameChanged.Subscribe(value => SetCurrentFrame(value));

			_videoTimeline = videoTimeline;
			_videoPlayerData = videoPlayerData;
			
			transformComponentViewModel.DrawTransfromKeyframe.Subscribe(DrawTransfromKeyFrame);
        }

		private void UpdateCursorPosition(int currentFrame) {
			CurrentFrame.Value = currentFrame;
		}

		private void SetCurrentFrame(int frame) {
			_videoTimeline.SetCurrentFrame(frame);
		}

		public void DrawTransfromKeyFrame() {
			DrawTrasnformKeyframe.Execute();
		}

    }
}