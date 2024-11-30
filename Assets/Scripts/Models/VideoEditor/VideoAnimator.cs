using System.Collections.Generic;
using SimpleMotions.Internal;
using System;
using System.Diagnostics;
using System.Linq;

namespace SimpleMotions {

    public interface IVideoAnimator {
        
        void GenerateVideoCache();
        void InterpolateAllEntities(int currentFrame);
		ReactiveValue<Entity> EntityDisplayInfo { get; }

    }

	public sealed class VideoAnimator : IVideoAnimator {
		
		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IComponentStorage _componentStorage;
		private readonly IInterpolator _interpolator;
		private readonly IEntityStorage _entityStorage;

		private IEnumerable<int> _activeEntities;
		private IEnumerable<int> _interpolableEntites;
		private IEnumerable<int> _lastInterpolableEntities;
		private bool _videoCacheGenerated;

		private IKeyframeSpline<Component> _currentComponentSpline;
		private Component _currentInterpolatedComponent;

		public ReactiveValue<Entity> EntityDisplayInfo { get; } = new();
		
		public VideoAnimator(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, 
							 IEntityStorage entityStorage, IInterpolator interpolator) {
			_keyframeStorage = keyframeStorage;
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;
			_interpolator = interpolator;
		}

		public void GenerateVideoCache() {
			bool cacheAlreadyGenerated = _lastInterpolableEntities != null && _interpolableEntites != _lastInterpolableEntities;
			if (cacheAlreadyGenerated) {
				return;
			}

			_videoCacheGenerated = false;

			_activeEntities = _entityStorage.GetActiveEntities();
			_interpolableEntites = _activeEntities.Where(entityId => _keyframeStorage.EntityHasMoreThanOneKeyframe(entityId));
			_lastInterpolableEntities = _interpolableEntites;

			_videoCacheGenerated = true;
		}

		public void InterpolateAllEntities(int currentFrame) {
			if (!_videoCacheGenerated) {
				throw new Exception("Generate video cache before starting entity interpolation.");
			}

			foreach (int entityId in _interpolableEntites) {
				InterpolateEntityKeyframes(entityId, currentFrame);
			}
		}

		private void InterpolateEntityKeyframes(int entityId, int currentFrame) {
			foreach (var componentType in _keyframeStorage.GetEntityKeyframeTypes(entityId)) {
				SetCurrentInterpolatedComponent(componentType, entityId);
				InterpolateKeyframeSpline(_currentComponentSpline, _currentInterpolatedComponent, currentFrame);
				SendInterpolationData(entityId);
			}
		}

		private void InterpolateKeyframeSpline(IKeyframeSpline<Component> keyframeSpline, Component component, int currentFrame) {
			var startKeyframe = keyframeSpline.GetPreviousKeyframe(currentFrame);
			var targetKeyframe = keyframeSpline.GetNextKeyframe(currentFrame);

			//UnityEngine.Debug.Log(startKeyframe.Ease);

			float t = 0.0f;
			int deltaFrame = targetKeyframe.Frame - startKeyframe.Frame;

			if (deltaFrame > 0) { // Different keyframes.
				float dt = currentFrame - startKeyframe.Frame;
				t = dt / deltaFrame;
			}

			InterpolateComponent(component, startKeyframe, targetKeyframe, t);
		}

		private void InterpolateComponent(Component component, IKeyframe<Component> start, IKeyframe<Component> target, float t) {
			if (!_interpolator.TryGetInterpolation(component.GetType(), out var interpolation)) {
				throw new NotSupportedException($"Interpolation not supported for type {component.GetType().Name}.");
			}

			interpolation.Interpolate(component, start.Value, target.Value, t, start.Ease);
		}

		private void SetCurrentInterpolatedComponent(Type componentType, int entityId) {
			_currentComponentSpline = _keyframeStorage.GetEntityKeyframeSplineOfType(componentType, entityId);
			_currentInterpolatedComponent = _componentStorage.GetComponent(componentType, entityId);
		}

		private void SendInterpolationData(int entityId) {
			EntityDisplayInfo.Value = _entityStorage.GetEntity(entityId);
		}

	}
}