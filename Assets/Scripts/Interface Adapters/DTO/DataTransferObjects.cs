using static StringExtensions;

/// <summary>
/// DTO stands for "Data Transfer Object" this objects are used instead of the structs declared in SimpleMotions.Internal.Components.cs
/// to maintain view agnostic from domain types. Also this structs are readonly because they're meant to be presented and not mutated.
/// 
/// TL;DR; Use this structs to communicate between View Models and Views.
/// in C# 9.0 or above this could be replaced by records.
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
		public readonly ColorDTO Color;

		public ShapeDTO(string primitiveShape, ColorDTO color) {
			PrimitiveShape = primitiveShape;
			Color = color;
		}
	}

	public readonly struct KeyframeDTO {
		public readonly ComponentDTO ComponentDTO;
		public readonly int EntityId;
		public readonly int Frame;
		public readonly int Ease;

		public KeyframeDTO(int entityId, ComponentDTO componentType, int frame, int ease) {
			EntityId = entityId;
			Frame = frame;
			ComponentDTO = componentType;
			Ease = ease;
		}

		public KeyframeDTO(int id, int frame, ComponentDTO componentType, Internal.Ease ease) {
			EntityId = id;
			Frame = frame;
			ComponentDTO = componentType;
			Ease = (int)ease;
		}
	}

	public readonly struct ColorDTO {
		public readonly float R;
		public readonly float G;
		public readonly float B;
		public readonly float A;

		public ColorDTO(float r, float g, float b, float a) {
			R = r;
			G = g;
			B = b;
			A = a;
		}
	}

	public readonly struct EditorThemeDTO {
		public readonly ColorDTO PrimaryColor;
		public readonly ColorDTO SecondaryColor;
		public readonly ColorDTO BackgroundColor;
		public readonly ColorDTO AccentColor;
		public readonly ColorDTO TextColor;

		public EditorThemeDTO(ColorDTO primary, ColorDTO secondary, ColorDTO background, ColorDTO accent, ColorDTO text) {
			PrimaryColor = primary;
			SecondaryColor = secondary;
			BackgroundColor = background;
			AccentColor = accent;
			TextColor = text;
		}
	}

	public readonly struct VideoExportDTO {
		public readonly int TotalFrames;
		public readonly int TargetFrameRate;
		public readonly string OutputFilepath;
		public readonly string FileName;

		public VideoExportDTO(int totalFrames, int targetFrameRate, string outputFilepath, string fileName) {
			TotalFrames = totalFrames;
			TargetFrameRate = targetFrameRate;
			OutputFilepath = outputFilepath;
			FileName = fileName;
		}
	}

	public enum PrimitiveShapeDTO {
		Rect,
		Circle,
		Arrow,
		Triangle,
		Star,
		Heart,
		Image
	}

	public enum ComponentDTO {
		Transform,
		Shape,
		Text
	}
	
}