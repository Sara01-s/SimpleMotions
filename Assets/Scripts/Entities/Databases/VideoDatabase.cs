namespace SimpleMotions {

	internal sealed class VideoDatabase : IVideoDatabase {
		public float CurrentTime { get; set; }
		public float Duration { get; set; } = 10.0f;
		public bool IsPlaying { get; set; }

		internal VideoDatabase() {
			Services.Instance.RegisterService<IVideoDatabase>(service: this);
		}

		public void Dispose() {
			Services.Instance.UnRegisterService<IVideoDatabase>();
		}
	}

}