
namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand<Void> OnFirstFrameUpdated { get; }
        ReactiveCommand<Void> OnBackwardUpdated { get; }
        ReactiveCommand<Void> OnTogglePlayUpdated { get; }
        ReactiveCommand<Void> OnForwardUpdated { get; }
        ReactiveCommand<Void> OnLastFrameUpdated { get; }

        ReactiveValue<int> CurrentFrame { get; }
        ReactiveValue<int> TotalFrames { get; }
        ReactiveValue<bool> IsPlaying { get; }
        ReactiveValue<float> CurrentTime { get; }
        ReactiveValue<float> DurationSeconds { get; }

    }

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand<Void> OnFirstFrameUpdated { get; } = new();
        public ReactiveCommand<Void> OnBackwardUpdated { get; } = new();
        public ReactiveCommand<Void> OnTogglePlayUpdated { get; } = new();
        public ReactiveCommand<Void> OnForwardUpdated { get; } = new();
        public ReactiveCommand<Void> OnLastFrameUpdated { get; } = new();

        public ReactiveValue<float> CurrentTime { get; } = new();
        public ReactiveValue<int> CurrentFrame { get; } = new();
        public ReactiveValue<bool> IsPlaying { get; } = new();
        public ReactiveValue<int> TotalFrames { get; } = new();
        public ReactiveValue<float> DurationSeconds { get; } = new();

        private readonly IVideoPlayback _videoPlayback;

        public VideoPlaybackViewModel(IVideoPlayback videoPlayback, IVideoPlayerData videoPlayerData) {
            _videoPlayback = videoPlayback;

            OnFirstFrameUpdated.Subscribe(value => UpdateFirstFrame());
            OnBackwardUpdated.Subscribe(value => UpdateBackward());
            OnTogglePlayUpdated.Subscribe(value => UpdateTogglePlay());
            OnForwardUpdated.Subscribe(value => UpdateForward());
            OnLastFrameUpdated.Subscribe(value => UpdateLastFrame());

            videoPlayerData.CurrentFrame.Subscribe(UpdateCurrentFrame);
            videoPlayerData.CurrentTime.Subscribe(UpdateCurrentTime);
            videoPlayerData.TotalFrames.Subscribe(UpdateTotalFrames);
            videoPlayerData.DurationSeconds.Subscribe(UpdateDurationSeconds);
            videoPlayerData.IsPlaying.Subscribe(UpdateTogglePlayIcon);
        }

        private void UpdateFirstFrame() {
            _videoPlayback.SetFirstFrame();
        }

        private void UpdateBackward() {
            _videoPlayback.DecreaseFrame();
        }

        private void UpdateTogglePlay() {
            _videoPlayback.TogglePlay();
        }

        private void UpdateForward() {
            _videoPlayback.IncreaseFrame();
        }

        private void UpdateLastFrame() {
            _videoPlayback.SetLastFrame();
        }
        

        private void UpdateCurrentTime(float time) {
            CurrentTime.Value = time;
        }

        private void UpdateCurrentFrame(int frame) {
            CurrentFrame.Value = frame;
        }

        private void UpdateTogglePlayIcon(bool isPlaying) {
            IsPlaying.Value = isPlaying;
        }

        private void UpdateTotalFrames(int frame) {
            TotalFrames.Value = frame;
        }

        private void UpdateDurationSeconds(float time) {
            DurationSeconds.Value = time;
        }

    }
}