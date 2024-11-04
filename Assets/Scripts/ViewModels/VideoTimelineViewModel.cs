
namespace SimpleMotions {

	public interface IVideoTimelineViewModel {
		int DefaultFrameCount { get; } 
		ReactiveCommand<Void> OnCreateTestEntity { get; }
		ReactiveCommand<int> OnSetCurrentFrame { get; }
		ReactiveCommand<VideoDisplayInfo> OnTimelineUpdate { get; }
	}

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

		public ReactiveCommand<Void> OnCreateTestEntity { get; } = new();
		public ReactiveCommand<int> OnSetCurrentFrame { get; } = new();
		public ReactiveCommand<VideoDisplayInfo> OnTimelineUpdate { get; } = new();

        public int DefaultFrameCount => _videoTimeline.DefaultKeyframes;

        private readonly IVideoEntities _videoEntities;
		private readonly IVideoTimeline _videoTimeline;

        public VideoTimelineViewModel(IVideoEntities videoEntities, IVideoTimeline videoTimeline, IEventService eventService) {
			_videoEntities = videoEntities;
			_videoTimeline = videoTimeline;

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