using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoEntities {

		void CreateTestEntity();
		void CreateEntity();

	}

	public sealed class VideoEntities : IVideoEntities {

		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IComponentStorage _componentStorage;
		private readonly IEntityStorage _entityStorage;
		private readonly IEntitySelector _entitySelector;
		private readonly IVideoCanvas _videoCanvas;

		public VideoEntities(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, 
							 IEntityStorage entityStorage, IEntitySelector entitySelector, IVideoCanvas videoCanvas) {
			_keyframeStorage = keyframeStorage;
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;
			_entitySelector = entitySelector;
			_videoCanvas = videoCanvas;
		}

		public void CreateEntity() {
			var newEntity = _entityStorage.CreateEntity();

			newEntity.Name = $"Entity ({newEntity.Id})";
			_componentStorage.AddComponent<Transform>(newEntity);
			_keyframeStorage.AddDefaultKeyframes(newEntity.Id);
			_videoCanvas.DisplayEntity(newEntity);

			var shape = _componentStorage.AddComponent<Shape>(newEntity);

			shape.Color = Color.White;
			shape.PrimitiveShape = Shape.Primitive.Circle;

			_entitySelector.SelectEntity(newEntity.Id);
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
		}

	}
	
}