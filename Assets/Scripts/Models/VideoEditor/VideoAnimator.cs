using System.Collections.Generic;
using System.Linq;
using System;

namespace SimpleMotions {

    public sealed class VideoAnimator : IVideoAnimator {
        
        private readonly IKeyframeStorage _keyframeStorage;
        private readonly IComponentStorage _componentStorage;
		private readonly IEventService _eventService;
		private readonly IEntityStorage _entityStorage;

		private readonly List<int> _keyframeIndices = new();

		private IEnumerable<int> _entitiesWithKeyframes;
		private bool _videoCacheGenerated = false;
		private int _lastKeyframeIndex = -1;
		private IList<IKeyframe<Component>> _componentKeyframes;
		private Component _component;

		
        public VideoAnimator(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, 
							 IEventService eventService, IEntityStorage entityStorage) 
		{
            _keyframeStorage = keyframeStorage;
            _componentStorage = componentStorage;
			_eventService = eventService;
			_entityStorage = entityStorage;
        }

		// TODO - Only call when there are new keyframes.
        public void GenerateVideoCache() {
			_videoCacheGenerated = false;
			var activeEntities = _entityStorage.GetActiveEntities();

			_entitiesWithKeyframes = activeEntities.Where(entityId => _keyframeStorage.EntityHasKeyframesOfAnyType(entityId));
			_lastKeyframeIndex = -1;

            int totalFrames = _keyframeStorage.GetTotalFrames();

            for (int currentFrame = TimelineData.FIRST_KEYFRAME; currentFrame <= totalFrames; currentFrame++) {
                var keyframe = _keyframeStorage.GetKeyframeAt(currentFrame);

                if (keyframe.EntityId == Entity.INVALID_ID) {
                    continue;
                }

				UnityEngine.Debug.Log($"Se encontró: {keyframe} en el frame {currentFrame}");
				_keyframeIndices.Add(currentFrame);
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
			var keyframe = _keyframeStorage.GetKeyframeAt(currentFrame);
			var componentTypes = _keyframeStorage.GetKeyframeTypes();

			// Since all entites have a default keyframe at frame 0, this condition will always be true for currentFrame = 0.
			if (keyframe.EntityId != Entity.INVALID_ID) {
				foreach (var componentType in componentTypes) {
					
					if (componentType == typeof(Transform)) {
						_component = _componentStorage.GetComponent<Transform>(entityId);
						_componentKeyframes = _keyframeStorage.GetEntityKeyframesOfType<Transform>(entityId);
					}
//					if (componentType == typeof(Shape)) {
//						_component = _componentStorage.GetComponent<Shape>(entityId);
//						_componentKeyframes = _keyframeStorage.GetEntityKeyframesOfType<Shape>(entityId);
//					}
//					else if (componentType == typeof(Text)) {
//						_component = _componentStorage.GetComponent<Text>(entityId);
//						_componentKeyframes = _keyframeStorage.GetEntityKeyframesOfType<Text>(entityId);
//					}
						
					//throw new Exception($"Tried to interpolate invalid component type. Component type: {keyframe.Value}");

				}
			}

			InterpolateEntityComponent(entityId, _component, _componentKeyframes, currentFrame);
		}

		public void InterpolateEntityComponent<T>(int entityId, T component, IList<IKeyframe<Component>> componentKeyframes, int currentFrame) where T : Component {
			if (_keyframeIndices.Contains(currentFrame)) {
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