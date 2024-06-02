using System.Collections.Generic;

namespace SimpleMotions {

	public interface IComponentDatabase : System.IDisposable {

		void AddComponent<T>(ref Entity entity, T component, IEntityDatabase entityDatabase) where T : Component;
		void RemoveComponent<T>(Entity entity) where T : Component;
		T GetComponent<T>(Entity entity) where T : Component;
		bool HasComponent<T>(Entity entity) where T : Component;
		IEnumerable<int> GetEntitiesWithComponent<T>() where T : Component;
		Dictionary<int, Component> GetComponentsOfType(System.Type componentType);
		int GetComponentBitmask<T>() where T : Component;

	}
}