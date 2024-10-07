namespace SimpleMotions {

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand<Void> TogglePlay { get; set; } = new();
        public ReactiveValue<float> CurrentTime { get; set; } = new();
        public ReactiveValue<float> Duration { get; set; } = new();
        public ReactiveValue<int> CurrentFrame { get; set; } = new();
        public ReactiveValue<int> TotalFrames { get; set; } = new();

        private readonly IVideoPlayback _videoPlayback;

        public VideoPlaybackViewModel(IVideoPlayback videoPlayback) {
            _videoPlayback = videoPlayback;

            TogglePlay.Subscribe(value => OnTogglePlay());
        }

        private void OnTogglePlay() {
            _videoPlayback.TogglePlay();
        }

    }
}