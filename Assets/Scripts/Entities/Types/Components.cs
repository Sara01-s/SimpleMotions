using System;

namespace SimpleMotions.Internal {

	[Serializable]
	public class Component {
		public Component() {}
	}

	public sealed class Transform : Component {
		public Position Position = new();
		public Scale Scale = new();
		public Roll Roll = new();

		public Transform() {}

		public Transform(Position position, Scale scale, Roll roll) {
			Position = position;
			Scale = scale;
			Roll = roll;	
		}

		public Transform(Position position, Scale scale) {
			Position = position;
			Scale = scale;
		}

		public Transform(Scale scale, Roll roll) {
			Scale = scale;
			Roll = roll;
		}

		public Transform(Position position, Roll roll) {
			Position = position;
			Roll = roll;
		}

		public Transform(Position position) {
			Position = position;
		}

		public Transform(Scale scale) {
			Scale = scale;
		}

		public Transform(Roll roll) {
			Roll = roll;
		}

#if DEBUG
        public override string ToString() {
			return $"Transform (Position: {Position}, Scale {Scale}, Roll {Roll})";
        }
#endif
    }

	public sealed class Position {
		public static Position Zero = new(0.0f, 0.0f);
		public static Position One = new(1.0f, 1.0f);
		public float X = 0.0f;
		public float Y = 0.0f;

		public Position() {}

		public Position(float x, float y) {
			X = x;
			Y = y;
		}

		public static Position operator +(Position a, Position b) {
			return new Position(a.X + b.X, a.Y + b.Y);
		}

#if DEBUG
		public override string ToString() {
			return $"({X}, {Y})";
		}
#endif
	}

	public sealed class Scale {
		public static Scale Zero = new(0.0f, 0.0f);
		public static Scale One = new(1.0f, 1.0f);
		public float Width = 1.0f;
		public float Height = 1.0f;

		public Scale() {}

		public Scale(float width, float height) {
			Width = width;
			Height = height;
		}

		
		public static Scale operator +(Scale a, Scale b) {
			return new Scale(a.Width + b.Width, a.Height + b.Height);
		}

#if DEBUG
		public override string ToString() {
			return $"({Width}, {Height})";
		}
#endif
	}

	public sealed class Roll {
		public float AngleDegrees = 0.0f;

		public Roll() {}

		public Roll(float angleDegrees) {
			AngleDegrees = angleDegrees;
		}

#if DEBUG
		public override string ToString() {
			return $"({AngleDegrees})";
		}
#endif
	}

	[Serializable]
	public sealed class Color {
		public static Color White = new(1.0f, 1.0f, 1.0f, 1.0f);
		public static Color Black = new(0.0f, 0.0f, 0.0f, 1.0f);
		public static Color Clear = new(0.0f, 0.0f, 0.0f, 0.0f);

		public static Color Red 	= new(1.0f, 0.0f, 0.0f, 1.0f);
		public static Color Green 	= new(0.0f, 1.0f, 0.0f, 1.0f);
		public static Color Blue 	= new(0.0f, 0.0f, 1.0f, 1.0f);
		public static Color Cyan 	= new(0.0f, 1.0f, 1.0f, 1.0f);
		public static Color Magenta = new(1.0f, 0.0f, 1.0f, 1.0f);
		public static Color Yellow 	= new(1.0f, 1.0f, 0.0f, 1.0f);

		public float R = 1.0f;
		public float G = 1.0f;
		public float B = 1.0f;
		public float A = 1.0f;

		public Color() {}
		
		public Color(float r, float g, float b, float a) {
			R = r;
			G = g;
			B = b;
			A = a;
		}

		public static bool operator==(Color color, Color otherColor) {
			return color.R == otherColor.R && color.G == otherColor.G && color.B == otherColor.B && color.A == otherColor.A;
		}

		public static bool operator!=(Color color, Color otherColor) {
			return color.R != otherColor.R || color.G != otherColor.G || color.B != otherColor.B || color.A != otherColor.A;
		}

		public override bool Equals(object obj) {
			if (obj is not Color otherColor) {
				return false;
			}

			return this == otherColor;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}

#if DEBUG
		public override string ToString() {
			return $"({R}, {G}, {B}, {A})";
		}
#endif
	}

	public sealed class Shape : Component {

		public enum Primitive {
			Rect,
			Circle,
			Arrow,
			Triangle,
			Star,
			Heart,
			Image
		}

		public Primitive PrimitiveShape = Primitive.Rect;
		public Color Color = new();

        public object V { get; }

        public Shape() {}

        public Shape(Primitive primitiveShape, Color color) {
			PrimitiveShape = primitiveShape;
            Color = color;
        }

#if DEBUG
        public override string ToString() {
			return $"Shape: (Primitive: {PrimitiveShape}, Color: {Color})";
		}
#endif
	}

	public sealed class Text : Component {
		public string Content = string.Empty;

		public Text() {}

#if DEBUG
		public override string ToString() {
			return $"Text: (Content: {Content})";
		}
#endif
	}

}