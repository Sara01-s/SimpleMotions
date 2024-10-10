using System.Collections.Generic;
using System;

namespace SimpleMotions {

	public sealed class KeyframeStorage : IKeyframeStorage {

		private readonly SortedDictionary<int, Keyframe<Transform>> _transformKeyframes = new();
		private readonly SortedDictionary<int, Keyframe<Shape>> _shapeKeyframes = new();
		private readonly SortedDictionary<int, Keyframe<Text>> _textKeyframes = new();

		private readonly Dictionary<Type, SortedDictionary<int, Keyframe<Component>>> _allKeyframes = new();
		private readonly VideoData _videoData;

		public KeyframeStorage(KeyframesData keyframesData, VideoData videoData) {
			_transformKeyframes = keyframesData.TransformKeyframes;
			_shapeKeyframes = keyframesData.ShapeKeyframes;
			_textKeyframes = keyframesData.TextKeyframes;

			_videoData = videoData;
		}

		public void AddKeyframe(Entity entity, int frame, Position value) {
			var keyframe = new Keyframe<Transform>(entity.Id, frame, new Transform { Position = value });

			if (!_transformKeyframes.TryAdd(frame, keyframe)) {
				return;
			}
		}

        public Keyframe<Transform> GetKeyframeAt(int frame) {
			if (_transformKeyframes.TryGetValue(frame, out var keyframe)) {
				return keyframe;
			}

			return Keyframe<Transform>.Empty;
        }

		public int GetTotalFrames() {
			return _videoData.TotalFrames;
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