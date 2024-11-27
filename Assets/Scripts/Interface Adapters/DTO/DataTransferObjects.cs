using static StringExtensions;
using SimpleMotions.Internal;

/// <summary>
/// DTO stands for "Data Transfer Object" this objects are used instead of the structs declared in SimpleMotions.Internal.Components.cs
/// to maintain view agnostic from domain types. Also this structs are readonly because they're meant to be presented and not mutated.
/// 
/// TL;DR; Use this structs to communicate between View Models and Views.
/// </summary>
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
		public readonly ComponentDTO ComponentDTO;
		public readonly int Id;
		public readonly int Frame;
		public readonly int Ease;

		public KeyframeDTO(ComponentDTO componentType, int id, int frame, int ease) {
			ComponentDTO = componentType;
			Id = id;
			Frame = frame;
			Ease = ease;
		}

		public KeyframeDTO(ComponentDTO componentType, int id, int frame, Ease ease) {
			ComponentDTO = componentType;
			Id = id;
			Frame = frame;
			Ease = (int)ease;
		}
	}

	public enum ComponentDTO {
		Transform,
		Shape,
		Text
	}
	
}