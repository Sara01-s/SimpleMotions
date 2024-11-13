
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {

		int TotalFrameCount { get; } 
		ReactiveCommand<int> OnSetCurrentFrame { get; }
		ReactiveValue<int> CurrentFrame { get; }

		void RefreshData();
		
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoTimeline.TotalFrames;
		public ReactiveCommand<int> OnSetCurrentFrame { get; } = new();
		public ReactiveValue<int> CurrentFrame { get; } = new();

		private readonly IVideoTimeline _videoTimeline;
		private readonly IVideoPlayerData _videoPlayerData;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoPlayerData videoPlayerData) {
			_videoTimeline = videoTimeline;
			_videoPlayerData = videoPlayerData;

			_videoPlayerData.CurrentFrame.Subscribe(UpdateCursorPosition);

			OnSetCurrentFrame.Subscribe(value => SetCurrentFrame(value));
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