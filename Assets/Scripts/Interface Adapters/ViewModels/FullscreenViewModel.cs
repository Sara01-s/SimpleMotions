
namespace SimpleMotions {

    public interface IFullscreenViewModel {
        ReactiveCommand OnFullscreen { get; }
    }

    public class FullscreenViewModel : IFullscreenViewModel {

        public ReactiveCommand OnFullscreen { get; } = new();

    }
}