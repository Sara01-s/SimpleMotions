namespace SimpleMotions {

	[System.Serializable]
	public sealed class ComponentsData {
		public SerializableDictionary<System.Type, SerializableDictionary<int, Component>> Components = new();
		public SerializableDictionary<System.Type, int> ComponentBitmasks = new();
		public int NextComponentBitmask = 1;
	}
}