using System.Collections.Generic;
using System;

namespace SimpleMotions {

	internal sealed class ComponentDatabase : IComponentDatabase {

		private readonly Dictionary<Type, Dictionary<int, Component>> _components = new();
		private readonly Dictionary<Type, int> _componentBitmasks = new();
		private int _nextComponentBitmask = 1;

		public ComponentDatabase() {
			Services.Instance.RegisterService<IComponentDatabase>(service: this);

			RegisterComponent<Position>();
			RegisterComponent<Scale>();
			RegisterComponent<Roll>();
			RegisterComponent<Shape>();
			RegisterComponent<KeyframeStorage>();
			RegisterComponent<Text>();
		}

		private void RegisterComponent<T>() where T : Component {

			var type = typeof(T);

			if (!_componentBitmasks.ContainsKey(type)) {
				_componentBitmasks[type] = _nextComponentBitmask;
				_nextComponentBitmask <<= 1;
			}
		}

		public void AddComponent<T>(Entity entity) where T : Component, new() {
			var componentType = typeof(T);

			if (!_components.ContainsKey(componentType)) {
				_components[componentType] = new Dictionary<int, Component>();
			}
			
			var component = new T();
			_components[componentType][entity.Id] = component;
			entity.ComponentMask |= _componentBitmasks[componentType];
		}

		public void RemoveComponent<T>(Entity entity) where T : Component {
			var componentType = typeof(T);
			
			entity.ComponentMask &= ~_componentBitmasks[componentType];
			_components[componentType].Remove(entity.Id);
		}

		public T GetComponent<T>(Entity entity) where T : Component {
			var type = typeof(T);
			
			if (_components.ContainsKey(type) && _components[type].ContainsKey(entity.Id)) {
				return (T)_components[type][entity.Id];
			}

			return null;
		}

		public bool HasComponent<T>(Entity entity) where T : Component {
			var type = typeof(T);
			return _components.ContainsKey(type) && _components[type].ContainsKey(entity.Id);
		}

		public IEnumerable<int> GetEntitiesWithComponent<T>() where T : Component {

			var type = typeof(T);

			if (_components.ContainsKey(type)) {
				return _components[type].Keys;
			}

			return new List<int>();
		}

		public Dictionary<int, Component> GetComponentsOfType<T>() where T : Component {
			var componentType = typeof(T);
			return _components.ContainsKey(componentType) ? _components[componentType] : new Dictionary<int, Component>();
		}

		public int GetComponentBitmask<T>() where T : Component {
			return _componentBitmasks[typeof(T)];
		}

		public void Dispose() {
			Services.Instance.UnRegisterService<IComponentDatabase>();
		}
		
	}
}