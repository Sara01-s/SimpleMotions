using System.Collections.Generic;
using System;
using System.Linq;

namespace SimpleMotions {

	public sealed class KeyframeStorage : IKeyframeStorage {

		private readonly Dictionary<Type, IKeyframeSpline<Component>> _allKeyframes;
		private readonly VideoData _videoData;

		public KeyframeStorage(KeyframesData keyframesData, VideoData videoData) {
			_allKeyframes = keyframesData.AllKeyframes;
			_videoData = videoData;
		}

		private IKeyframeSpline<T> RegisterComponentKeyframes<T>() where T : Component {
			var componentKeyframes = new KeyframeSpline<T>();

			_allKeyframes.Add(typeof(T), componentKeyframes);
			return componentKeyframes;
		}

		public IEnumerable<Type> GetKeyframeTypes() {
			return _allKeyframes.Keys;
		}

#nullable enable
		public IKeyframeSpline<Component>? GetKeyframeSplineOfType<T>() where T : Component {
			return _allKeyframes[typeof(T)];
		}
#nullable disable

		public bool TryGetAllKeyframesOfType<T>(out IKeyframeSpline<Component> componentKeyframes) where T : Component {
			var componentType = typeof(T);

			if (_allKeyframes.TryGetValue(componentType, out var keyframeSpline)) {
				componentKeyframes = keyframeSpline;
				return true;
			}

			componentKeyframes = null;
			return false;
		}

		public void AddKeyframe<T>(int entityId, IKeyframe<T> keyframe) where T : Component {
			AddKeyframe(entityId, keyframe.Frame, keyframe.Value);
		}

		public IKeyframe<Component> AddKeyframe<T>(int entityId, int frame, T value) where T : Component {
			IKeyframe<Component> keyframe = new Keyframe<Component>(entityId, frame, value);

			if (TryGetAllKeyframesOfType<T>(out IKeyframeSpline<Component> componentKeyframes)) {
				componentKeyframes.Add(frame, keyframe);
				return keyframe;
			}

			componentKeyframes = RegisterComponentKeyframes<T>();
			componentKeyframes.Add(frame, keyframe);

			return keyframe;
		}

		public IList<IKeyframe<Component>> GetEntityKeyframesOfType<T>(int entityId) where T : Component {
			if (!EntityHasKeyframesOfType<T>(entityId)) {
				throw new Exception("Entity has no keyframes of type " + typeof(T));
			}

			var entityKeyframes = GetEntityKeyframes(entityId);
			return entityKeyframes.Where(keyframe => keyframe.Value is T).ToList();
		}

		public IList<IKeyframe<Component>> GetEntityKeyframes(int entityId) {
			var allEntityKeyframes = new List<IKeyframe<Component>>();

			foreach (var keyframeSpline in _allKeyframes.Values) {
				allEntityKeyframes.AddRange(keyframeSpline.GetKeyframes());
			}

			return allEntityKeyframes.Where(keyframe => keyframe.EntityId == entityId).ToList();
		}

        public IKeyframe<Component> GetKeyframeAt<T>(int frame) where T : Component {
			if (TryGetAllKeyframesOfType<T>(out var componentKeyframes)) {
				if (componentKeyframes.TryGetValue(frame, out var keyframe)) {
					return keyframe;
				}
			}

			return Keyframe<Component>.Invalid;
        }

		public IKeyframe<Component> GetKeyframeAt(int frame) {
			foreach (var componentType in _allKeyframes.Keys) {
				var componentKeyframes = _allKeyframes[componentType];

				if (componentKeyframes.TryGetValue(frame, out var keyframe)) {
					return keyframe;
				}
			}

			return new Keyframe<Component>(Entity.INVALID_ID);
		}

		public bool EntityHasKeyframesOfAnyType(int entityId) {
			foreach (var componentType in _allKeyframes.Keys) {
				var componentKeyframes = _allKeyframes[componentType];

				if (componentKeyframes is null || componentKeyframes.Count <= 0) {
					continue;
				}
				
				return componentKeyframes[0].EntityId == entityId;
			}

			return false;
		}

		public bool EntityHasKeyframesOfType<T>(int entityId) where T : Component {
			if (!TryGetAllKeyframesOfType<T>(out var componentKeyframes)) {
				return false;
			}

			if (componentKeyframes.Count <= 0) {
				return false;
			}

			return componentKeyframes[0].EntityId == entityId;
		}

		public int GetTotalFrames() {
			return _videoData.TotalFrames;
		}

        public KeyframesData GetKeyframesData() {
			return new KeyframesData {
				AllKeyframes = _allKeyframes
			};
		}

	}
}