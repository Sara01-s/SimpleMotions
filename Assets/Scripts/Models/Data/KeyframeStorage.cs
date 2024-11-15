using System.Collections.Generic;
using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

	public interface IKeyframeStorage {

		IEnumerable<Type> KeyframeTypes { get; }
		int TotalFrames { get; }

#nullable enable
		IKeyframeSpline? GetKeyframeSplineOfType<T>() where T : Component;
		IKeyframeSpline? GetEntityKeyframesOfType<T>(int entityId) where T : Component;
#nullable disable

		IKeyframe<Component> GetKeyframeAt<T>(int frame) where T : Component;
		IKeyframe<Component> GetKeyframeAt(int frame);
		KeyframesData GetKeyframesData();

		void AddKeyframe<T>(IKeyframe<T> keyframe) where T : Component;
		void AddKeyframe<T>(int entityId, IKeyframe<T> keyframe) where T : Component;
		IKeyframe<Component> AddKeyframe<T>(int entityId, int frame, T value) where T : Component;
		void AddDefaultKeyframes(int entityId);

		void RemoveKeyframe<T>(int entityId, int frame) where T : Component;
		void ClearEntityKeyframes(int entityId);
	
		IEnumerable<IKeyframeSpline> GetEntityKeyframes(int entityId);
		IEnumerable<IKeyframe<Component>> GetAllKeyframesAt(int currentFrame);

		bool FrameHasKeyframe(int frame);
		bool EntityHasKeyframesOfType<T>(int entityId) where T : Component;
		bool TryGetAllKeyframesOfType<T>(out IKeyframeSpline keyframeSpline) where T : Component;
		bool EntityHasKeyframesOfAnyType(int entityId);
	}
	

	public sealed class KeyframeStorage : IKeyframeStorage {

		private readonly Dictionary<Type, IKeyframeSpline> _allKeyframes;
		private readonly VideoData _videoData;

		public IEnumerable<Type> KeyframeTypes => _allKeyframes.Keys;
		public int TotalFrames => _videoData.TotalFrames;

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

#nullable enable
		public IKeyframeSpline? GetKeyframeSplineOfType<T>() where T : Component {
			return _allKeyframes[typeof(T)];
		}
#nullable disable

		public bool TryGetAllKeyframesOfType<T>(out IKeyframeSpline componentKeyframeSpline) where T : Component {
			var componentType = typeof(T);

			if (_allKeyframes.TryGetValue(componentType, out var keyframeSpline)) {
				componentKeyframeSpline = keyframeSpline;
				return true;
			}

			componentKeyframeSpline = null;
			return false;
		}

		public void AddKeyframe<T>(IKeyframe<T> keyframe) where T : Component {
			AddKeyframe(keyframe.EntityId, keyframe.Frame, keyframe.Value);
		}

		public void AddKeyframe<T>(int entityId, IKeyframe<T> keyframe) where T : Component {
			AddKeyframe(entityId, keyframe.Frame, keyframe.Value);
		}

		public void AddDefaultKeyframes(int entityId) {
			AddKeyframe(entityId, new Keyframe<Transform>(entityId));
			AddKeyframe(entityId, new Keyframe<Shape>(entityId));
			AddKeyframe(entityId, new Keyframe<Text>(entityId));
		}

		public void ClearEntityKeyframes(int entityId) { // TODO - Measure a faster way to do this?
			for (int frame = TimelineData.FIRST_FRAME; frame <= TotalFrames; frame++) {
				UnityEngine.Debug.Log(GetKeyframeAt(frame));
				RemoveKeyframe<Transform>(entityId, frame);
				UnityEngine.Debug.Log(GetKeyframeAt(frame));
			}
		}

		public void RemoveKeyframe<T>(int entityId, int frame) where T : Component {
			if (!FrameHasKeyframe(frame)) {
				return;
			}

			if (GetKeyframeAt(frame).EntityId == entityId) {
				_allKeyframes[typeof(T)].Remove(frame);
			}
		}

		public IKeyframe<Component> AddKeyframe<T>(int entityId, int frame, T value) where T : Component {
			IKeyframe<Component> keyframe = new Keyframe<Component>(entityId, frame, value);

			if (TryGetAllKeyframesOfType<T>(out var componentKeyframeSpline)) {
				componentKeyframeSpline.Add(frame, keyframe);
				return keyframe;
			}

			componentKeyframeSpline = RegisterComponentKeyframes<T>();
			componentKeyframeSpline.Add(frame, keyframe);

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
			return GetKeyframeAt(frame).EntityId != Entity.Invalid.Id;
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

			return new Keyframe<Component>(Entity.Invalid.Id);
		}

		public bool EntityHasKeyframesOfAnyType(int entityId) {
			foreach (var componentType in KeyframeTypes) {
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

        public KeyframesData GetKeyframesData() {
			return new KeyframesData {
				AllKeyframes = _allKeyframes
			};
		}

	}
}