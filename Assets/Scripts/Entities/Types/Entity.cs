namespace SimpleMotions {

	[System.Serializable]
	public sealed class Entity {
		public const int INVALID_ID = -1;
		
		public bool IsActive;
		public string Name;
		public int Id;
		public int ComponentMask;
	}

}