namespace SimpleMotions {

	internal sealed class VideoTimeline : IVideoTimeline {

		private readonly IVideoDatabase _videoDatabase;

		internal VideoTimeline(IVideoDatabase videoDatabase) {
			_videoDatabase = videoDatabase;
		}

		public float GetCurrentTime() {
			return _videoDatabase.CurrentTime;
		}

		public float GetDuration() {
			return _videoDatabase.Duration;
		}

		public void Pause() {
			_videoDatabase.IsPlaying = false;
		}

		public void Play() {
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