using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoEntities {

		int CreateEntity();
		int TryCreateEntity();
		void DeleteEntity(int entityId);
		
		ReactiveCommand ShowMaxEntitiesWarning { get; }
		ReactiveCommand OnCreateEntity { get; }

	}

	public sealed class VideoEntities : IVideoEntities {

		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IComponentStorage _componentStorage;
		private readonly IEntityStorage _entityStorage;
		private readonly IEntitySelector _entitySelector;
		private readonly IVideoCanvas _videoCanvas;
		
        public ReactiveCommand ShowMaxEntitiesWarning { get; } = new();
		public ReactiveCommand OnCreateEntity { get; } = new();

		private byte _createdEntities;

        public VideoEntities(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, 
							 IEntityStorage entityStorage, IEntitySelector entitySelector, IVideoCanvas videoCanvas) {
			_keyframeStorage = keyframeStorage;
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;
			_entitySelector = entitySelector;
			_videoCanvas = videoCanvas;
		}

		public int TryCreateEntity() {
			if (_createdEntities >= TimelineData.MAX_ALLOWED_ENTITIES) {
				ShowMaxEntitiesWarning.Execute();
				return Entity.Invalid.Id;
			}

			return CreateEntity();
		}

		public int CreateEntity() {
			var entity = _entityStorage.CreateEntity();
			entity.Name = $"Entity ({entity.Id})";
			_createdEntities++;

			_componentStorage.AddComponent<Transform>(entity);
			_componentStorage.AddComponent<Shape>(entity);

			_keyframeStorage.AddDefaultKeyframes(entity.Id);

			_entitySelector.SelectEntity(entity.Id);
			_videoCanvas.DisplayEntity(entity.Id);

			OnCreateEntity.Execute();

			return entity.Id;
		}
		
		public void DeleteEntity(int entityId) {
			if (_createdEntities <= 0) {
				return;
			}

			_videoCanvas.OnEntityRemoved.Execute(entityId);
			_keyframeStorage.DeleteEntityKeyframes(entityId);
			_entityStorage.DeleteEntity(entityId);
			_entitySelector.DeselectEntity();

			_createdEntities--;
		}

	}
}