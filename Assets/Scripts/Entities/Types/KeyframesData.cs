using System.Collections.Generic;

namespace SimpleMotions.Internal {

	[System.Serializable]
	public sealed class KeyframesData {

		public Dictionary<int, Dictionary<System.Type, IKeyframeSpline>> AllKeyframes = new();

	}
}