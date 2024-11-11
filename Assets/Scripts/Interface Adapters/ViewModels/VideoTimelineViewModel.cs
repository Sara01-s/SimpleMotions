
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {

		int TotalFrameCount { get; } 
		ReactiveCommand<Void> OnCreateTestEntity { get; }
		ReactiveCommand<int> OnSetCurrentFrame { get; }
		public ReactiveValue<int> CurrentFrame { get; }

		void RefreshData();
		
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public int TotalFrameCount => _videoTimeline.TotalFrames;
		public ReactiveCommand<Void> OnCreateTestEntity { get; } = new();
		public ReactiveCommand<int> OnSetCurrentFrame { get; } = new();
		public ReactiveValue<int> CurrentFrame { get; } = new();

		private readonly IVideoTimeline _videoTimeline;
        private readonly IVideoEntities _videoEntities;

		private IVideoPlayerData _videoPlayerData;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoEntities videoEntities, IVideoPlayerData videoPlayerData) {
			_videoTimeline = videoTimeline;
			_videoEntities = videoEntities;
			_videoPlayerData = videoPlayerData;

			_videoPlayerData.CurrentFrame.Subscribe(UpdateCursorPosition);

			OnCreateTestEntity.Subscribe(value => CreateTestEntity());
			OnSetCurrentFrame.Subscribe(value => SetCurrentFrame(value));
        }

		private void UpdateCursorPosition(int currentFrame) {
			CurrentFrame.Value = currentFrame;
		}

		private void SetCurrentFrame(int frame) {
			_videoTimeline.SetCurrentFrame(frame);
		}

		private void CreateTestEntity() {
			_videoEntities.CreateTestEntity();
		}

		public void RefreshData() {
			_videoPlayerData.SetReactiveValues();
		}

    }
}