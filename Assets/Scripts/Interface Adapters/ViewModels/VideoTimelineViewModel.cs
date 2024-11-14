
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {

		int TotalFrameCount { get; } 
		ReactiveValue<int> CurrentFrame { get; }
		ReactiveCommand<int> OnFrameChanged { get; }
		ReactiveCommand ShowKeyframe { get; }

		void RefreshData();
		
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoTimeline.TotalFrames;
		public ReactiveValue<int> CurrentFrame { get; } = new();
		public ReactiveCommand<int> OnFrameChanged { get; } = new();
        public ReactiveCommand ShowKeyframe { get; } = new();

        private readonly IVideoTimeline _videoTimeline;
		private readonly IVideoPlayerData _videoPlayerData;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoPlayerData videoPlayerData, ITransformComponentViewModel transformComponentViewModel) {
			// XD
			transformComponentViewModel.SaveTransformKeyframe.Subscribe(ShowKeyframeXD);
			_videoTimeline = videoTimeline;
			_videoPlayerData = videoPlayerData;

			_videoPlayerData.CurrentFrame.Subscribe(UpdateCursorPosition);
			OnFrameChanged.Subscribe(value => SetCurrentFrame(value));
        }

		private void UpdateCursorPosition(int currentFrame) {
			CurrentFrame.Value = currentFrame;
		}

		private void SetCurrentFrame(int frame) {
			_videoTimeline.SetCurrentFrame(frame);
		}

		public void RefreshData() {
			_videoPlayerData.SetReactiveValues();
		}

		public void ShowKeyframeXD(((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees) xd) {
			ShowKeyframe.Execute();
		}

    }
}