using System.Collections.Generic;

namespace SimpleMotions {

	[System.Serializable]
	public sealed class KeyframesData {

		public Dictionary<System.Type, IKeyframeSpline<Component>> AllKeyframes = new();

	}
}