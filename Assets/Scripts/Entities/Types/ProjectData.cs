using System;

namespace SimpleMotions {

	[Serializable]
	public sealed class ProjectData {
		public string ProjectName = "untitled_project";
		public TimelineData Timeline = new();
		public VideoData Video = new();
	}

	[Serializable]
	public sealed class TimelineData {
		// TODO - VER QUE HACER
		public readonly int InvalidKeyframe = -1;
		// TODO - VER QUE HACER
		public readonly int FirstKeyframe = 0;
		public readonly int DefaultKeyframes = 100;

		public KeyframesData Keyframes = new();
		public EntitiesData Entities = new();
		public ComponentsData Components = new();
	}

	[Serializable]
	public sealed class VideoData {
		
		public bool IsLooping = false;
		public bool IsPlaying = false;
		public float CurrentTime = 0.0f;
		public float Duration; // In seconds

		public int TargetFrameRate = 60;
		// TODO - VER QUE HACER
		public int CurrentFrame = 0;
		// TODO - VER QUE HACER
		public int TotalFrames = 300;

		public Color CanvasBackgroundColor = new();
		public Scale Resolution = new();
	}

}