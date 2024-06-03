namespace SimpleMotions {

	internal interface IVideoTimeline {
		
		void Play();
		void Pause();
		void Resume();
		void SetTime(float newCurrentTime);
		float GetCurrentTime();
		float GetDuration();
		
	}
}