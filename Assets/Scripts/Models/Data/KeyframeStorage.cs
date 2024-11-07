using System.Collections.Generic;
using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

	public interface IKeyframeStorage {

#nullable enable
		IKeyframeSpline? GetKeyframeSplineOfType<T>() where T : Component;
		IKeyframeSpline? GetEntityKeyframesOfType<T>(int entityId) where T : Component;
#nullable disable

		IKeyframe<Component> GetKeyframeAt<T>(int frame) where T : Component;
		IKeyframe<Component> GetKeyframeAt(int frame);
		KeyframesData GetKeyframesData();
		void AddKeyframe<T>(int entityId, IKeyframe<T> keyframe) where T : Component;
		IKeyframe<Component> AddKeyframe<T>(int entityId, int frame, T value) where T : Component;
		IEnumerable<IKeyframeSpline> GetEntityKeyframes(int entityId);

		IEnumerable<Type> GetKeyframeTypes();

		bool FrameHasKeyframe(int frame);
		bool EntityHasKeyframesOfType<T>(int entityId) where T : Component;
		bool TryGetAllKeyframesOfType<T>(out IKeyframeSpline keyframeSpline) where T : Component;
		bool EntityHasKeyframesOfAnyType(int entityId);

		int GetTotalFrames();
		IEnumerable<IKeyframe<Component>> GetAllKeyframesAt(int currentFrame);
	}
	

	public sealed class KeyframeStorage : IKeyframeStorage {

		private readonly Dictionary<Type, IKeyframeSpline> _allKeyframes;
		private readonly VideoData _videoData;

		public KeyframeStorage(KeyframesData keyframesData, VideoData videoData) {
			_allKeyframes = keyframesData.AllKeyframes;
			_videoData = videoData;
		}

		private IKeyframeSpline RegisterComponentKeyframes<T>() where T : Component {
			var componentKeyframes = new KeyframeSpline();

			_allKeyframes.Add(typeof(T), componentKeyframes);
			return componentKeyframes;
		}

		public IEnumerable<IKeyframe<Component>> GetAllKeyframesAt(int frame) {
			var keyframesAtFrame = new List<IKeyframe<Component>>();

			foreach (var componentType in _allKeyframes.Keys) {
				if (_allKeyframes[componentType].TryGetValue(frame, out var keyframe)) {
					keyframesAtFrame.Add(keyframe);
				}
			}

			if (keyframesAtFrame.Count <= 0) {
				keyframesAtFrame.Add(Keyframe<Component>.Invalid);
			}

			return keyframesAtFrame;
		}

		public IEnumerable<Type> GetKeyframeTypes() {
			return _allKeyframes.Keys;
		}

#nullable enable
		public IKeyframeSpline? GetKeyframeSplineOfType<T>() where T : Component {
			return _allKeyframes[typeof(T)];
		}
#nullable disable

		public bool TryGetAllKeyframesOfType<T>(out IKeyframeSpline componentKeyframes) where T : Component {
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

			if (TryGetAllKeyframesOfType<T>(out var componentKeyframes)) {
				componentKeyframes.Add(frame, keyframe);
				return keyframe;
			}

			componentKeyframes = RegisterComponentKeyframes<T>();
			componentKeyframes.Add(frame, keyframe);

			return keyframe;
		}

#nullable enable
		public IKeyframeSpline? GetEntityKeyframesOfType<T>(int entityId) where T : Component {
			if (!EntityHasKeyframesOfType<T>(entityId)) {
				throw new Exception("Entity has no keyframes of type " + typeof(T));
			}
			
			var entityKeyframeSplines = GetEntityKeyframes(entityId);

			foreach (var spline in entityKeyframeSplines) {
				if (spline[0].Value is T) {
					return spline;
				}
			}

			return null;
		}
#nullable disable

		public IEnumerable<IKeyframeSpline> GetEntityKeyframes(int entityId) {
			var allEntityKeyframes = new List<IKeyframeSpline>(_allKeyframes.Keys.Count);

			foreach (var keyframeSpline in _allKeyframes.Values) {
				allEntityKeyframes.Add(keyframeSpline.GetEntityKeyframes(entityId));
			}

			return allEntityKeyframes;
		}

		public bool FrameHasKeyframe(int frame) {
			return GetKeyframeAt(frame).EntityId != Entity.INVALID_ID;
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