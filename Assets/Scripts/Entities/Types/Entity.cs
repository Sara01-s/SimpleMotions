namespace SimpleMotions.Internal {

	[System.Serializable]
	public sealed class Entity {
		public static Entity InvalidEntity = new() { Id = INVALID_ID };
		public const int INVALID_ID = -1;
		
		public bool IsActive;
		public string Name;
		public int Id;
		public int ComponentMask;
	}

}