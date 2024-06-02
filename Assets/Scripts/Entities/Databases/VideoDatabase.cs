namespace SimpleMotions {

	internal sealed class VideoDatabase : IVideoDatabase {
		public float CurrentTime { get; set;  }
		public float Duration { get; set;  }
		public bool IsPlaying { get; set; }

		public void Dispose() {
			throw new System.NotImplementedException();
		}
	}

}