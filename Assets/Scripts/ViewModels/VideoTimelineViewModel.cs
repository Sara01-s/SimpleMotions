
namespace SimpleMotions {

    internal sealed class VideoTimelineViewModel : IVideoTimelineViewModel {

        public ReactiveCommand<Void> TogglePlay { get; set; } = new();
        public ReactiveValue<float> CurrentTime { get; set; } = new();
        public ReactiveValue<float> Duration { get; set; } = new();

        private readonly IVideoTimeline _videoTimeline;
        private bool _isPlaying = false;

        public VideoTimelineViewModel(IVideoTimeline videoTimeline) {
            _videoTimeline = videoTimeline;

            TogglePlay.Subscribe(value => OnTogglePlay());
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

    }
}