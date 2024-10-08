using System.Collections.Generic;
using System;

namespace SimpleMotions {

	public sealed class KeyframeStorage : IKeyframeStorage {

		private readonly SortedDictionary<int, Keyframe<Transform>> _transformKeyframes = new();
		private readonly SortedDictionary<int, Keyframe<Shape>> _shapeKeyframes = new();
		private readonly SortedDictionary<int, Keyframe<Text>> _textKeyframes = new();

		private readonly Dictionary<Type, SortedDictionary<int, Keyframe<Component>>> _allKeyframes = new();

		public KeyframeStorage(KeyframesData keyframesData) {
			_transformKeyframes = keyframesData.TransformKeyframes;
			_shapeKeyframes = keyframesData.ShapeKeyframes;
			_textKeyframes = keyframesData.TextKeyframes;
		}

		public void AddKeyframe(Entity entity, int frame, Position value) {
			var keyframe = new Keyframe<Transform>(entity.Id, frame, new Transform { Position = value });

			_transformKeyframes.Add(frame, keyframe);
		}

        public Keyframe<Transform> GetKeyframeAt(int frame) {
			if (_transformKeyframes.TryGetValue(frame, out var keyframe)) {
				return keyframe;
			}
			else {
				return new Keyframe<Transform>(-1, 0, new Transform());
			}
        }

        public KeyframesData GetKeyframesData() {
			return new KeyframesData {
				TransformKeyframes = _transformKeyframes,
				ShapeKeyframes = _shapeKeyframes,
				TextKeyframes = _textKeyframes
			};
		}

	}
}