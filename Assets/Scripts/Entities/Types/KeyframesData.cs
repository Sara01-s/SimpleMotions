using System.Collections.Generic;

namespace SimpleMotions {

	public sealed class KeyframesData {

		public SortedDictionary<int, Keyframe<Transform>> TransformKeyframes = new();
		public SortedDictionary<int, Keyframe<Shape>> ShapeKeyframes = new();
		public SortedDictionary<int, Keyframe<Text>> TextKeyframes = new();

	}
}