namespace SimpleMotions {

	public sealed class VideoEntities : IVideoEntities {

		private readonly IKeyframeStorage _keyframeStorage;
		private readonly IComponentStorage _componentStorage;
		private readonly IEntityStorage _entityStorage;
		private readonly VideoCanvas _videoCanvas;

		public VideoEntities(IKeyframeStorage keyframeStorage, IComponentStorage componentStorage, IEntityStorage entityStorage, VideoCanvas videoCanvas) {
			_keyframeStorage = keyframeStorage;
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;
			_videoCanvas = videoCanvas;
		}

		public void CreateTestEntity() {
			var newEntity = _entityStorage.CreateEntity();

			newEntity.Name = "prueba";
			
			var entityTransform = _componentStorage.AddComponent<Transform>(newEntity);
			var shape = _componentStorage.AddComponent<Shape>(newEntity);

			entityTransform.Position = new Position(3.0f, -1.0f);
			
			shape.Color = Color.Red;
			shape.PrimitiveShape = Shape.Primitive.Circle;

			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 0  , value: new Transform(new Position( 0.0f  ,  0.0f  )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 20 , value: new Transform(new Position( 300.0f,  0.0f  )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 50 , value: new Transform(new Position( 0.0f  ,  200.0f)));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 100, value: new Transform(new Position( 0.0f  , -200.0f)));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 150, value: new Transform(new Position(-300.0f, -150.0f)));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 200, value: new Transform(new Position(-100.0f,  200.0f)));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 250, value: new Transform(new Position( 100.0f, -200.0f)));

			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 0, value: new Shape { Color = Color.Yellow });
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 100, value: new Shape { Color = Color.Blue });


			_videoCanvas.UpdateCanvas(newEntity);
		}

	}
}