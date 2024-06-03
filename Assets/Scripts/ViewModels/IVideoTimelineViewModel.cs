
namespace SimpleMotions {

    public interface IVideoTimelineViewModel {

        ReactiveCommand<Void> TogglePlay { get; set; }
        ReactiveValue<float> CurrentTime { get; set; } 
        ReactiveValue<float> Duration { get; set; } 

    }
}