using System.Collections.Generic;
using System;

namespace SimpleMotions {

	public interface IComponentStorage {

		T AddComponent<T>(Entity entity) where T : Component, new();
		void RemoveComponent<T>(Entity entity) where T : Component;

		T GetComponent<T>(int entityId) where T : Component;
		bool HasComponent<T>(int entityId) where T : Component;

		IEnumerable<int> GetEntitiesWithComponent<T>() where T : Component;
		Dictionary<int, Component> GetComponentsOfType<T>() where T : Component;
		int GetComponentBitmask<T>() where T : Component;

		Component[] GetAllComponents(int entityId);

		ComponentsData GetComponentsData();

	}

	public sealed class ComponentStorage : IComponentStorage {

		// Example Usage: Dictionary<Position Type, Dictionary<entityID, Position instance>>
		private readonly Dictionary<Type, Dictionary<int, Component>> _components = new();
		private readonly Dictionary<Type, int> _componentBitmasks = new();
		private int _nextComponentBitmask = 1;


		public ComponentStorage(ComponentsData componentsData) {
			_components = componentsData.Components;
			_componentBitmasks = componentsData.ComponentBitmasks;
			_nextComponentBitmask = componentsData.NextComponentBitmask;

			RegisterComponent<Transform>();
			RegisterComponent<Shape>();
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
				_componentBitmasks.Add(type, _nextComponentBitmask);
				_nextComponentBitmask <<= 1;
			}
		}

		public T AddComponent<T>(Entity entity) where T : Component, new() {
			var componentType = typeof(T);

			if (!_components.ContainsKey(componentType)) {
				_components[componentType] = new Dictionary<int, Component>();
			}
			
			var component = new T();
			_components[componentType][entity.Id] = component;
			entity.ComponentMask |= _componentBitmasks[componentType];

			return component;
		}

		public void RemoveComponent<T>(Entity entity) where T : Component {
			var componentType = typeof(T);
			
			entity.ComponentMask &= ~_componentBitmasks[componentType];
			_components[componentType].Remove(entity.Id);
		}

		public T GetComponent<T>(int entityId) where T : Component {
			var type = typeof(T);
			
			if (_components.ContainsKey(type) && _components[type].ContainsKey(entityId)) {
				// get instance of component "T" from entity "Id"
				return (T)_components[type][entityId];
			}

			return null;
		}

		public Component[] GetAllComponents(int entityId) {
			var components = new List<Component>();

			foreach (var componentType in _components.Keys) { 					// For all registered components
				if (_components[componentType].ContainsKey(entityId)) {		// if entity "id" has that component
					components.Add(_components[componentType][entityId]);		// add the unique component instance associated with entity "id"
				}
			}

			return components.ToArray();
		}

		public bool HasComponent<T>(int entityId) where T : Component {
			var type = typeof(T);
			return _components.ContainsKey(type) && _components[type].ContainsKey(entityId);
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