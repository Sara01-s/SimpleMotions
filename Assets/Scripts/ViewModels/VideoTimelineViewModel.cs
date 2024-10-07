
namespace SimpleMotions {

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

		public ReactiveCommand<Void> CreateTestEntity { get; set; } = new();

		private readonly IVideoEntities _videoEntities;

        public VideoTimelineViewModel(IVideoEntities videoEntities) {
			_videoEntities = videoEntities;

			CreateTestEntity.Subscribe(value => OnCreateTestEntity());
        }

		private void OnCreateTestEntity() {
			_videoEntities.CreateTestEntity();
		}

    }
}