using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace SimpleMotions.Internal {

    public interface IKeyframeSpline : IEnumerable<IKeyframe<Component>> {
		
		IKeyframe<Component>[] Keyframes { get; }
		int[] KeyframeIndices { get; }
		int Count { get; }
		IKeyframe<Component> this[int frame] { get; }
		
		void Add(int frame, IKeyframe<Component> keyframe);
		void AddRange(IKeyframeSpline keyframeSpline);
		void Remove(int frame);
		void RemoveRange(IKeyframeSpline keyframeSpline);
		bool TryGetValue(int frame, out IKeyframe<Component> keyframe);
		IKeyframe<Component> GetLastKeyframe(int frame);
		IKeyframe<Component> GetNextKeyframe(int frame);

		IKeyframeSpline GetEntityKeyframes(int entityId);
		IKeyframeSpline GetKeyframesOfComponent<T>() where T : Component;

	}

	[Serializable]
    public class KeyframeSpline : IKeyframeSpline {

		public IKeyframe<Component>[] Keyframes => _keyframes.Values.ToArray();
		public int[] KeyframeIndices => _keyframes.Keys.ToArray();
		
		public IKeyframe<Component> this[int frame] => _keyframes[frame];
		public int Count => _keyframes.Values.Count;

		// 					 			frame -> IKeyframe<Component>
		private readonly SortedDictionary<int, IKeyframe<Component>> _keyframes = new();

		public KeyframeSpline() {}

		public KeyframeSpline(IKeyframeSpline keyframeSpline) {
			AddRange(keyframeSpline);
		}

		public IKeyframeSpline GetEntityKeyframes(int entityId) {
			var keyframeSpline = new KeyframeSpline();

			foreach (var keyframe in this) {
				if (entityId == keyframe.EntityId) {
					keyframeSpline.Add(keyframe.Frame, keyframe);
				}
			}

			return keyframeSpline;
		}

		public IKeyframeSpline GetKeyframesOfComponent<T>() where T : Component {
			var keyframeSpline = new KeyframeSpline();

			foreach (var keyframe in this) {
				if (keyframe.Value is T) {
					keyframeSpline.Add(keyframe.Frame, keyframe);
				}
			}

			return keyframeSpline;
		}

		public IKeyframe<Component> GetLastKeyframe(int frame) {
			var allFramesWithKeyframes = _keyframes.Keys.ToArray();
			var selectedKeyframeFrame = allFramesWithKeyframes[0];

			foreach (int keyframeFrame in allFramesWithKeyframes) {
				if (keyframeFrame <= frame) {
					selectedKeyframeFrame = keyframeFrame;
				}
				else {
					break;
				}
			}

			return _keyframes[selectedKeyframeFrame];
		}

		public IKeyframe<Component> GetNextKeyframe(int frame) {
			var allFramesWithKeyframes = _keyframes.Keys.ToArray();
			
			foreach (var keyframe in allFramesWithKeyframes) {
				if (keyframe > frame) {
					return _keyframes[keyframe];
				}
			}
        
			return _keyframes.Last().Value;
		}

		public void Add(int frame, IKeyframe<Component> keyframe) {
			_keyframes[frame] = keyframe;
		}

		public void Remove(int frame) {
			_keyframes[frame] = null;
		}

		public void AddRange(IKeyframeSpline keyframeSpline) {
			if (keyframeSpline.KeyframeIndices.Length != keyframeSpline.Keyframes.Length) {
				throw new Exception("Keyframe Spline with different number of indices and keyframes.");
			}

			for (int i = 0; i < keyframeSpline.Count; i++) {
				var keyframe = keyframeSpline.Keyframes[i];
				int frame = keyframeSpline.KeyframeIndices[i];

				Add(frame, keyframe);
			}
		}

		public void RemoveRange(IKeyframeSpline keyframeSpline) {
			if (keyframeSpline.KeyframeIndices.Length != keyframeSpline.Keyframes.Length) {
				throw new Exception("Keyframe Spline with different number of indices and keyframes.");
			}

			for (int i = 0; i < keyframeSpline.Count; i++) {
				int frame = keyframeSpline.KeyframeIndices[i];
				Remove(frame);
			}
		}

		public bool TryGetValue(int frame, out IKeyframe<Component> keyframe) {
			bool result = _keyframes.TryGetValue(frame, out var storedKeyframe);

			keyframe = storedKeyframe;
			return result;
		}

		public IEnumerator<IKeyframe<Component>> GetEnumerator() {
			return _keyframes.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

	}
}
