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
		public readonly int InvalidFrame = -1;
		public readonly int FirstFrame = 0;

		public KeyframesData Keyframes = new();
		public EntitiesData Entities = new();
		public ComponentsData Components = new();
	}

	[Serializable]
	public sealed class VideoData {
		public int TargetFrameRate {
			get => _targetFrameRate;
			set {
				if (value <= 0) {
					throw new ArgumentException($"Target frame rate cannot be less or equal to 0. {value}");
				}

				_targetFrameRate = value;
			}
		}
		
		public bool IsLooping = false;
		public bool IsPlaying = false;
		public float CurrentTime;
		public float DurationSeconds => TotalFrames / TargetFrameRate;

		public int CurrentFrame;
		public int TotalFrames;

		public Color CanvasBackgroundColor = new();
		public Scale Resolution = new();

		private int _targetFrameRate;

		public VideoData() {}

		public VideoData(int firstFrame, int targetFrameRate) {
			CurrentTime = firstFrame;
			CurrentFrame = firstFrame;
			TotalFrames = 100;
			TargetFrameRate = targetFrameRate;
		}
	}

}