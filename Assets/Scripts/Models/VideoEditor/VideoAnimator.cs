using static SimpleMotions.SmMath;
using System.Collections.Generic;
using SimpleMotions.Internal;
using System.Linq;
using System;

namespace SimpleMotions {

    public interface IVideoAnimator {
        
        void GenerateVideoCache();
        void InterpolateAllEntities(int currentFrame);
		ReactiveValue<(int, string)> EntityDisplayInfo { get; }

    }

	public sealed class VideoAnimator : IVideoAnimator {
		
		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IComponentStorage _componentStorage;
		private readonly IEntityStorage _entityStorage;
		private readonly IEnumerable<Type> _componentTypes;

		private IEnumerable<int> _entitiesWithKeyframes;
		private IEnumerable<int> _lastEntitiesWithKeyframes;
		private bool _videoCacheGenerated;

		private IKeyframeSpline _currentComponentSpline;
		private Component _currentInterpolatedComponent;

		public ReactiveValue<(int, string)> EntityDisplayInfo { get; } = new();
		
		public VideoAnimator(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, 
							 IEntityStorage entityStorage) {
			_keyframeStorage = keyframeStorage;
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;

			_componentTypes = _keyframeStorage.GetKeyframeTypes();
		}

		public void GenerateVideoCache() {
			bool cacheAlreadyGenerated = _lastEntitiesWithKeyframes != null && _entitiesWithKeyframes != _lastEntitiesWithKeyframes;
			if (cacheAlreadyGenerated) {
				return;
			}

			_videoCacheGenerated = false;

			var activeEntities = _entityStorage.GetActiveEntities();
			_entitiesWithKeyframes = activeEntities.Where(entityId => _keyframeStorage.EntityHasKeyframesOfAnyType(entityId));

			_lastEntitiesWithKeyframes = _entitiesWithKeyframes;
			_videoCacheGenerated = true;
		}

		public void InterpolateAllEntities(int currentFrame) {
			if (!_videoCacheGenerated) {
				throw new Exception("Generate video cache before entity interpolation.");
			}

			foreach (int entityId in _entitiesWithKeyframes) {
				InterpolateEntityKeyframes(entityId, currentFrame);
			}
		}

		private void InterpolateEntityKeyframes(int entityId, int currentFrame) {
			foreach (var componentType in _componentTypes) {
				switch (componentType) {
					case var t when t == typeof(Transform):
						SetCurrentInterpolatedComponent<Transform>(entityId);
						break;
					case var t when t == typeof(Shape):
						SetCurrentInterpolatedComponent<Shape>(entityId);
						break;
					default:
						continue;
				}

				InterpolateKeyframeSpline(_currentComponentSpline, _currentInterpolatedComponent, currentFrame);
				SendInterpolationData(entityId);
			}
		}

		private void SetCurrentInterpolatedComponent<T>(int entityId) where T : Component {
			_currentComponentSpline = _keyframeStorage.GetEntityKeyframesOfType<T>(entityId);
			_currentInterpolatedComponent = _componentStorage.GetComponent<T>(entityId);
		}

		private void InterpolateKeyframeSpline<T>(IKeyframeSpline keyframeSpline, T component, int currentFrame) where T : Component {
			var startKeyframe = keyframeSpline.GetLastKeyframe(currentFrame);
			var targetKeyframe = keyframeSpline.GetNextKeyframe(currentFrame);

			float t = 0.0f;
			int deltaFrame = targetKeyframe.Frame - startKeyframe.Frame;

			if (deltaFrame > 0) { // Different keyframes.
				float dt = currentFrame - startKeyframe.Frame;
				t = dt / deltaFrame;
			}

			InterpolateComponent(component, startKeyframe, targetKeyframe, easeOutBack(t));
		}

		private void SendInterpolationData(int entityId) {
			// Send updated data to view.
			var entity = _entityStorage.GetEntity(entityId);

			EntityDisplayInfo.Value = (entity.Id, entity.Name);
		}

		private void InterpolateComponent<T>(T component, IKeyframe<Component> start, IKeyframe<Component> target, float t) {
			switch (component) {
				case Transform transform: {
					var startTransform = start.Value as Transform;
					var targetTransform = target.Value as Transform;

					var deltaPosition = lerp(startTransform.Position, targetTransform.Position, t);
					var deltaScale = lerp(startTransform.Scale, targetTransform.Scale, t);
					var deltaRoll = lerp(startTransform.Roll.AngleDegrees, targetTransform.Roll.AngleDegrees, t);

					transform.Position = deltaPosition;
					transform.Scale = deltaScale;
					transform.Roll.AngleDegrees = deltaRoll;
					break;
				}
				case Shape shape: {
					var startShape = start.Value as Shape;
					var targetShape = target.Value as Shape;

					var deltaColor = lerp(startShape.Color, targetShape.Color, t);

					shape.Color = deltaColor;
					break;
				}
			}
		}

	}
}