namespace SimpleMotions {

    public sealed class VideoPlayback : IVideoPlayback {

        private readonly IVideoPlayer _videoPlayer;

        private bool _shouldPlay;

        public VideoPlayback(IVideoPlayer videoPlayer) {
            _videoPlayer = videoPlayer;
        }
        
        public void TogglePlay() {
            _shouldPlay = !_shouldPlay;

            if (_shouldPlay) {
                _videoPlayer.Play();
            }
            else {
                _videoPlayer.Pause();
            }
        }

        
		public void Play() {
			UnityEngine.Debug.Log("Play");
			_videoPlayer.Play();
		}

		public void Pause() {
			UnityEngine.Debug.Log("Pause");
			_videoPlayer.Pause();
		}

    }
}