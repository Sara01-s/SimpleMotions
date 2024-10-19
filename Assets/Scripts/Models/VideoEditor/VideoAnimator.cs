using System.Collections.Generic;
using System.Linq;
using System;

namespace SimpleMotions {

    public sealed class VideoAnimator : IVideoAnimator {
        
        private readonly IKeyframeStorage _keyframeStorage;
        private readonly IComponentStorage _componentStorage;
		private readonly IEventService _eventService;
		private readonly IEntityStorage _entityStorage;
        private readonly SortedDictionary<int, Keyframe<Transform>> _transformKeyframes = new();
		private readonly List<int> _keyframeIndices = new();

		private IEnumerable<int> _entitiesWithKeyframes;
		private bool _videoCacheGenerated = false;
		private int _lastKeyframeIndex = -1;

		
        public VideoAnimator(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, 
							 IEventService eventService, IEntityStorage entityStorage) 
		{
            _keyframeStorage = keyframeStorage;
            _componentStorage = componentStorage;
			_eventService = eventService;
			_entityStorage = entityStorage;
        }

        public void GenerateVideoCache() {
			_videoCacheGenerated = false;

			var activeEntities = _entityStorage.GetActiveEntities();

			_entitiesWithKeyframes = activeEntities.Where(entityId => _keyframeStorage.EntityHasKeyframes<Transform>(entityId));
			_lastKeyframeIndex = -1;
            _transformKeyframes.Clear();

            int totalFrames = _keyframeStorage.GetTotalFrames();
            int validFrame = 0;

            for (int currentFrame = TimelineData.FIRST_KEYFRAME; currentFrame <= totalFrames; currentFrame++) {
                var keyframe = _keyframeStorage.GetKeyframeAt<Transform>(currentFrame);
				UnityEngine.Debug.Log(keyframe);

                if (keyframe.EntityId == Entity.INVALID_ID) {
                    continue;
                }

				// 

                _transformKeyframes.Add(validFrame, keyframe);
				_keyframeIndices.Add(keyframe.Frame);

                validFrame++;
            }

			_videoCacheGenerated = true;
        }

		public void InterpolateAllEntities(int currentFrame) {
			if (!_videoCacheGenerated) {
				throw new System.Exception("Generate video cache before entity interpolatation.");
			}

			foreach (int entityId in _entitiesWithKeyframes) {
				InterpolateEntityKeyframes(entityId, currentFrame);
			}
		}

		public void InterpolateEntityKeyframes(int entityId, int currentFrame) {
			if (_keyframeIndices.Contains(currentFrame)) {
				// keyframe found.
				if (_lastKeyframeIndex + 1 < _transformKeyframes.Count) {
					_lastKeyframeIndex += 1;
				}
			}

			// rest of the frames.

			var startKeyframe = _transformKeyframes[_lastKeyframeIndex]; // 0, 1, 2, 3

			if (!_transformKeyframes.TryGetValue(_lastKeyframeIndex + 1, out var targetKeyframe)) {
				targetKeyframe = startKeyframe;
			}

			float t = 0.0f;
			int deltaFrame = targetKeyframe.Frame - startKeyframe.Frame;

			if (deltaFrame > 0) { // keyframes diferentes
				t = (currentFrame - startKeyframe.Frame) / (float)deltaFrame;
			}

			var deltaPosition = LerpPos(startKeyframe.Value.Position, targetKeyframe.Value.Position, t);
			var transform = _componentStorage.GetComponent<Transform>(entityId);
			
			transform.Position = deltaPosition;


			// Send data to view
			var entityDisplayInfoCurrentFrame = new EntityDisplayInfo {
				Entity = _entityStorage.GetEntity(entityId),
				Components = _componentStorage.GetAllComponents(entityId)
			};

			_eventService.Dispatch<EntityDisplayInfo>(entityDisplayInfoCurrentFrame);
		}

		public void InterpolateEntityComponent<T>(T component) where T : Component {
			if (component is Transform transform) {
				
			}
			else if (component is Shape shape) {

			}
			else if (component is Text text) {

			}
		}

		
		private float Lerp(float a, float b, float t) {
			return a * (1.0f - t) + b * t;
		}

		private Position LerpPos(Position u, Position v, float t) {
			float dx = Lerp(u.X, v.X, t);
			float dy = Lerp(u.Y, v.Y, t);

			return new Position(dx, dy);
		}
    }
}