using System;

namespace SimpleMotions.Internal {

	[Serializable]
	public sealed class ProjectData {
		public string ProjectName = "untitled_project";
		public TimelineData Timeline = new();
		public VideoData Video = new();
	}

	[Serializable]
	public sealed class TimelineData {
		public const int MAX_ALLOWED_ENTITIES = 50;
		public const int INVALID_FRAME = -1;
		public const int FIRST_FRAME = 0;

		public KeyframesData Keyframes = new();
		public EntitiesData Entities = new();
		public ComponentsData Components = new();
	}

	[Serializable]
	public sealed class VideoData {
		public int TargetFrameRate {
			get => _targetFrameRate;
			set {
				if (value <= TimelineData.FIRST_FRAME) {
					throw new ArgumentException($"Target frame rate cannot be less or equal to {TimelineData.FIRST_FRAME}");
				}

				_targetFrameRate = value;
			}
		}
		
		public bool IsLooping = true;
		public bool IsPlaying = false;
		public float CurrentTime = 0;
		public float DurationSeconds => TotalFrames / TargetFrameRate;

		public int CurrentFrame = TimelineData.FIRST_FRAME;
		public int TotalFrames;

		public Color CanvasBackgroundColor = new();
		public Scale Resolution = new();

		private int _targetFrameRate = 1;

		public VideoData() {}

		public VideoData(int targetFrameRate, int totalFrames) {
			TargetFrameRate = targetFrameRate;
			TotalFrames = totalFrames;
		}
	}

}