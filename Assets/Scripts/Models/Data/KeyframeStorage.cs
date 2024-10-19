using System.Collections.Generic;
using System;

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

		public IKeyframeSpline<Component> GetAllKeyframesOfType<T>() where T : Component {
			var componentType = typeof(T);

			if (_allKeyframes.TryGetValue(componentType, out var componentKeyframes)) {
				return componentKeyframes;
			}

			return RegisterComponentKeyframes<T>();
		}


		public void AddKeyframe<T>(int entityId, int frame, T value) where T : Component {
			IKeyframe<T> keyframe = new Keyframe<T>(entityId, frame, value);
			var componentKeyframes = GetAllKeyframesOfType<T>();

			UnityEngine.Debug.Log(componentKeyframes);

			componentKeyframes.Add(frame, keyframe);
		}

        public Keyframe<T> GetKeyframeAt<T>(int frame) where T : Component {
			var componentKeyframes = GetAllKeyframesOfType<T>();

			if (componentKeyframes.TryGetValue<T>(frame, out var keyframe)) {
				return keyframe;
			}

			return Keyframe<T>.Empty;
        }

		public bool EntityHasKeyframes<T>(int entityId) where T : Component {
			var componentKeyframes = GetAllKeyframesOfType<T>();

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