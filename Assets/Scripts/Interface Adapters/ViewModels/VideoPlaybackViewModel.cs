
namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand OnTogglePlay { get; }
        ReactiveCommand OnFirstFrame { get; }
        ReactiveCommand OnLastFrame { get; }
        ReactiveCommand OnBackwardFrame { get; }
        ReactiveCommand OnForwardFrame { get; }

        ReactiveCommand<bool> IsLooping { get; }
        ReactiveCommand<int> OnSetCurrentFrame { get; }
        ReactiveCommand<int> OnSetTotalFrames { get; }

        ReactiveValue<bool> IsPlaying { get; }
        ReactiveValue<float> CurrentTime { get; }
        ReactiveValue<float> DurationSeconds { get; }
        ReactiveValue<int> CurrentFrame { get; }
        ReactiveValue<int> TotalFrames { get; }

        void RefreshData();

    }

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand OnTogglePlay { get; } = new();
        public ReactiveCommand OnFirstFrame { get; } = new();
        public ReactiveCommand OnLastFrame { get; } = new();
        public ReactiveCommand OnBackwardFrame { get; } = new();
        public ReactiveCommand OnForwardFrame { get; } = new();

        public ReactiveCommand<bool> IsLooping { get; } = new();
        public ReactiveCommand<int> OnSetCurrentFrame { get; } = new();
        public ReactiveCommand<int> OnSetTotalFrames { get; } = new();

        public ReactiveValue<bool> IsPlaying { get; } = new();
        public ReactiveValue<float> CurrentTime { get; } = new();
        public ReactiveValue<float> DurationSeconds { get; } = new();
        public ReactiveValue<int> CurrentFrame { get; } = new();
        public ReactiveValue<int> TotalFrames { get; } = new();

        private readonly IVideoPlayerData _videoPlayerData;
        private readonly IVideoPlayback _videoPlayback;

        public VideoPlaybackViewModel(IVideoPlayerData videoPlayerData, IVideoPlayback videoPlayback) {
            _videoPlayerData = videoPlayerData;
            _videoPlayback = videoPlayback;

            InitReactiveCommands();
            InitReactiveValues();
        }

        public void RefreshData() {
            _videoPlayerData.SetReactiveValues();
        }

        private void InitReactiveCommands() {
            OnTogglePlay.   Subscribe(() => _videoPlayback.TogglePlay());
            OnFirstFrame.   Subscribe(() => _videoPlayback.SetFirstFrame());
            OnLastFrame.    Subscribe(() => _videoPlayback.SetLastFrame());
            OnBackwardFrame.Subscribe(() => _videoPlayback.DecreaseFrame());
            OnForwardFrame. Subscribe(() => _videoPlayback.IncreaseFrame());
            
            IsLooping.          Subscribe(isLooping => _videoPlayerData.IsLooping.Value = isLooping);
            OnSetCurrentFrame.  Subscribe(currentFrame => _videoPlayerData.CurrentFrame.Value = currentFrame);
            OnSetTotalFrames.   Subscribe(totalFrames => _videoPlayerData.TotalFrames.Value = totalFrames);
        }

        private void InitReactiveValues() {
            _videoPlayerData.IsPlaying.         Subscribe(isPlaying => IsPlaying.Value = isPlaying);
            _videoPlayerData.CurrentTime.       Subscribe(currentTime => CurrentTime.Value = currentTime);
            _videoPlayerData.DurationSeconds.   Subscribe(durationsSeconds => DurationSeconds.Value = durationsSeconds);
            _videoPlayerData.CurrentFrame.      Subscribe(currentFrame => CurrentFrame.Value = currentFrame);
            _videoPlayerData.TotalFrames.       Subscribe(totalFrames => TotalFrames.Value = totalFrames);
        }

    }
}