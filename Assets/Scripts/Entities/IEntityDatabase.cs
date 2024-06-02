using System.Collections.Generic;

namespace SimpleMotions {

	public interface IEntityDatabase : System.IDisposable {

		Entity CreateEntity();
		void DestroyEntity(Entity entity);
		bool IsAlive(Entity entity);
		IEnumerable<int> GetAliveEntities();
		void UpdateComponentMask(ref Entity entity, int componentBitmask);
		void RemoveComponentMask(ref Entity entity, int componentBitmask);
		Entity GetEntity(int entityId);

	}
}