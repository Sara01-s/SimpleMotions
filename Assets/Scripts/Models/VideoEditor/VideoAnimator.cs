using static SimpleMotions.SimpleMath;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SimpleMotions {

	public sealed class VideoAnimator : IVideoAnimator {
		
		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IComponentStorage _componentStorage;
		private readonly IEventService _eventService;
		private readonly IEntityStorage _entityStorage;
		private readonly Dictionary<Type, List<int>> _keyframeIndices = new();
		private readonly IEnumerable<Type> _componentTypes;

		private IEnumerable<int> _entitiesWithKeyframes;
		private bool _videoCacheGenerated;
		private IKeyframeSpline _componentKeyframes;
		private Component _component;

		
		public VideoAnimator(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, 
							 IEventService eventService, IEntityStorage entityStorage) 
		{
			_keyframeStorage = keyframeStorage;
			_componentStorage = componentStorage;
			_eventService = eventService;
			_entityStorage = entityStorage;

			_componentTypes = _keyframeStorage.GetKeyframeTypes();
		}

		// TODO - Only call when there are new keyframes.
		public void GenerateVideoCache() {
			_videoCacheGenerated = false;
			var activeEntities = _entityStorage.GetActiveEntities();
			int totalFrames = _keyframeStorage.GetTotalFrames();

			_entitiesWithKeyframes = activeEntities.Where(entityId => _keyframeStorage.EntityHasKeyframesOfAnyType(entityId));

			for (int currentFrame = TimelineData.FIRST_KEYFRAME; currentFrame <= totalFrames; currentFrame++) {
				var keyframesAtCurrentFrame = _keyframeStorage.GetAllKeyframesAt(currentFrame);

				foreach (var keyframe in keyframesAtCurrentFrame) {
					if (keyframe.EntityId == Entity.INVALID_ID)  {
						continue;
					}
					
					var keyframeType = keyframe.Value.GetType();

					if (!_keyframeIndices.ContainsKey(keyframeType)) {
						_keyframeIndices[keyframeType] = new List<int>(); // TODO - Se podría reservar un array de tamaño fijo conociendo la cantidad de keyframes por tipo.
					}

					UnityEngine.Debug.Log($"Se encontró: {keyframe} de tipo {keyframeType} en el frame {currentFrame}");
					_keyframeIndices[keyframeType].Add(currentFrame);
				}
			}

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

				InterpolateEntityComponent(_component, _componentKeyframes, currentFrame);
				SendInterpolationData(entityId);
			}
		}

		private void SetCurrentInterpolatedComponent<T>(int entityId) where T : Component {
			_componentKeyframes = _keyframeStorage.GetEntityKeyframesOfType<T>(entityId);
			_component = _componentStorage.GetComponent<T>(entityId);
		}

		private void InterpolateEntityComponent<T>(T component, IKeyframeSpline keyframeSpline, int currentFrame) where T : Component {
			var startKeyframe = keyframeSpline.GetLastKeyframe(currentFrame);
			var targetKeyframe = keyframeSpline.GetNextKeyframe(currentFrame);

			float t = 0.0f;
			int deltaFrame = targetKeyframe.Frame - startKeyframe.Frame;

			if (deltaFrame > 0) { // Different keyframes.
				t = (currentFrame - startKeyframe.Frame) / deltaFrame;
			}

			InterpolateComponent(component, startKeyframe, targetKeyframe, t);
		}

		private void SendInterpolationData(int entityId) {
			// Send updated data to view.
			var entityDisplayInfoCurrentFrame = new EntityDisplayInfo {
				Entity = _entityStorage.GetEntity(entityId),
				Components = _componentStorage.GetAllComponents(entityId)
			};

			_eventService.Dispatch(entityDisplayInfoCurrentFrame);
		}

		private void InterpolateComponent<T>(T component, IKeyframe<Component> start, IKeyframe<Component> target, float t) {
			switch (component) {
				case Transform transform: {
					var startTransform = start.Value as Transform;
					var targetTransform = target.Value as Transform;

					var deltaPosition = lerpPos(startTransform.Position, targetTransform.Position, t);
					var deltaScale = lerpScale(startTransform.Scale, targetTransform.Scale, t);
					var deltaRoll = lerp(startTransform.Roll.AngleDegrees, targetTransform.Roll.AngleDegrees, t);

					transform.Position = deltaPosition;
					transform.Scale = deltaScale;
					transform.Roll.AngleDegrees = deltaRoll;
					break;
				}
				case Shape shape: {
					var startShape = start.Value as Shape;
					var targetShape = target.Value as Shape;

					var deltaColor = lerpColor(startShape.Color, targetShape.Color, t);

					shape.Color = deltaColor;
					break;
				}
			}
		}

	}
}