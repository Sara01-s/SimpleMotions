
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {

		int TotalFrameCount { get; } 
		ReactiveValue<int> CurrentFrame { get; }
		ReactiveCommand<int> OnFrameChanged { get; }

		void RefreshData();
		
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoTimeline.TotalFrames;
		public ReactiveValue<int> CurrentFrame { get; } = new();
		public ReactiveCommand<int> OnFrameChanged { get; } = new();

		private readonly IVideoTimeline _videoTimeline;
		private readonly IVideoPlayerData _videoPlayerData;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoPlayerData videoPlayerData) {
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

    }
}