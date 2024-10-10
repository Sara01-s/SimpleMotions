using System.Collections.Generic;

namespace SimpleMotions {

	public interface IComponentStorage {

		T AddComponent<T>(Entity entity) where T : Component, new();
		void RemoveComponent<T>(Entity entity) where T : Component;

		T GetComponent<T>(Entity entity) where T : Component;
		T GetComponent<T>(int entityId) where T : Component;

		bool HasComponent<T>(Entity entity) where T : Component;
		bool HasComponent<T>(int entityId) where T : Component;

		IEnumerable<int> GetEntitiesWithComponent<T>() where T : Component;
		Dictionary<int, Component> GetComponentsOfType<T>() where T : Component;
		int GetComponentBitmask<T>() where T : Component;

		Component[] GetAllComponents(Entity entity);
		Component[] GetAllComponents(int entityId);

		ComponentsData GetComponentsData();

	}
}