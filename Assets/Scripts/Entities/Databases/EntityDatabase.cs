using System.Collections.Generic;

namespace SimpleMotions {

	internal sealed class EntityDatabase : IEntityDatabase {

		private int _nextId = 0;
		private readonly List<int> _availableIds = new();
		private readonly HashSet<int> _aliveEntities = new();
		private readonly Dictionary<int, Entity> _entities = new();

		internal EntityDatabase() {
			Services.Instance.RegisterService<IEntityDatabase>(service: this);
		}

		public Entity CreateEntity() {

			int id;

			if (_availableIds.Count > 0) {
				int lastIdIndex = _availableIds.Count - 1;

				id = _availableIds[lastIdIndex];
				_availableIds.RemoveAt(lastIdIndex);
			}
			else {
				id = _nextId++;
			}

			var entity = new Entity { Id = id, ComponentMask = 0 };

			_aliveEntities.Add(id);
			_entities[id] = entity;

			return entity;
		}

		public void DestroyEntity(Entity entity) {
			
			if (_aliveEntities.Contains(entity.Id)) {
				_aliveEntities.Remove(entity.Id);
				_availableIds.Add(entity.Id);
				_entities.Remove(entity.Id);
			}
		}

		public bool IsAlive(Entity entity) {
			return _aliveEntities.Contains(entity.Id);
		}

		public IEnumerable<int> GetAliveEntities() {
			return _aliveEntities;
		}

		public void UpdateComponentMask(ref Entity entity, int componentBitmask) {
			entity.ComponentMask |= componentBitmask;
			_entities[entity.Id] = entity;
		}

		public void RemoveComponentMask(ref Entity entity, int componentBitmask) {
			entity.ComponentMask &= ~componentBitmask;
			_entities[entity.Id] = entity;
		}

		public Entity GetEntity(int id) {
			return _entities[id];
		}

		public void Dispose() {
			Services.Instance.UnRegisterService<IEntityDatabase>();
		}
	}

}