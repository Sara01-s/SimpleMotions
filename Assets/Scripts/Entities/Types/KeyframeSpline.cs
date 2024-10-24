using System.Collections.Generic;

namespace SimpleMotions {

    public interface IKeyframeSpline<out T> where T : Component {
		
		void Add(int frame, IKeyframe<Component> keyframe);
		IEnumerable<IKeyframe<Component>> GetKeyframes();
		bool TryGetValue(int frame, out IKeyframe<Component> keyframe);

		int Count { get; }
		IKeyframe<Component> this[int frame] { get; }

	}

	[System.Serializable]
    public class KeyframeSpline<T> : IKeyframeSpline<T> where T : Component {

		// 					 frame -> IKeyframe<Component>
		public SortedDictionary<int, IKeyframe<Component>> Keyframes => _keyframes;
		public IKeyframe<Component> this[int frame] => _keyframes[frame];
		public int Count => _keyframes.Values.Count;

		private readonly SortedDictionary<int, IKeyframe<Component>> _keyframes = new();

		public KeyframeSpline() {}

		public IEnumerable<IKeyframe<Component>> GetKeyframes() {
            return _keyframes.Values;
        }

		public void Add(int frame, IKeyframe<Component> keyframe) {
			_keyframes[frame] = keyframe;
		}

		public bool TryGetValue(int frame, out IKeyframe<Component> keyframe) {
			bool result = _keyframes.TryGetValue(frame, out var storedKeyframe);

			keyframe = storedKeyframe;
			return result;
		}

	}
}
