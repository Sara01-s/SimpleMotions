using SimpleMotions.Internal;
using System.Linq.Expressions;
using System;

namespace SimpleMotions {

	public interface IComponentViewModel {

		bool EntityHasTransform(int entityId);
		bool TryGetEntityTransform(int entityId, out TransformDTO transformDTO);
		bool TryGetEntityShape(int entityId, out ShapeDTO shapeDTO);
		bool TryGetEntityText(int entityId, out string content);

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

		public void SetEntityComponentProperty<TComponent, TProperty>(int entityId, Expression<Func<TComponent, TProperty>> propertySelector, TProperty value) where TComponent : Component {
			var component = _videoCanvas.GetEntityComponent<TComponent>(entityId);

			if (propertySelector.Body is MemberExpression memberExpr) {
				var componentPropertyInfo = typeof(TComponent).GetProperty(memberExpr.Member.Name)
					   					 ?? throw new Exception("Failed to set property: " + memberExpr.Member.Name);

				componentPropertyInfo.SetValue(component, value);
			}
			else {
				throw new ArgumentException("Expression must be a property.");
			}

			_videoCanvas.DisplayEntity(entityId);
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
			if (Enum.TryParse(typeof(Shape.Primitive), shapeName, out var primitiveShape)) {
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

		public bool TryGetEntityTransform(int entityId, out TransformDTO transformDTO) {
			if (!_videoCanvas.EntityHasComponent<Transform>(entityId)) {
				transformDTO = default;
				return false;
			}

			var transform = _videoCanvas.GetEntityComponent<Transform>(entityId);

			transformDTO = new TransformDTO (
				position: (transform.Position.X, transform.Position.Y),
				scale: (transform.Scale.Width, transform.Scale.Height),
				rollDegrees: transform.Roll.AngleDegrees
			);

			return true;
		}

		public bool TryGetEntityShape(int entityId, out ShapeDTO shapeDTO) {
			if (!_videoCanvas.EntityHasComponent<Shape>(entityId)) {
				shapeDTO = default;
				return false;
			}

			var shape = _videoCanvas.GetEntityComponent<Shape>(entityId);

			shapeDTO = new ShapeDTO (
				primitiveShape: shape.PrimitiveShape.ToString(),
				color: new ColorDTO(shape.Color.R, shape.Color.G, shape.Color.B, shape.Color.A)
			);

			return true;
		}

		public bool TryGetEntityText(int entityId, out string content) {
			if (!_videoCanvas.EntityHasComponent<Text>(entityId)) {
				content = default;
				return false;
			}

			content = _videoCanvas.GetEntityComponent<Text>(entityId).Content;
			return true;
		}

    }
}