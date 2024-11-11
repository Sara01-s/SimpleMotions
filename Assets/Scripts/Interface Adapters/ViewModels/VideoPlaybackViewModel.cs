
namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand<Void> OnTogglePlayUpdated { get; }
        ReactiveCommand<Void> OnFirstFrameUpdated { get; }
        ReactiveCommand<Void> OnLastFrameUpdated { get; }
        ReactiveCommand<Void> OnBackwardUpdated { get; }
        ReactiveCommand<Void> OnForwardUpdated { get; }

        ReactiveValue<bool> IsPlaying { get; }
        ReactiveValue<float> CurrentTime { get; }
        ReactiveValue<float> DurationSeconds { get; }
        ReactiveValue<int> CurrentFrame { get; }
        ReactiveValue<int> TotalFrames { get; }

        void RefreshData();

    }

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand<Void> OnTogglePlayUpdated { get; } = new();
        public ReactiveCommand<Void> OnFirstFrameUpdated { get; } = new();
        public ReactiveCommand<Void> OnLastFrameUpdated { get; } = new();
        public ReactiveCommand<Void> OnBackwardUpdated { get; } = new();
        public ReactiveCommand<Void> OnForwardUpdated { get; } = new();

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
            OnTogglePlayUpdated.Subscribe(value => UpdateTogglePlay());
            OnFirstFrameUpdated.Subscribe(value => UpdateFirstFrame());
            OnLastFrameUpdated.Subscribe(value => UpdateLastFrame());
            OnBackwardUpdated.Subscribe(value => UpdateBackward());
            OnForwardUpdated.Subscribe(value => UpdateForward());
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