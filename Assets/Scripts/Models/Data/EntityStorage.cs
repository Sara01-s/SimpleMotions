using System.Collections.Generic;

namespace SimpleMotions {

	public sealed class EntityStorage : IEntityStorage {

		private int _nextAvailableId = 0;
		private readonly List<int> _availableIds = new();
		private readonly HashSet<int> _aliveEntities = new();
		private readonly Dictionary<int, Entity> _entities = new();

		public EntityStorage(EntitiesData entitiesData) {
			_nextAvailableId = entitiesData.NextAvailableId;
			_availableIds = entitiesData.AvailableIds;
			_aliveEntities = entitiesData.AliveEntities;
			_entities = entitiesData.Entities;
		}

		public Entity CreateEntity() {
			int assignedId;

			if (_availableIds.Count > 0) {
				int lastIdIndex = _availableIds.Count - 1;

				assignedId = _availableIds[lastIdIndex];
				_availableIds.RemoveAt(lastIdIndex);
			}
			else {
				assignedId = _nextAvailableId;
				_nextAvailableId++;
			}

			var entity = new Entity { Id = assignedId, ComponentMask = 0 };

			_aliveEntities.Add(assignedId);
			_entities[assignedId] = entity;

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

		public Entity GetEntity(int id) {
			return _entities[id];
		}
	}

}