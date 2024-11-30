using System.Collections.Generic;
using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IEntityStorage {

		Entity CreateEntity();
		Entity CreateEntity(string name);
		
		void DeleteEntity(int entityId);
		void SetActive(int entityId, bool active);

		bool IsAlive(int entityId);
		IEnumerable<int> GetAliveEntities();
		IEnumerable<int> GetActiveEntities();
		Entity GetEntity(int entityId);
		EntitiesData GetEntitiesData();

	}

	public sealed class EntityStorage : IEntityStorage {

		private readonly Dictionary<int, Entity> _entities = new();
		private readonly List<int> _availableIds = new();
		private readonly HashSet<int> _aliveEntities = new();
		private readonly HashSet<int> _activeEntities = new();
		private int _nextAvailableId = 0;

		public EntityStorage(EntitiesData entitiesData) {
			_nextAvailableId = entitiesData.NextAvailableId;
			_availableIds = entitiesData.AvailableIds;
			_aliveEntities = entitiesData.AliveEntities;
			_activeEntities = entitiesData.ActiveEntities;
			_entities = entitiesData.Entities;
		}

		public EntitiesData GetEntitiesData() {
			return new EntitiesData {
				NextAvailableId = _nextAvailableId,
				ActiveEntities = _activeEntities,
				AliveEntities = _aliveEntities,
				AvailableIds = _availableIds,
				Entities = _entities,
				SelectedEntity = Entity.Invalid
			};
		}

		public Entity CreateEntity(string name) {
			var entity = CreateEntity();
			entity.Name = name;
			
			return entity;
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

			var entity = new Entity { Id = assignedId, ComponentMask = 0, IsActive = true };

			_activeEntities.Add(assignedId);
			_aliveEntities.Add(assignedId);
			_entities.Add(assignedId, entity);

			return entity;
		}

		public void SetActive(int entityId, bool active) {
			SetActive(GetEntity(entityId), active);
		}

		public void SetActive(Entity entity, bool active) {
			entity.IsActive = active;

			if (entity.IsActive) {
				_activeEntities.Add(entity.Id);
			}
			else {
				_activeEntities.Remove(entity.Id);
			}
		}

		public void DeleteEntity(int entityId) {
			if (_aliveEntities.Contains(entityId)) {
				_activeEntities.Remove(entityId);
				_aliveEntities.Remove(entityId);
				_entities.Remove(entityId);

				_availableIds.Add(entityId);
			}
		}

		public bool IsAlive(int entityId) {
			return _aliveEntities.Contains(entityId);
		}

		public IEnumerable<int> GetAliveEntities() {
			return _aliveEntities;
		}

		public IEnumerable<int> GetActiveEntities() {
			return _activeEntities;
		}

		public Entity GetEntity(int entityId) {
			if (entityId <= Entity.Invalid.Id) {
				return Entity.Invalid;
			}

			if (!_entities.TryGetValue(entityId, out var entity)) {
				throw new System.ArgumentException($"Entity with id [{entityId}] not found.");
			}

			return entity;
		}

	}
}