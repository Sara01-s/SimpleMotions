using System.Collections.Generic;

namespace SimpleMotions {

	public interface IEntityStorage : System.IDisposable {

		Entity CreateEntity();
		void DestroyEntity(Entity entity);
		bool IsAlive(Entity entity);
		IEnumerable<int> GetAliveEntities();
		Entity GetEntity(int entityId);

	}
}