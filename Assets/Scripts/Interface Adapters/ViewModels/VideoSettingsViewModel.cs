
namespace SimpleMotions {

    public interface IVideoSettingsViewModel {
        ReactiveCommand<int> OnFramerateUpdate { get; }
    }

    public class VideoSettingsViewModel : IVideoSettingsViewModel {

        public ReactiveCommand<int> OnFramerateUpdate { get; } = new();

        public VideoSettingsViewModel() {

        }

    }

}