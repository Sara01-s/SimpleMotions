using System.Collections.Generic;

namespace SimpleMotions.Internal {

	public interface IInterpolator {
		bool TryGetInterpolation(System.Type componentType, out IInterpolation interpolation);
	}

	public class Interpolator : IInterpolator {

		public bool TryGetInterpolation(System.Type componentType, out IInterpolation interpolation) {
			return InterpolationStrategies.TryGetValue(componentType, out interpolation);
		}

		private static readonly Dictionary<System.Type, IInterpolation> InterpolationStrategies = new() {
			{ typeof(Transform), new TransformInterpolation() },
			{ typeof(Shape), new ShapeInterpolation() }
		};
	}
}