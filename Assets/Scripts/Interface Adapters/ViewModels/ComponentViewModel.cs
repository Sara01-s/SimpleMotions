using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IComponentViewModel {

		bool EntityHasTransform(int entityId);
		bool EntityHasTransform(int entityId, out ((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData);
		bool EntityHasShape(int entityId, out ((float r, float g, float b, float a) color, string primitiveShape) shapeData);
		bool EntityHasText(int entityId, out string text);

		void SetEntityPosition(int entityId, (float x, float y) position);
		void SetEntityScale(int entityId, (float w, float h) scale);
		void SetEntityRoll(int entityId, float angleDegrees);
		void SetEntityShape(int entityId, string shapeName);
		void SetEntityColor(int entityId, Color color);
		
	}

	public abstract class ComponentViewModel : IComponentViewModel {

		private readonly IVideoCanvas _videoCanvas;

		public ComponentViewModel(IVideoCanvas videoCanvas) {
			_videoCanvas = videoCanvas;
		}

		public void SetEntityPosition(int entityId, (float x, float y) position) {
			_videoCanvas.GetEntityComponent<Transform>(entityId).Position = new Position(position.x, position.y);
			_videoCanvas.DisplayEntity(entityId);
		}

		public void SetEntityScale(int entityId, (float w, float h) scale) {
			_videoCanvas.GetEntityComponent<Transform>(entityId).Scale = new Scale(scale.w, scale.h);
			_videoCanvas.DisplayEntity(entityId);
		}

		public void SetEntityRoll(int entityId, float angleDegrees) {
			_videoCanvas.GetEntityComponent<Transform>(entityId).Roll.AngleDegrees = angleDegrees;
			_videoCanvas.DisplayEntity(entityId);
		}

		public void SetEntityColor(int entityId, Color newColor) {
            if (_videoCanvas.EntityHasComponent<Shape>(entityId)) {
                _videoCanvas.GetEntityComponent<Shape>(entityId).Color = newColor;
                _videoCanvas.DisplayEntity(entityId);
            }
        }

		public void SetEntityShape(int entityId, string shapeName) {
			if (System.Enum.TryParse(typeof(Shape.Primitive), shapeName, out var primitiveShape)) {
				_videoCanvas.GetEntityComponent<Shape>(entityId).PrimitiveShape = (Shape.Primitive)primitiveShape;
				_videoCanvas.DisplayEntity(entityId);
			}
			else {
				throw new System.ArgumentException($"\"{shapeName}\" primitive shape does not exist.");
			}
		}

		public bool EntityHasTransform(int entityId) {
			return _videoCanvas.EntityHasComponent<Transform>(entityId);
		}

		public bool EntityHasTransform(int entityId, out ((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData) {
			if (!_videoCanvas.EntityHasComponent<Transform>(entityId)) {
				transformData = default;
				return false;
			}

			var t = _videoCanvas.GetEntityComponent<Transform>(entityId);
			transformData = ((t.Position.X, t.Position.Y), (t.Scale.Width, t.Scale.Height), t.Roll.AngleDegrees);
			return true;
		}

		public bool EntityHasShape(int entityId, out ((float r, float g, float b, float a) color, string primitiveShape) shapeData) {
			if (!_videoCanvas.EntityHasComponent<Shape>(entityId)) {
				shapeData = default;
				return false;
			}

			var s = _videoCanvas.GetEntityComponent<Shape>(entityId);
			shapeData = ((s.Color.R, s.Color.G, s.Color.B, s.Color.A), s.PrimitiveShape.ToString());
			return true;
		}

		public bool EntityHasText(int entityId, out string text) {
			if (!_videoCanvas.EntityHasComponent<Text>(entityId)) {
				text = default;
				return false;
			}

			text = _videoCanvas.GetEntityComponent<Text>(entityId).Content;
			return true;
		}

    }
}