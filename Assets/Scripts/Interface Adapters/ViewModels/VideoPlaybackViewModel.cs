
namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand<Void> OnTogglePlay { get; }
        ReactiveCommand<Void> OnFirstFrame { get; }
        ReactiveCommand<Void> OnLastFrame { get; }
        ReactiveCommand<Void> OnBackwardFrame { get; }
        ReactiveCommand<Void> OnForwardFrame { get; }

        ReactiveValue<bool> IsPlaying { get; }
        ReactiveValue<float> CurrentTime { get; }
        ReactiveValue<float> DurationSeconds { get; }
        ReactiveValue<int> CurrentFrame { get; }
        ReactiveValue<int> TotalFrames { get; }

        void RefreshData();

    }

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand<Void> OnTogglePlay { get; } = new();
        public ReactiveCommand<Void> OnFirstFrame { get; } = new();
        public ReactiveCommand<Void> OnLastFrame { get; } = new();
        public ReactiveCommand<Void> OnBackwardFrame { get; } = new();
        public ReactiveCommand<Void> OnForwardFrame { get; } = new();

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

        private void UpdateTogglePlay() {
            _videoPlayback.TogglePlay();
        }

        private void UpdateFirstFrame() {
            _videoPlayback.SetFirstFrame();
        }

        private void UpdateLastFrame() {
            _videoPlayback.SetLastFrame();
        }

        private void UpdateBackward() {
            _videoPlayback.DecreaseFrame();
        }

        private void UpdateForward() {
            _videoPlayback.IncreaseFrame();
        }

        private void UpdateTogglePlayIcon(bool isPlaying) {
            IsPlaying.Value = isPlaying;
        }

        private void UpdateCurrentTime(float time) {
            CurrentTime.Value = time;
        }

        private void UpdateDurationSeconds(float time) {
            DurationSeconds.Value = time;
        }

        private void UpdateCurrentFrame(int frame) {
            CurrentFrame.Value = frame;
        }

        private void UpdateTotalFrames(int frame) {
            TotalFrames.Value = frame;
        }

        public void RefreshData() {
            _videoPlayerData.SetReactiveValues();
        }

        private void InitReactiveCommands() {
            OnTogglePlay.Subscribe(value => UpdateTogglePlay());
            OnFirstFrame.Subscribe(value => UpdateFirstFrame());
            OnLastFrame.Subscribe(value => UpdateLastFrame());
            OnBackwardFrame.Subscribe(value => UpdateBackward());
            OnForwardFrame.Subscribe(value => UpdateForward());
        }

        private void InitReactiveValues() {
            _videoPlayerData.IsPlaying.Subscribe(UpdateTogglePlayIcon);
            _videoPlayerData.CurrentTime.Subscribe(UpdateCurrentTime);
            _videoPlayerData.DurationSeconds.Subscribe(UpdateDurationSeconds);
            _videoPlayerData.CurrentFrame.Subscribe(UpdateCurrentFrame);
            _videoPlayerData.TotalFrames.Subscribe(UpdateTotalFrames);
        }

    }
}