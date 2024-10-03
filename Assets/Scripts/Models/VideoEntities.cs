namespace SimpleMotions {

	internal sealed class VideoEntities : IVideoEntities {

		private readonly IComponentStorage _componentStorage;
		private readonly IEntityStorage _entityStorage;
		private readonly VideoCanvas _videoCanvas;

		internal VideoEntities(IComponentStorage componentStorage, IEntityStorage entityStorage, VideoCanvas videoCanvas) {
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;
			_videoCanvas = videoCanvas;
		}

		public void CreateTestEntity() {
			var newEntity = _entityStorage.CreateEntity();

			_componentStorage.AddComponent<Position>(newEntity);
			_componentStorage.AddComponent<Scale>(newEntity);
			_componentStorage.AddComponent<Shape>(newEntity);

			_videoCanvas.UpdateCanvas(newEntity);
		}

	}
}