using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoCanvas {

		void DisplayEntity(Entity entity);
		void DisplayEntity(int entityId);
		void SetBackgroundColor(Color color);
		bool EntityHasComponent<T>(int entityId) where T : Component;
		T GetEntityComponent<T>(int entityId) where T : Component;
		Component GetEntityComponent(System.Type componentType, int entityId);
		ReactiveValue<Entity> EntityDisplayInfo { get; }
		ReactiveCommand<int> OnEntityRemoved { get; }
		ReactiveCommand<int, string> OnSetEntityImage { get; }

	}

	public sealed class VideoCanvas : IVideoCanvas {

		private readonly IComponentStorage _componentStorage;
		private readonly IEntityStorage _entityStorage;

        public ReactiveValue<Entity> EntityDisplayInfo { get; } = new();
		public ReactiveCommand<int> OnEntityRemoved { get; } = new();
		public ReactiveCommand<int, string> OnSetEntityImage { get; } = new();

		private readonly VideoData _videoData;

        public VideoCanvas(IComponentStorage componentStorage, IEntityStorage entityStorage, VideoData videoData) {
			_componentStorage = componentStorage;
			_entityStorage = entityStorage;
			_videoData = videoData;

			OnSetEntityImage.Subscribe(DisplayEntityImage);
		}

		public void DisplayEntityImage(int entityId, string imageFilepath) {
			GetEntityComponent<Shape>(entityId).PrimitiveShape = Shape.Primitive.Image;
		}

		public bool EntityHasComponent<T>(int entityId) where T : Component {
			return _componentStorage.HasComponent<T>(entityId);
		}

		public Component GetEntityComponent(System.Type componentType, int entityId) {
			return _componentStorage.GetComponent(componentType, entityId);
		}

		public T GetEntityComponent<T>(int entityId) where T : Component {
			return _componentStorage.GetComponent<T>(entityId);
		}

		public void DisplayEntity(int entityId) {
			var entity = _entityStorage.GetEntity(entityId);
			DisplayEntity(entity);
		}

		public void DisplayEntity(Entity entity) {
			EntityDisplayInfo.Value = entity;
		}

        public void SetBackgroundColor(Color color) {
			_videoData.CanvasBackgroundColor = color;
        }

    }
}