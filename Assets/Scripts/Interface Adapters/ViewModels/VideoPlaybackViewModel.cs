
namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand OnLastFrame { get; }
        ReactiveCommand OnTogglePlay { get; }
        ReactiveCommand OnFirstFrame { get; }
        ReactiveCommand OnForwardFrame { get; }
        ReactiveCommand OnBackwardFrame { get; }
        
        ReactiveCommand<bool> OnSetLoop { get; }
        ReactiveCommand<int> OnSetTotalFrames { get; }
        ReactiveCommand<int> OnSetCurrentFrame { get; }

        ReactiveValue<bool> IsPlaying { get; }
        ReactiveValue<bool> IsLooping { get; }
        ReactiveValue<int> TotalFrames { get; }
        ReactiveValue<int> CurrentFrame { get; }
        ReactiveValue<float> CurrentTime { get; }
        ReactiveValue<float> DurationSeconds { get; }
        ReactiveValue<int> TargetFramerate { get; }

        void InitVideoData();

    }

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand OnLastFrame { get; } = new();
        public ReactiveCommand OnTogglePlay { get; } = new();
        public ReactiveCommand OnFirstFrame { get; } = new();
        public ReactiveCommand OnForwardFrame { get; } = new();
        public ReactiveCommand OnBackwardFrame { get; } = new();

        public ReactiveCommand<bool> OnSetLoop { get; } = new();
        public ReactiveCommand<int> OnSetTotalFrames { get; } = new();
        public ReactiveCommand<int> OnSetCurrentFrame { get; } = new();

        public ReactiveValue<bool> IsPlaying { get; } = new();
        public ReactiveValue<bool> IsLooping { get; } = new();
        public ReactiveValue<int> TotalFrames { get; } = new();
        public ReactiveValue<int> CurrentFrame { get; } = new();
        public ReactiveValue<float> CurrentTime { get; } = new();
        public ReactiveValue<int> TargetFramerate { get; } = new();
        public ReactiveValue<float> DurationSeconds { get; } = new();


        private readonly IVideoPlayerData _videoPlayerData;
        private readonly IVideoPlayback _videoPlayback;

        public VideoPlaybackViewModel(IVideoPlayerData videoPlayerData, IVideoPlayback videoPlayback) {
            _videoPlayerData = videoPlayerData;
            _videoPlayback = videoPlayback;

            InitReactiveCommands();
            InitReactiveValues();
        }

        public void InitVideoData() {
            _videoPlayerData.InitReactiveData();
        }

        private void InitReactiveCommands() {
            OnTogglePlay.   Subscribe(() => _videoPlayback.TogglePlay());
            OnLastFrame.    Subscribe(() => _videoPlayback.SetLastFrame());
            OnFirstFrame.   Subscribe(() => _videoPlayback.SetFirstFrame());
            OnBackwardFrame.Subscribe(() => _videoPlayback.DecreaseFrame());
            OnForwardFrame. Subscribe(() => _videoPlayback.IncreaseFrame());
            
            OnSetLoop.          Subscribe(isLooping => _videoPlayerData.IsLooping.Value = isLooping);
            OnSetTotalFrames.   Subscribe(totalFrames => _videoPlayerData.TotalFrames.Value = totalFrames);
            OnSetCurrentFrame.  Subscribe(currentFrame => _videoPlayerData.CurrentFrame.Value = currentFrame);
            
            _videoPlayerData.TargetFrameRate.Subscribe(targetFramerate => TargetFramerate.Value = targetFramerate);
        }

        private void InitReactiveValues() {
            _videoPlayerData.IsPlaying.         Subscribe(isPlaying => IsPlaying.Value = isPlaying);
            _videoPlayerData.IsLooping.         Subscribe(isLooping => IsLooping.Value = isLooping);
            _videoPlayerData.CurrentTime.       Subscribe(currentTime => CurrentTime.Value = currentTime);
            _videoPlayerData.TotalFrames.       Subscribe(totalFrames => TotalFrames.Value = totalFrames);
            _videoPlayerData.CurrentFrame.      Subscribe(currentFrame => CurrentFrame.Value = currentFrame);
            _videoPlayerData.DurationSeconds.   Subscribe(durationsSeconds => DurationSeconds.Value = durationsSeconds);
        }

    }
}