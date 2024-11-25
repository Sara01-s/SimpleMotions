using static SimpleMotions.SmMath;

namespace SimpleMotions.Internal {

	public interface IInterpolation {
		void Interpolate(Component component, Component start, Component target, float t, Ease ease);
	}

	public class TransformInterpolation : IInterpolation {

    	public void Interpolate(Component component, Component start, Component target, float t, Ease ease) {
			t = Easing.Evaluate(t, ease);

			if (component is not Transform transform 
				|| start  is not Transform startTransform 
				|| target is not Transform targetTransform) 
			{
				throw new System.ArgumentException("Invalid component types for Transform interpolation.");
			}

			transform.Position = lerp(startTransform.Position, targetTransform.Position, t);
			transform.Scale = lerp(startTransform.Scale, targetTransform.Scale, t);
			transform.Roll.AngleDegrees = lerp(startTransform.Roll.AngleDegrees, targetTransform.Roll.AngleDegrees, t);
		}

	}

	public class ShapeInterpolation : IInterpolation {

		public void Interpolate(Component component, Component start, Component target, float t, Ease ease) {
			t = Easing.Evaluate(t, ease);

			if (component is not Shape shape 
				|| start  is not Shape startShape 
				|| target is not Shape targetShape)
			{
				throw new System.ArgumentException("Invalid component types for Shape interpolation.");
			}

			shape.Color = lerp(startShape.Color, targetShape.Color, t);
			shape.PrimitiveShape = startShape.PrimitiveShape; // Immediate switch.
		}

	}
}