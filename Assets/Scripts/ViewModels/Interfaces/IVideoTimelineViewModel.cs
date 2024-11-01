
namespace SimpleMotions {

    public interface IVideoTimelineViewModel {
		ReactiveCommand<Void> OnCreateTestEntity { get; }
		ReactiveCommand<int> OnSetCurrentFrame { get; }
		ReactiveCommand<VideoDisplayInfo> OnTimelineUpdate { get; }
	}
}