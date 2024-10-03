
namespace SimpleMotions {

    internal sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public ReactiveCommand<Void> TogglePlay { get; set; } = new();
        public ReactiveValue<float> CurrentTime { get; set; } = new();
        public ReactiveValue<float> Duration { get; set; } = new();
		public ReactiveCommand<Void> CreateTestEntity { get; set; } = new();

		private readonly IVideoTimeline _videoTimeline;
		private readonly IVideoEntities _videoEntities;
        private bool _isPlaying = false;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoEntities videoEntities) {
            _videoTimeline = videoTimeline;
			_videoEntities = videoEntities;

            TogglePlay.Subscribe(value => OnTogglePlay());
			CreateTestEntity.Subscribe(value => OnCreateTestEntity());
        }

        private void OnTogglePlay() {
            _isPlaying = !_isPlaying;

            if (_isPlaying) {
                _videoTimeline.Resume();
            }
            else {
                _videoTimeline.Pause();
            }
        }

		private void OnCreateTestEntity() {
			_videoEntities.CreateTestEntity();
		}

    }
}