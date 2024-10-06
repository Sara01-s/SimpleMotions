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

			_keyframeStorage.AddKeyframe(newEntity, 0.0f, new Position(5.0f, 10.0f));
			_keyframeStorage.AddKeyframe(newEntity, 2.0f, new Position(-5.0f, -8.0f));
			_keyframeStorage.AddKeyframe(newEntity, 1.0f, new Position(-10.0f, -4.0f));


			_videoCanvas.UpdateCanvas(newEntity);
		}

	}
}