using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace SimpleMotions {

	public interface IVideoPlaybackViewModel {
        
        ReactiveCommand<Void> FirstFrame { get; set; }
        ReactiveCommand<Void> Backward { get; set; }
        ReactiveCommand<Void> TogglePlay { get; set; }
        ReactiveCommand<Void> Forward { get; set; }
        ReactiveCommand<Void> LastFrame { get; set; }
        ReactiveValue<int> CurrentFrame { get; set; }
        ReactiveValue<int> TotalFrames { get; set; }
        ReactiveValue<float> CurrentTime { get; set; }
        ReactiveValue<float> Duration { get; set; }

    }

    public sealed class VideoPlaybackViewModel : IVideoPlaybackViewModel {

        public ReactiveCommand<Void> FirstFrame { get; set; } = new();
        public ReactiveCommand<Void> Backward { get; set; } = new();
        public ReactiveCommand<Void> TogglePlay { get; set; } = new();
        public ReactiveCommand<Void> Forward { get; set; } = new();
        public ReactiveCommand<Void> LastFrame { get; set; } = new();
        public ReactiveValue<float> CurrentTime { get; set; } = new();
        public ReactiveValue<float> Duration { get; set; } = new();
        public ReactiveValue<int> CurrentFrame { get; set; } = new();
        public ReactiveValue<int> TotalFrames { get; set; } = new();

        private readonly IVideoPlayback _videoPlayback;

        public VideoPlaybackViewModel(IVideoPlayback videoPlayback, IEventService eventService) {
            _videoPlayback = videoPlayback;

            FirstFrame.Subscribe(value => OnFirstFrame());
            Backward.Subscribe(value => OnBackward());
            TogglePlay.Subscribe(value => OnTogglePlay());
            Forward.Subscribe(value => OnForward());
            LastFrame.Subscribe(value => OnLastFrame());

            eventService.Subscribe<VideoDisplayInfo>(value => OnCurrentTimeUpdated(value.CurrentTime));
            eventService.Subscribe<VideoDisplayInfo>(value => OnCurrentFrameUpdated(value.CurrentFrame));
            eventService.Subscribe<VideoDisplayInfo>(value => OnTotalFramesSet(value.TotalFrames));
            eventService.Subscribe<VideoDisplayInfo>(value => OnDurationSet(value.Duration));
        }

        private void OnFirstFrame() {
            _videoPlayback.SetCurrentFrame(0);
        }

        private void OnBackward() {
            _videoPlayback.DecreaseFrame();
        }

        private void OnTogglePlay() {
            _videoPlayback.TogglePlay();
        }

        private void OnForward() {
            _videoPlayback.IncreaseFrame();
        }

        private void OnLastFrame() {
            _videoPlayback.SetCurrentFrame(300);
        }

        private void OnCurrentTimeUpdated(float time) {
            CurrentTime.Value = time;
        }

        private void OnCurrentFrameUpdated(int frame) {
            CurrentFrame.Value = frame;
        }

        private void OnTotalFramesSet(int frame) {
            TotalFrames.Value = frame;
        }

        private void OnDurationSet(float time) {
            Duration.Value = time;
        }

    }
}