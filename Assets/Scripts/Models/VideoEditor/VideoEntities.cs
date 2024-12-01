using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoEntities {

		void CreateTestEntity();
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

		public void CreateTestEntity() {
			var newEntity = _entityStorage.CreateEntity();

			newEntity.Name = "Test Entity";
			_componentStorage.AddComponent<Transform>(newEntity);
			
			var shape = _componentStorage.AddComponent<Shape>(newEntity);

			shape.Color = Color.White;
			shape.PrimitiveShape = Shape.Primitive.Circle;

			_keyframeStorage.AddDefaultKeyframes(newEntity.Id);
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 0   , value: new Transform(new Position(  0.0f , 0.0f ), new Scale(1.0f, 1.0f), new Roll( 0.0f )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 100 , value: new Transform(new Position(  3.0f , 0.0f ), new Scale(-2.0f, 1.0f), new Roll( 270.0f )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 200 , value: new Transform(new Position( -3.0f , 3.0f ), new Scale(1.0f, 4.0f), new Roll( -720.0f )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 300 , value: new Transform(new Position( -2.0f , 6.0f ), new Scale(2.0f, 2.0f), new Roll( 45.0f )));

			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 0  , value: new Shape { Color = Color.Yellow 	});
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 200, value: new Shape { Color = Color.Magenta });
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 250, value: new Shape { Color = Color.Cyan 	});
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 300, value: new Shape { Color = Color.Green   });

			_videoCanvas.DisplayEntity(newEntity);
			_entitySelector.SelectEntity(newEntity.Id);
		}

	}
}