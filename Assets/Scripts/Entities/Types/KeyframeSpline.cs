using System.Collections;
using System.Collections.Generic;

namespace SimpleMotions {

    public interface IKeyframeSpline<out T> where T : Component {
		
		public IKeyframe<T> this[int frame] { get; }
		public int Count { get; }
		void Add<TComponent>(int frame, IKeyframe<TComponent> keyframe) where TComponent : Component;
		IEnumerable<IKeyframe<T>> GetKeyframes();
		bool TryGetValue<TComponent>(int frame, out Keyframe<TComponent> keyframe) where TComponent : Component;
	}

	[System.Serializable]
    public class KeyframeSpline<T> : IKeyframeSpline<T> where T : Component {

		public IKeyframe<T> this[int frame] => _keyframes[frame];
        private readonly SortedDictionary<int, IKeyframe<T>> _keyframes = new();
		public IEnumerable<IKeyframe<T>> Keyframes => _keyframes.Values;
		public int Count => _keyframes.Values.Count;

		public KeyframeSpline() {}

		public IEnumerable<IKeyframe<T>> GetKeyframes() {
            return _keyframes.Values;
        }

		public void Add<TComponent>(int frame, IKeyframe<TComponent> keyframe) where TComponent : Component {
			_keyframes.Add(frame, keyframe as IKeyframe<T>);
		}

		public bool TryGetValue<TComponent>(int frame, out Keyframe<TComponent> keyframe) where TComponent : Component {
			bool result = _keyframes.TryGetValue(frame, out var storedKeyframe);

			keyframe = storedKeyframe as Keyframe<TComponent>;
			return result;
		}

	}
}
