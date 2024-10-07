namespace SimpleMotions {

    public interface IVideoPlaybackViewModel {
        
        ReactiveCommand<Void> TogglePlay { get; set; }
        ReactiveValue<int> CurrentFrame { get; set; }
        ReactiveValue<int> TotalFrames { get; set; }
        ReactiveValue<float> CurrentTime { get; set; }
        ReactiveValue<float> Duration { get; set; }

    }
}