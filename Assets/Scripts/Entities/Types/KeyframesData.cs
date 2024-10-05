using System.Collections.Generic;

namespace SimpleMotions {

	public sealed class KeyframesData {

		public HashSet<SortedSet<Keyframe<System.Type>>> AllKeyframes;
		
		public SortedSet<Keyframe<Position>> PositionKeyframes = new();
		public SortedSet<Keyframe<Scale>> ScaleKeyframes = new();
		public SortedSet<Keyframe<Shape>> ShapeKeyframes = new();
		public SortedSet<Keyframe<Roll>> RollKeyframes = new();
		public SortedSet<Keyframe<Text>> TextKeyframes = new();

	}
}