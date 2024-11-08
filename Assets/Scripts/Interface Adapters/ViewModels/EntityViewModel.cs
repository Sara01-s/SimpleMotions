using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IEntityViewModel {

		bool EntityHasTransform(int entityId, out ((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData);
		bool EntityHasShape(int entityId, out ((float r, float g, float b, float a) color, string primitiveShape) shapeData);
		bool EntityHasText(int entityId, out string text);
		
	}

	public abstract class EntityViewModel : IEntityViewModel {

		private readonly IVideoCanvas _videoCanvas;

		public EntityViewModel(IVideoCanvas videoCanvas) {
			_videoCanvas = videoCanvas;
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
			if (_videoCanvas.EntityHasComponent<Text>(entityId)) {
				text = default;
				return false;
			}

			text = _videoCanvas.GetEntityComponent<Text>(entityId).Content;
			return true;
		}


	}
}