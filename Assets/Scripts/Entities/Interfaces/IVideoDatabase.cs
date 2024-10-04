namespace SimpleMotions {

	public interface IVideoDatabase {

		float CurrentTime { get; set; }
		float Duration { get; set; }
		float NormalizedTime => CurrentTime / Duration;
		bool IsPlaying { get; set; }

	}
}