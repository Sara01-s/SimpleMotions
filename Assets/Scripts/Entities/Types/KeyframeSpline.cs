using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace SimpleMotions.Internal {

    public interface IKeyframeSpline<T> : IEnumerable<IKeyframe<T>> where T : Component, new() {
		
		IKeyframe<T>[] Keyframes { get; }
		int[] KeyframeIndices { get; }
		int Count { get; }
		IKeyframe<T> this[int frame] { get; }
		
		void AddKeyframe(int frame, IKeyframe<T> keyframe);
		void AddRange(IKeyframeSpline<T> keyframeSpline);
		void RemoveKeyframe(int frame);
		void RemoveRange(IKeyframeSpline<T> keyframeSpline);
		bool TryGetValue(int frame, out IKeyframe<T> keyframe);
		IKeyframe<T> GetPreviousKeyframe(int frame);
		IKeyframe<T> GetNextKeyframe(int frame);

		IKeyframeSpline<T> GetEntityKeyframes(int entityId);

		bool HasKeyframeAtFrame(int frame);
		IKeyframeSpline<TComponent> As<TComponent>() where TComponent : Component, new();
	}

	[Serializable]
    public class KeyframeSpline<T> : IKeyframeSpline<T> where T : Component, new() {

		public IKeyframe<T>[] Keyframes => _keyframes.Values.ToArray();
		public int[] KeyframeIndices => _keyframes.Keys.ToArray();
		
		public IKeyframe<T> this[int frame] => _keyframes[frame];
		public int Count => _keyframes.Values.Count;

		// 					 			frame -> IKeyframe<Component>
		private readonly SortedDictionary<int, IKeyframe<T>> _keyframes = new();

		public KeyframeSpline() {}

		public KeyframeSpline(IKeyframeSpline<T> keyframeSpline) {
			AddRange(keyframeSpline);
		}

		public IKeyframeSpline<TComponent> As<TComponent>() where TComponent : Component, new() {
			var keyframeSpline = new KeyframeSpline<TComponent>();

			foreach (var keyframe in this) {
				if (keyframe.Value is TComponent castedValue) {
					var castedKeyframe = new Keyframe<TComponent>(keyframe.EntityId, keyframe.Frame, castedValue);
					keyframeSpline.AddKeyframe(keyframe.Frame, castedKeyframe);
				}
				else {
					throw new Exception("Failed to cast keyframe spline to: " + typeof(TComponent));
				}
			}

			return keyframeSpline;
		}

		public IKeyframeSpline<T> GetEntityKeyframes(int entityId) {
			var keyframeSpline = new KeyframeSpline<T>();

			foreach (var keyframe in this) {
				if (entityId == keyframe.EntityId) {
					keyframeSpline.AddKeyframe(keyframe.Frame, keyframe);
				}
			}

			return keyframeSpline;
		}

		public IKeyframe<T> GetPreviousKeyframe(int frame) {
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

		public IKeyframe<T> GetNextKeyframe(int frame) {
			var allFramesWithKeyframes = _keyframes.Keys.ToArray();
			
			foreach (var keyframe in allFramesWithKeyframes) {
				if (keyframe > frame) {
					return _keyframes[keyframe];
				}
			}
        
			return _keyframes.Last().Value;
		}

		public bool HasKeyframeAtFrame(int frame) {
			return _keyframes.ContainsKey(frame);
		}

		public void AddKeyframe(int frame, IKeyframe<T> keyframe) {
			_keyframes[frame] = keyframe;
		}

		public void RemoveKeyframe(int frame) {
			_keyframes[frame] = Keyframe<T>.Invalid;
		}

		public void AddRange(IKeyframeSpline<T> keyframeSpline) {
			if (keyframeSpline.KeyframeIndices.Length != keyframeSpline.Keyframes.Length) {
				throw new Exception("Keyframe Spline with different number of indices and keyframes.");
			}

			for (int i = 0; i < keyframeSpline.Count; i++) {
				var keyframe = keyframeSpline.Keyframes[i];
				int frame = keyframeSpline.KeyframeIndices[i];

				AddKeyframe(frame, keyframe);
			}
		}

		public void RemoveRange(IKeyframeSpline<T> keyframeSpline) {
			if (keyframeSpline.KeyframeIndices.Length != keyframeSpline.Keyframes.Length) {
				throw new Exception("Keyframe Spline with different number of indices and keyframes.");
			}

			for (int i = 0; i < keyframeSpline.Count; i++) {
				int frame = keyframeSpline.KeyframeIndices[i];
				RemoveKeyframe(frame);
			}
		}

		public bool TryGetValue(int frame, out IKeyframe<T> keyframe) {
			bool result = _keyframes.TryGetValue(frame, out var storedKeyframe);

			keyframe = storedKeyframe;
			return result;
		}

		public IEnumerator<IKeyframe<T>> GetEnumerator() {
			return _keyframes.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

	}
}
