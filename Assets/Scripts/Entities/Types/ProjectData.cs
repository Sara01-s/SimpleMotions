using System;

namespace SimpleMotions {

	[Serializable]
	public sealed class ProjectData {
		public string ProjectName = "untitled_project";
		public TimelineData Timeline = new();
		public VideoData Video = new();
		public PlaybackData Playback = new();
	}

	[Serializable]
	public sealed class TimelineData {
		public KeyframesData Keyframes = new();
		public EntitiesData Entities = new();
		public ComponentsData Components = new();
	}

	[Serializable]
	public sealed class VideoData {
		public bool IsLooping = true;
		public bool IsPlaying;
		public float CurrentTime;
		public float Duration; // In seconds

		public int TargetFrameRate;
		public int CurrentFrame;
		public int TotalFrames = 300;

		public Color CanvasBackgroundColor = new();
		public Scale Resolution = new();
	}

	[Serializable]
	public sealed class PlaybackData {
		public int CurrentFrame;
		public bool IsLooping;
	}
	
}