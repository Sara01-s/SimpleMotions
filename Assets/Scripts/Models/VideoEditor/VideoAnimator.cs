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

		private IEnumerable<int> _entitiesWithKeyframes;
		private bool _videoCacheGenerated = false;
		private int _lastKeyframeIndex = -1;
		private IList<IKeyframe<Component>> _componentKeyframes;
		private Component _component;
		private readonly IEnumerable<Type> _componentTypes;

		
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

			_entitiesWithKeyframes = activeEntities.Where(entityId => _keyframeStorage.EntityHasKeyframesOfAnyType(entityId));
			_lastKeyframeIndex = 0;

            int totalFrames = _keyframeStorage.GetTotalFrames();

            for (int currentFrame = TimelineData.FIRST_KEYFRAME; currentFrame <= totalFrames; currentFrame++) {
				var keyframesAtCurrentFrame = _keyframeStorage.GetAllKeyframesAt(currentFrame);

				foreach (var keyframe in keyframesAtCurrentFrame) {
					if (keyframe.EntityId != Entity.INVALID_ID) {
						var keyframeType = keyframe.Value.GetType();

						if (!_keyframeIndices.ContainsKey(keyframeType)) {
							_keyframeIndices[keyframeType] = new List<int>(); // TODO - Se podría reservar un array de tamaño fijo conociendo la cantidad de keyframes por tipo.
						}

						UnityEngine.Debug.Log($"Se encontró: {keyframe} de tipo {keyframeType} en el frame {currentFrame}");
						_keyframeIndices[keyframeType].Add(currentFrame);
					}
				}
            }

			_videoCacheGenerated = true;
        }

		public void InterpolateAllEntities(int currentFrame) {
			if (!_videoCacheGenerated) {
				throw new Exception("Generate video cache before entity interpolatation.");
			}

			foreach (int entityId in _entitiesWithKeyframes) {
				InterpolateEntityKeyframes(entityId, currentFrame);
			}
		}

		public void InterpolateEntityKeyframes(int entityId, int currentFrame) {
			IEnumerable<IKeyframe<Component>> keyframesInFrame = _keyframeStorage.GetAllKeyframesAt(currentFrame);
			// Since all entites have a default keyframe at frame 0, this condition will always be true for currentFrame = 0.

			// Keyframes con valores asociados
			foreach (var componentType in _componentTypes) {
				// Transform, Shape, Text - entramos acá 3 veces SI O SI.
				
				var keyframe = (from k in keyframesInFrame
								where k.Value.GetType() == componentType
								select k).SingleOrDefault()
								?? Keyframe<Component>.Invalid;

				if (keyframe.EntityId != Entity.INVALID_ID) {
					if (keyframe.Value is Transform) {
						_component = _componentStorage.GetComponent<Transform>(entityId);
						_componentKeyframes = _keyframeStorage.GetEntityKeyframesOfType<Transform>(entityId);
					}
					else if (keyframe.Value is Shape) {
						_component = _componentStorage.GetComponent<Shape>(entityId);
						_componentKeyframes = _keyframeStorage.GetEntityKeyframesOfType<Shape>(entityId);
					}
					else if (keyframe.Value is Text) {
						_component = _componentStorage.GetComponent<Text>(entityId);
						_componentKeyframes = _keyframeStorage.GetEntityKeyframesOfType<Text>(entityId);
					}

					// ENSEÑARLE MATEMATICAS BASICAS AL PC.
				}
				
				InterpolateEntityComponent(entityId, _component, _componentKeyframes, currentFrame);
			}
		}

		public void InterpolateEntityComponent<T>(int entityId, T component, IList<IKeyframe<Component>> componentKeyframes, int currentFrame) where T : Component {
			if (_keyframeIndices[component.GetType()].Contains(currentFrame)) {
				if (_lastKeyframeIndex + 1 < componentKeyframes.Count) {
					_lastKeyframeIndex += 1;
				}
			}

			int nextKeyframeIndex = _lastKeyframeIndex + 1;
			var startKeyframe = componentKeyframes[_lastKeyframeIndex];
			var targetKeyframe = nextKeyframeIndex < componentKeyframes.Count 
								? componentKeyframes[nextKeyframeIndex]
								: startKeyframe;

			float t = 0.0f;
			int deltaFrame = targetKeyframe.Frame - startKeyframe.Frame;

			if (deltaFrame > 0) { // Keyframes are not the same.
				float dt = currentFrame - startKeyframe.Frame;
				t = dt / deltaFrame;
			}

			switch (component) {
				case Transform transform:
					var startTransform = startKeyframe.Value as Transform;
					var targetTransform = targetKeyframe.Value as Transform;

					var deltaPosition = LerpPos(startTransform.Position, targetTransform.Position, t);
					var deltaScale = LerpScale(startTransform.Scale, targetTransform.Scale, t);
					var deltaRoll = Lerp(startTransform.Roll.AngleDegrees, targetTransform.Roll.AngleDegrees, t);

					transform.Position = deltaPosition;
					transform.Scale = deltaScale;
					transform.Roll.AngleDegrees = deltaRoll;
					break;
				case Shape shape:
					var startShape = startKeyframe.Value as Shape;
					var targetShape = targetKeyframe.Value as Shape;

					var deltaColor = LerpColor(startShape.Color, targetShape.Color, t);

					shape.Color = deltaColor;
					break;
				default:
					break;
			}

			// Send updated data to be displayed by view.
			var entityDisplayInfoCurrentFrame = new EntityDisplayInfo {
				Entity = _entityStorage.GetEntity(entityId),
				Components = _componentStorage.GetAllComponents(entityId)
			};

			_eventService.Dispatch<EntityDisplayInfo>(entityDisplayInfoCurrentFrame);
		}


		private float Lerp(float a, float b, float t) {
			return a * (1.0f - t) + b * t;
		}

		private Color LerpColor(Color c, Color d, float t) {
			float dr = Lerp(c.R, d.R, t);
			float dg = Lerp(c.G, d.G, t);
			float db = Lerp(c.B, d.B, t);
			float da = Lerp(c.A, d.A, t);

			return new Color(dr, dg, db, da);
		}

		private Position LerpPos(Position u, Position v, float t) {
			float dx = Lerp(u.X, v.X, t);
			float dy = Lerp(u.Y, v.Y, t);

			return new Position(dx, dy);
		}

		private Scale LerpScale(Scale u, Scale v, float t) {
			float dw = Lerp(u.Width, v.Width, t);
			float dh = Lerp(u.Height, v.Height, t);

			return new Scale(dw, dh);
		}

	}
}