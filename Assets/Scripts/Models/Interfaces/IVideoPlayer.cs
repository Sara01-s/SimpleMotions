namespace SimpleMotions {

	public interface IVideoPlayer {
		
		void TogglePlay();
		VideoData GetVideoData();
		void Play();
		void Pause();
		void Reset();
		void ResetAndPlay();
		void StopAndReset();
		void SetCurrentTime(float seconds);

	}
}