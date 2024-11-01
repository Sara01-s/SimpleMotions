using System.Collections.Generic;

namespace SimpleMotions {

	public interface IEntityStorage {

		Entity CreateEntity();
		Entity CreateEntity(string name);
		
		void DestroyEntity(int entityId);
		void SetActive(int entityId, bool active);

		bool IsAlive(int entityId);
		IEnumerable<int> GetAliveEntities();
		IEnumerable<int> GetActiveEntities();
		Entity GetEntity(int entityId);
		EntitiesData GetEntitiesData();

	}
}