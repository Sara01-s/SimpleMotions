namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand<Void> TogglePlay { get; set; }
        ReactiveValue<int> CurrentFrame { get; set; }
        ReactiveValue<int> TotalFrames { get; set; }
        ReactiveValue<float> CurrentTime { get; set; }
        ReactiveValue<float> Duration { get; set; }

    }

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