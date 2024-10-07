namespace SimpleMotions {

	public interface IVideoPlayer {

		VideoData GetVideoData();
		void Play();
		void Resume();
		void Pause();
		void SetCurrentTime(float seconds);

	}
}