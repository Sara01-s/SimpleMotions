
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {
		int TotalFrameCount { get; } 
		ReactiveCommand<Void> OnCreateTestEntity { get; }
		ReactiveCommand<int> OnSetCurrentFrame { get; }
		ReactiveCommand<VideoDisplayInfo> OnTimelineUpdate { get; }
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

		public ReactiveCommand<Void> OnCreateTestEntity { get; } = new();
		public ReactiveCommand<int> OnSetCurrentFrame { get; } = new();
		public ReactiveCommand<VideoDisplayInfo> OnTimelineUpdate { get; } = new();

        public int TotalFrameCount => _videoTimeline.TotalFrames;

		private readonly IVideoTimeline _videoTimeline;
        private readonly IVideoEntities _videoEntities;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoEntities videoEntities, IEventService eventService) {
			_videoTimeline = videoTimeline;
			_videoEntities = videoEntities;

			eventService.Subscribe<VideoDisplayInfo>(UpdateTimeline);

			OnCreateTestEntity.Subscribe(value => CreateTestEntity());
			OnSetCurrentFrame.Subscribe(value => SetCurrentFrame(value));
        }

		private void UpdateTimeline(VideoDisplayInfo videoDisplayInfo) {
			OnTimelineUpdate.Execute(videoDisplayInfo);
		}

		private void SetCurrentFrame(int frame) {
			_videoTimeline.SetCurrentFrame(frame);
		}

		private void CreateTestEntity() {
			_videoEntities.CreateTestEntity();
		}

    }
}