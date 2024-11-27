using static StringExtensions;

namespace SimpleMotions {

	public readonly struct EntityDTO {
		public readonly int Id;
		public readonly string Name;
		public readonly bool IsActive;

		public EntityDTO(int id, string name = "") : this() {
			Id = id;
			Name = name;
		}
	}

	public readonly struct TransformDTO {
		public readonly (float x, float y) Position;
		public readonly (float w, float h) Scale;
		public readonly float RollDegrees;

		public TransformDTO((float x, float y) position, (float w, float h) scale, float rollDegrees) {
			Position = position;
			Scale = scale;
			RollDegrees = rollDegrees;
		}

		public TransformDTO((string x, string y) position, (string w, string h) scale, string rollDegrees) {
			Position = (ParseFloat(position.x), ParseFloat(position.y));
			Scale = (ParseFloat(scale.w), ParseFloat(scale.h));
			RollDegrees = ParseFloat(rollDegrees);
		}
	}

	public readonly struct ShapeDTO {
		public readonly string PrimitiveShape;
		public readonly (float r, float g, float b, float a) Color;

		public ShapeDTO(string primitiveShape, (float r, float g, float b, float a) color) {
			PrimitiveShape = primitiveShape;
			Color = color;
		}
	}

	public readonly struct KeyframeDTO {
		public readonly ComponentType ComponentType;
		public readonly int Id;
		public readonly int Frame;

		public KeyframeDTO(ComponentType componentType, int id, int frame) {
			ComponentType = componentType;
			Id = id;
			Frame = frame;
		}
	}

	public enum ComponentType {
		Transform,
		Shape,
		Text
	}
	
}