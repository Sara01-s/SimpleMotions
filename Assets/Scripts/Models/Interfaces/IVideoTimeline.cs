namespace SimpleMotions {

	public interface IVideoTimeline {
		
		void Play();
		void Pause();
		void Resume();
		void SetTime(float newCurrentTime);
		
	}
}