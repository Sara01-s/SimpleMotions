namespace SimpleMotions {

	public interface IVideoPlayer {
		
		void TogglePlay();
		VideoData GetVideoData();
		void Play();
		void Pause();
		void Reset();
		void SetCurrentFrame(int frame);

	}
}