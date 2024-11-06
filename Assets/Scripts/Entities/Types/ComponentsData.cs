using System.Collections.Generic;

namespace SimpleMotions.Internal {

	[System.Serializable]
	public sealed class ComponentsData {
		public Dictionary<System.Type, Dictionary<int, Component>> Components = new();
		public Dictionary<System.Type, int> ComponentBitmasks = new();
		public int NextComponentBitmask = 1;
	}
}