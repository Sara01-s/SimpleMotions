
namespace SimpleMotions {

	public interface IVideoPlayback {
        
        void TogglePlay();
		void SetCurrentFrame(int frame);
        void IncreaseFrame();
        void DecreaseFrame();
        void SetFirstFrame();
        void SetLastFrame();

    }

    public sealed class VideoPlayback : IVideoPlayback {

        private readonly IVideoPlayer _videoPlayer;

		public VideoPlayback(IVideoPlayer videoPlayer) {
            _videoPlayer = videoPlayer;
        }

		public void TogglePlay() {
            _videoPlayer.TogglePlay();
        }

        public void IncreaseFrame() {
            _videoPlayer.IncreaseFrame();
        }

        public void DecreaseFrame() {
            _videoPlayer.DecreaseFrame();
        }

        public void SetFirstFrame() {
            SetCurrentFrame(_videoPlayer.FirstFrame);
        }

        public void SetLastFrame() {
            SetCurrentFrame(_videoPlayer.LastFrame);
        }

        public void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
		}

    }
}