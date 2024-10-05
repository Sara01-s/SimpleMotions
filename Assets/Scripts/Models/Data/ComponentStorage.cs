using System.Collections.Generic;
using System;

namespace SimpleMotions {

	public sealed class ComponentStorage : IComponentStorage {

		private readonly SerializableDictionary<Type, SerializableDictionary<int, Component>> _components = new();
		private readonly SerializableDictionary<Type, int> _componentBitmasks = new();
		private int _nextComponentBitmask = 1;

		public ComponentStorage(ComponentsData componentsData) {
			_components = componentsData.Components;
			_componentBitmasks = componentsData.ComponentBitmasks;
			_nextComponentBitmask = componentsData.NextComponentBitmask;

			RegisterComponent<Position>();
			RegisterComponent<Scale>();
			RegisterComponent<Roll>();
			RegisterComponent<Shape>();
			RegisterComponent<KeyframeStorage>();
			RegisterComponent<Text>();
		}
		
		public ComponentsData GetComponentsData() {
			return new ComponentsData {
				Components = _components,
				ComponentBitmasks = _componentBitmasks,
				NextComponentBitmask = _nextComponentBitmask
			};
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
				_components[componentType] = new SerializableDictionary<int, Component>();
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
		
	}
}