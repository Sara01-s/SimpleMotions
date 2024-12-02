namespace SimpleMotions.Internal {

	[System.Serializable]
	public sealed class Entity {
		public readonly static Entity Invalid = new() { Id = INVALID_ID };
		private const int INVALID_ID = -1;
		
		// Do not use in a for loop (DOD) Use a bitmasks instead.
		// ref: https://www.youtube.com/watch?v=WDIkqP4JbkE&t=2681s
		public bool IsActive;
		public string Name;
		public int Id;
		public int ComponentMask;

#if DEBUG
		public override string ToString() {
			return $"({Name} [{Id}])";
		}
#endif
	}

}