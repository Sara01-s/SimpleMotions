namespace SimpleMotions {

	public struct EntityDTO {
		public int Id;
		public string Name;
		public bool IsActive;
	}

	public struct TransformDTO {
		public (float x, float y) Position;
		public (float w, float h) Scale;
		public float RollDegrees;
	}

	public struct ShapeDTO {
		public string PrimitiveShape;
		public (float r, float g, float b, float a) Color;
	}
}