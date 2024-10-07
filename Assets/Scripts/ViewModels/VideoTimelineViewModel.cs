
namespace SimpleMotions {

    public sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public ReactiveCommand<Void> TogglePlay { get; set; } = new();
        public ReactiveValue<float> CurrentTime { get; set; } = new();
        public ReactiveValue<float> Duration { get; set; } = new();
		public ReactiveCommand<Void> CreateTestEntity { get; set; } = new();

		private readonly IVideoTimeline _videoTimeline;
		private readonly IVideoEntities _videoEntities;
        private bool _shouldPlay = false;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline, IVideoEntities videoEntities) {
            _videoTimeline = videoTimeline;
			_videoEntities = videoEntities;

            TogglePlay.Subscribe(value => OnTogglePlay());
			CreateTestEntity.Subscribe(value => OnCreateTestEntity());
        }

        private void OnTogglePlay() {
            _shouldPlay = !_shouldPlay;

            if (_shouldPlay) {
                _videoTimeline.Play();
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