using System.Collections.Generic;
using SimpleMotions.Internal;
using System.Linq;
using System;
using Unity.Collections;

#nullable enable

namespace SimpleMotions {

	public interface IKeyframeStorage {
		int TotalFrames { get; }
		Type[] AllComponentTypes { get; }

		bool EntityHasKeyframes(int entityId);
		bool EntityHasKeyframesOfType(Type componentType, int entityId);
		bool EntityHasAnyKeyframeAtFrame(int entityId, int frame);
		bool EntityHasKeyframeAtFrameOfType<T>(int entityId, int frame) where T : Component, new();
		bool EntityHasMoreThanOneKeyframe(int entityId);

		void AddKeyframe(IKeyframe<Component> keyframe);
		void AddKeyframe<T>(IKeyframe<T> keyframe) where T : Component, new();
		IKeyframe<T> AddKeyframe<T>(int entityId, int frame, T value, Ease ease) where T : Component, new();

		void AddDefaultKeyframes(int entityId);
		void AddDefaultKeyframe(Type componentType, int entityId);

		void DeleteEntityKeyframes(int entityId);
		void ResetEntityKeyframes(int entityId);
		void RemoveKeyframeOfType(Type componentType, int entityId, int frame);

		void SetKeyframeFrame(int entityId, Type componentType, int originalFrame, int newFrame);
		void SetKeyframeEase(int entityId, Type componentType, int originalFrame, Ease newEase);
		void SetKeyframeValue<T>(int entityId, int frame, T newValue) where T : Component, new();

		IKeyframeSpline<T> GetEntityKeyframeSplineOfType<T>(int entityId) where T : Component, new();
		IKeyframeSpline<Component> GetEntityKeyframeSplineOfType(Type componentType, int entityId);
		IEnumerable<IKeyframeSpline<Component>> GetEntityKeyframeSplines(int entityId);

		IEnumerable<IKeyframe<Component>> GetEntityKeyframesAtFrame(int entityId, int frame);
		IEnumerable<Type> GetEntityKeyframeTypes(int entityId);
		IKeyframe<T>? GetEntityKeyframeOfType<T>(int entityId, int frame) where T : Component, new();

		KeyframesData GetKeyframesData();
	}

	public sealed class KeyframeStorage : IKeyframeStorage {

		public int TotalFrames => _videoData.TotalFrames;
		public Type[] AllComponentTypes { get; }

		// entity id -> [component type -> keyframe spline (frame -> keyframe)].
		private readonly Dictionary<int, Dictionary<Type, IKeyframeSpline<Component>>> _keyframes;
		private readonly VideoData _videoData;

		public KeyframeStorage(KeyframesData keyframesData, VideoData videoData) {
			_keyframes = keyframesData.AllKeyframes;
			_videoData = videoData;

			AllComponentTypes = new Type[] { // TODO - Get from outside this class. (Dependency injection).
				typeof(Transform), typeof(Shape), typeof(Text)
			};
		}

		public bool EntityHasKeyframes(int entityId) {
			return _keyframes.TryGetValue(entityId, out var _);
		}

		// This is useful for determining interpolable entities. (Since and interpolation needs at minimun 2 keyframes).
		public bool EntityHasMoreThanOneKeyframe(int entityId) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return false;
			}

			foreach(var componentType in GetEntityKeyframeTypes(entityId)) {
				if (componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
					if (keyframeSpline.Count > 1) {
						return true;
					}
				}
			}

