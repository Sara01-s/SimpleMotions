namespace SimpleMotions {

	public sealed class VideoTimeline : IVideoTimeline {

		private readonly IVideoDatabase _videoDatabase;

		public VideoTimeline(IVideoDatabase videoDatabase) {
			_videoDatabase = videoDatabase;
		}

		public float GetCurrentTime() {
			return _videoDatabase.CurrentTime;
		}

		public float GetDuration() {
			return _videoDatabase.Duration;
		}

		public void Pause() {
			UnityEngine.Debug.Log("Pause");
			_videoDatabase.IsPlaying = false;
		}

		public void Play() {
			UnityEngine.Debug.Log("Play");
			_videoDatabase.IsPlaying = true;
		}

		public void Resume() {
			Play();
		}

		public void SetTime(float newCurrentTime) {
			_videoDatabase.CurrentTime = newCurrentTime;
		}
	}

}