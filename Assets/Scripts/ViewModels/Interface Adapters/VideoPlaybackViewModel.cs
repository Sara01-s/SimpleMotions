namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand<Void> OnFirstFrameUpdated { get; set; }
        ReactiveCommand<Void> OnBackwardUpdated { get; set; }
        ReactiveCommand<Void> OnTogglePlayUpdated { get; set; }
        ReactiveCommand<Void> OnForwardUpdated { get; set; }
        ReactiveCommand<Void> OnLastFrameUpdated { get; set; }
        ReactiveValue<int> CurrentFrame { get; set; }
        ReactiveValue<int> TotalFrames { get; set; }
        ReactiveValue<float> CurrentTime { get; set; }
        ReactiveValue<float> Duration { get; set; }

    }

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand<Void> OnFirstFrameUpdated { get; set; } = new();
        public ReactiveCommand<Void> OnBackwardUpdated { get; set; } = new();
        public ReactiveCommand<Void> OnTogglePlayUpdated { get; set; } = new();
        public ReactiveCommand<Void> OnForwardUpdated { get; set; } = new();
        public ReactiveCommand<Void> OnLastFrameUpdated { get; set; } = new();
        public ReactiveValue<float> CurrentTime { get; set; } = new();
        public ReactiveValue<float> Duration { get; set; } = new();
        public ReactiveValue<int> CurrentFrame { get; set; } = new();
        public ReactiveValue<int> TotalFrames { get; set; } = new();

        private readonly IVideoPlayback _videoPlayback;

        public VideoPlaybackViewModel(IVideoPlayback videoPlayback, IEventService eventService) {
            _videoPlayback = videoPlayback;

            OnFirstFrameUpdated.Subscribe(value => UpdateFirstFrame());
            OnBackwardUpdated.Subscribe(value => UpdateBackward());
            OnTogglePlayUpdated.Subscribe(value => UpdateTogglePlay());
            OnForwardUpdated.Subscribe(value => UpdateForward());
            OnLastFrameUpdated.Subscribe(value => UpdateLastFrame());

            eventService.Subscribe<VideoDisplayInfo>(value => UpdateCurrentTime(value.CurrentTime));
            eventService.Subscribe<VideoDisplayInfo>(value => UpdateCurrentFrame(value.CurrentFrame));
            eventService.Subscribe<VideoDisplayInfo>(value => UpdateTotalFrames(value.TotalFrames));
            eventService.Subscribe<VideoDisplayInfo>(value => UpdateDuration(value.Duration));
        }

        private void UpdateFirstFrame() {
            _videoPlayback.SetCurrentFrame(_videoPlayback.SetFirstFrame());
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
            _videoPlayback.SetCurrentFrame(_videoPlayback.SetLastFrame());
        }

        private void UpdateCurrentTime(float time) {
            CurrentTime.Value = time;
        }

        private void UpdateCurrentFrame(int frame) {
            CurrentFrame.Value = frame;
        }

        private void UpdateTotalFrames(int frame) {
            TotalFrames.Value = frame;
        }

        private void UpdateDuration(float time) {
            Duration.Value = time;
        }

    }
}