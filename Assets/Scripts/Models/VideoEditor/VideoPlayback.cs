using Unity.VisualScripting;

namespace SimpleMotions {

	public interface IVideoPlayback {
        
        void TogglePlay();
		void SetCurrentFrame(int frame);
        void IncreaseFrame();
        void DecreaseFrame();
        int GetFirstFrame();
        int GetLastFrame();

    }

    public sealed class VideoPlayback : IVideoPlayback {

        private readonly IVideoPlayer _videoPlayer;

        public VideoPlayback(IVideoPlayer videoPlayer) {
            _videoPlayer = videoPlayer;
        }

		public void SetCurrentFrame(int frame) {
			_videoPlayer.SetCurrentFrame(frame);
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

        public int GetFirstFrame() {
            return _videoPlayer.GetFirstFrame();
        }

        public int GetLastFrame() {
            return _videoPlayer.GetLastFrame();
        }

    }
}