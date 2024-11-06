using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoEntities {

		void CreateTestEntity();

	}

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

		public void CreateEntity() {
			var newEntity = _entityStorage.CreateEntity(name: "entidad xd");
		}

		public void CreateTestEntity() {
			var newEntity = _entityStorage.CreateEntity();

			// Add default keyframes for new entity.
			_keyframeStorage.AddKeyframe(newEntity.Id, new Keyframe<Transform>(newEntity.Id));
			_keyframeStorage.AddKeyframe(newEntity.Id, new Keyframe<Shape>(newEntity.Id));
			_keyframeStorage.AddKeyframe(newEntity.Id, new Keyframe<Text>(newEntity.Id));

			newEntity.Name = "prueba";
			
			var entityTransform = _componentStorage.AddComponent<Transform>(newEntity);
			var shape = _componentStorage.AddComponent<Shape>(newEntity);

			entityTransform.Position = new Position(3.0f, -1.0f);
			
			
			shape.Color = Color.Red;
			shape.PrimitiveShape = Shape.Primitive.Circle;

			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 0   , value: new Transform(new Position(  0.0f   , 0.0f   ), new Scale(1.0f, 1.0f), new Roll( 0.0f )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 100 , value: new Transform(new Position(  200.0f , 0.0f   ), new Scale(1.0f, 1.0f), new Roll( 0.0f )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 200 , value: new Transform(new Position( -200.0f , 0.0f   ), new Scale(1.0f, 1.0f), new Roll( 0.0f )));
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 300 , value: new Transform(new Position( -200.0f , 200.0f ), new Scale(1.0f, 1.0f), new Roll( 0.0f )));

			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 0  , value: new Shape { Color = Color.Yellow });
			_keyframeStorage.AddKeyframe(newEntity.Id, frame: 300, value: new Shape { Color = Color.Blue   });


			_videoCanvas.UpdateCanvas(newEntity);
		}

	}
}