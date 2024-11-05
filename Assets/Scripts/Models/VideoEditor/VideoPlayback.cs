
namespace SimpleMotions {

	public interface IVideoPlayback {
        
        void TogglePlay();
		void SetCurrentFrame(int frame);
        void IncreaseFrame();
        void DecreaseFrame();
        int SetFirstFrame();
        int SetLastFrame();

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

        public int SetFirstFrame() {
            int frame = _videoPlayer.GetFirstFrame();

            SetCurrentFrame(frame);

            return frame;
        }

        public int SetLastFrame() {
            int frame = _videoPlayer.GetLastFrame();
        
            SetCurrentFrame(frame);

            return frame;
        }

    }
}