			return false;
		}

		public bool EntityHasKeyframesOfType(Type componentType, int entityId) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return false;
			}

			return componentToKeyframeSpline.ContainsKey(componentType);
		}

		public bool EntityHasAnyKeyframeAtFrame(int entityId, int frame) {
			return GetEntityKeyframesAtFrame(entityId, frame).Count() != 0;
		}

		public bool EntityHasKeyframeAtFrameOfType<T>(int entityId, int frame) where T : Component, new() {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return false;
			}


			if (!componentToKeyframeSpline.TryGetValue(typeof(T), out var keyframeSpline)) {
				return false;
			}

			return keyframeSpline.HasKeyframeAtFrame(frame);
 		}

		public void AddKeyframe(IKeyframe<Component> keyframe) {
			AddKeyframe(keyframe.EntityId, keyframe.Frame, keyframe.Value, keyframe.Ease);
		}

		public void AddKeyframe<T>(IKeyframe<T> keyframe) where T : Component, new() {
			AddKeyframe(keyframe.EntityId, keyframe.Frame, keyframe.Value, keyframe.Ease);
		}

		public void AddDefaultKeyframes(int entityId) {
			if (entityId == Entity.Invalid.Id) {
				return;
			}

			AddKeyframe(entityId, TimelineData.FIRST_FRAME, new Transform(), Ease.Linear);
			AddKeyframe(entityId, TimelineData.FIRST_FRAME, new Shape(), Ease.Linear);
			AddKeyframe(entityId, TimelineData.FIRST_FRAME, new Text(), Ease.Linear);

			//foreach (var componentType in AllComponentTypes) {
			//	AddDefaultKeyframe(componentType, entityId);
			//}
		}

		// TODO - Abstraer esto, el problema es que no puedo crear instancias específicas de Transform, Shape y Text,
		// todas son tratadas como Component.
		public void AddDefaultKeyframe(Type componentType, int entityId) {
			if (!AllComponentTypes.Contains(componentType)) {
				throw new ArgumentException($"Unsupported component type: {componentType}", nameof(componentType));
			}

			var component = (Component)Activator.CreateInstance(componentType);

			AddKeyframe(entityId, TimelineData.FIRST_FRAME, component, Ease.Linear);
		}

		public IKeyframe<T> AddKeyframe<T>(int entityId, int frame, T value, Ease ease) where T : Component, new() {
			var keyframe = new Keyframe<T>(entityId, frame, value, ease);
			var componentType = typeof(T);

			if (componentType == typeof(Component)) {
				throw new Exception("Cannot create keyframe holding a value of type: " + componentType);
			}

			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				_keyframes[entityId] = componentToKeyframeSpline = new();
			}

			if (!componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
				componentToKeyframeSpline[componentType] = keyframeSpline = new KeyframeSpline<Component>();
			}

			keyframeSpline.AddKeyframe(frame, keyframe);
			return keyframe;
		}

		/// <summary> Deletes all entity keyframes (use only for entity deletion). </summary>
		public void DeleteEntityKeyframes(int entityId) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return;
			}

			componentToKeyframeSpline.Clear();
		}

		/// <summary> Deletes all entity keyframes and adds default keyframes again. </summary>
		public void ResetEntityKeyframes(int entityId) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return;
			}

			componentToKeyframeSpline.Clear();
			AddDefaultKeyframes(entityId);
		}

		public void RemoveKeyframeOfType(Type componentType, int entityId, int frame) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return;
			}

			if (!componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
				return;
			}

			if (!keyframeSpline.HasKeyframeAtFrame(frame)) {
				return;
			}

			keyframeSpline.RemoveKeyframe(frame);
		}

		public IKeyframeSpline<Component> GetEntityKeyframeSplineOfType(Type componentType, int entityId) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				throw new ArgumentException("Entity has no keyframes", entityId.ToString());
			}

			if (!EntityHasKeyframesOfType(componentType, entityId)) {
				throw new ArgumentException($"Entity has no keyframes of type {componentType}", componentType.Name);
			}

			if (!componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
				throw new ArgumentException($"Component has no keyframe spline {componentType}", componentType.Name);
			}

			return keyframeSpline;
		}

		public IKeyframeSpline<T> GetEntityKeyframeSplineOfType<T>(int entityId) where T : Component, new() {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				throw new ArgumentException("Entity has no keyframes", entityId.ToString());
			}

			var componentType = typeof(T);

			if (!EntityHasKeyframesOfType(componentType, entityId)) {
				throw new ArgumentException($"Entity has no keyframes of type {componentType}", componentType.Name);
			}

			if (!componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
				throw new ArgumentException($"Component has no keyframe spline {componentType}", componentType.Name);
			}

			return keyframeSpline.As<T>();
		}

		public IEnumerable<IKeyframeSpline<Component>> GetEntityKeyframeSplines(int entityId) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return Enumerable.Empty<IKeyframeSpline<Component>>();
			}

			var entityKeyframeSplines = new List<IKeyframeSpline<Component>>();

			foreach (var componentType in componentToKeyframeSpline.Keys) {
				if (componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
					entityKeyframeSplines.Add(keyframeSpline);
				}
			}

			return entityKeyframeSplines;
		}

		// TODO - Tiene que evitar buscar por todos los tipos de componentes
        public IKeyframe<T>? GetEntityKeyframeOfType<T>(int entityId, int frame) where T : Component, new() {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				throw new ArgumentException("Entity has no keyframes", entityId.ToString());
			}

			var componentKeyframeSpline = componentToKeyframeSpline[typeof(T)];

			if (!componentKeyframeSpline.As<T>().TryGetValue(frame, out var keyframe)) {
				throw new InvalidCastException($"Keyframe found at frame {frame} is not of the expected type {typeof(IKeyframe<T>)}.");
			}

			UnityEngine.Debug.Log("KEYFRAME STORAGE: " + keyframe.Ease);
			return keyframe;
        }

		public IEnumerable<IKeyframe<Component>> GetEntityKeyframesAtFrame(int entityId, int frame) {
			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				return Array.Empty<IKeyframe<Component>>();
			}

			var foundKeyframes = new List<IKeyframe<Component>>();

			foreach (var componentType in componentToKeyframeSpline.Keys) {
				if (componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
					if (keyframeSpline.TryGetValue(frame, out var keyframe)) {
						foundKeyframes.Add(keyframe);
					}
				}
			}
			
			if (foundKeyframes.Count <= 0) {
				return Enumerable.Empty<IKeyframe<Component>>();
			}

			return foundKeyframes;
		}

		public IEnumerable<Type> GetEntityKeyframeTypes(int entityId) {
			if (_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				var entityKeyframeTypes = componentToKeyframeSpline.Keys;
				
				return entityKeyframeTypes.Where(componentType => {
					var keyframeSpline = componentToKeyframeSpline[componentType];
					return keyframeSpline.Count > 1; // If this component only has the default keyframe associated, exclude it.
				});
			}

			return Enumerable.Empty<Type>();
		}

		public void SetKeyframeFrame(int entityId, Type componentType, int originalFrame, int newFrame) {
			switch (componentType) {
				case var component when component == typeof(Transform):
					var transformKeyframe = GetEntityKeyframeOfType<Transform>(entityId, originalFrame) 
						?? throw new NullReferenceException("Keyframe not found.");
					AddKeyframe(entityId, newFrame, transformKeyframe.Value, transformKeyframe.Ease);
				break;
				case var component when component == typeof(Shape):
					var shapeKeyframe = GetEntityKeyframeOfType<Shape>(entityId, originalFrame) 
						?? throw new NullReferenceException("Keyframe not found.");
					AddKeyframe(entityId, newFrame, shapeKeyframe.Value, shapeKeyframe.Ease);
				break;
				default:
					throw new NotImplementedException();
			}
			
			RemoveKeyframeOfType(componentType, entityId, originalFrame);
		}

		public void SetKeyframeEase(int entityId, Type componentType, int originalFrame, Ease newEase) {
			switch (componentType) {
				case var component when component == typeof(Transform):
					var transformKeyframe = GetEntityKeyframeOfType<Transform>(entityId, originalFrame) 
						?? throw new NullReferenceException("Keyframe not found.");
					AddKeyframe(entityId, originalFrame, transformKeyframe.Value, newEase);
				break;
				case var component when component == typeof(Shape):
					var shapeKeyframe = GetEntityKeyframeOfType<Shape>(entityId, originalFrame) 
						?? throw new NullReferenceException("Keyframe not found.");
					AddKeyframe(entityId, originalFrame, shapeKeyframe.Value, newEase);
				break;
				default:
					throw new NotImplementedException();
			}
		}

		public void SetKeyframeValue<T>(int entityId, int frame, T newValue) where T : Component, new() {

		}

        public KeyframesData GetKeyframesData() {
			return new KeyframesData {
				AllKeyframes = _keyframes
			};
		}

	}
}
