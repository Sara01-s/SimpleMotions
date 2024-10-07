namespace SimpleMotions {

    public sealed class VideoPlayback : IVideoPlayback {

        private readonly IVideoPlayer _videoPlayer;

        public VideoPlayback(IVideoPlayer videoPlayer) {
            _videoPlayer = videoPlayer;
        }
        
        public void TogglePlay() {
            _videoPlayer.TogglePlay();
        }
        
    }
}