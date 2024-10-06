using System.Collections.Generic;

namespace SimpleMotions {

	public sealed class KeyframeStorage : IKeyframeStorage {

		private readonly HashSet<SortedSet<Keyframe<System.Type>>> _allKeyframes;

		private readonly SortedSet<Keyframe<Position>> _positionKeyframes = new();
		private readonly SortedSet<Keyframe<Scale>> _scaleKeyframes = new();
		private readonly SortedSet<Keyframe<Shape>> _shapeKeyframes = new();
		private readonly SortedSet<Keyframe<Roll>> _rollKeyframes = new();
		private readonly SortedSet<Keyframe<Text>> _textKeyframes = new();

		public KeyframeStorage(KeyframesData keyframesData) {
			_allKeyframes = keyframesData.AllKeyframes;
			
			_positionKeyframes = keyframesData.PositionKeyframes;
			_scaleKeyframes = keyframesData.ScaleKeyframes;
			_shapeKeyframes = keyframesData.ShapeKeyframes;
			_rollKeyframes = keyframesData.RollKeyframes;
			_textKeyframes = keyframesData.TextKeyframes;
		}

		public void AddKeyframe(Entity entity, float time, Position value) {
			var keyframe = new Keyframe<Position>(entity.Id, time, value);

			_positionKeyframes.Add(keyframe);
		}

		public KeyframesData GetKeyframesData() {
			return new KeyframesData {
				AllKeyframes = _allKeyframes,

				PositionKeyframes = _positionKeyframes,
				ScaleKeyframes = _scaleKeyframes,
				ShapeKeyframes = _shapeKeyframes,
				RollKeyframes = _rollKeyframes,
				TextKeyframes = _textKeyframes
			};
		}

	}
}