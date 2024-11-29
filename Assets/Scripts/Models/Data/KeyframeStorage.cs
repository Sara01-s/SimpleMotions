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
		IKeyframe<T> AddKeyframe<T>(int entityId, int frame, T value, Ease ease = Ease.Linear) where T : Component, new();

		void AddDefaultKeyframes(int entityId);
		void AddDefaultKeyframe(Type componentType, int entityId);

		void DeleteEntityKeyframes(int entityId);
		void ResetEntityKeyframes(int entityId);
		void RemoveKeyframeOfType(Type componentType, int entityId, int frame);

		void SetKeyframeFrame(IKeyframe<Component> keyframe, int targetFrame);
		public void SetKeyframeEase(IKeyframe<Component> original, Ease newEase);

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

			AddKeyframe(entityId, TimelineData.FIRST_FRAME, new Transform());
			AddKeyframe(entityId, TimelineData.FIRST_FRAME, new Shape());
			AddKeyframe(entityId, TimelineData.FIRST_FRAME, new Text());

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

			AddKeyframe(entityId, TimelineData.FIRST_FRAME, component);
		}

		public IKeyframe<T> AddKeyframe<T>(int entityId, int frame, T value, Ease ease = Ease.Linear) where T : Component, new() {
			var keyframe = new Keyframe<T>(entityId, frame, value, ease);
			var componentType = typeof(T);

			if (!_keyframes.TryGetValue(entityId, out var componentToKeyframeSpline)) {
				_keyframes[entityId] = componentToKeyframeSpline = new();
			}

			if (!componentToKeyframeSpline.TryGetValue(componentType, out var keyframeSpline)) {
				componentToKeyframeSpline[componentType] = keyframeSpline = new KeyframeSpline<Component>();
			}

			keyframeSpline.AddKeyframe(frame, keyframe);
			//UnityEngine.Debug.Log("Añadido keyframe: " + keyframe);
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

			var componentKeyframes = componentToKeyframeSpline[typeof(T)];

			foreach (var k in componentKeyframes) {
				UnityEngine.Debug.Log(k);
			}

			if (!componentKeyframes.As<T>().TryGetValue(frame, out var keyframe)) {
				throw new InvalidCastException($"Keyframe found at frame {frame} is not of the expected type {typeof(IKeyframe<T>)}.");
			}

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

        public KeyframesData GetKeyframesData() {
			return new KeyframesData {
				AllKeyframes = _keyframes
			};
		}

		public void SetKeyframeFrame(IKeyframe<Component> original, int newFrame) {
			original.Frame = newFrame;
			UnityEngine.Debug.Log($"keyframe {original} moved to frame {original.Frame}");
		}

		public void SetKeyframeEase(IKeyframe<Component> original, Ease newEase) {
			original.Ease = newEase;
			UnityEngine.Debug.Log($"keyframe {original} changed ease to {original.Ease}");
		}

    }
}