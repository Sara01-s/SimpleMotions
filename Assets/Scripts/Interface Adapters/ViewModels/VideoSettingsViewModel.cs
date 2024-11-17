
namespace SimpleMotions {

    public interface IVideoSettingsViewModel {
        ReactiveCommand<string> OnFramerateUpdate { get; }
        string Framerate { get; }
    }

    public class VideoSettingsViewModel : IVideoSettingsViewModel {

        public ReactiveCommand<string> OnFramerateUpdate { get; } = new();
        public string Framerate { get; }

        private readonly IVideoPlayerData _videoPlayer;

        public VideoSettingsViewModel(IVideoPlayerData videoPlayer) {
            _videoPlayer = videoPlayer;

            Framerate = _videoPlayer.TargetFrameRate.Value.ToString();
            OnFramerateUpdate.Subscribe(UpdateFramerate);
        }

        private void UpdateFramerate(string framerateText) {
            if (int.TryParse(framerateText, out var framerate)) {
                _videoPlayer.TargetFrameRate.Value = framerate;
            }
        }

    }
}