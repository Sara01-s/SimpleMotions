using System;

namespace SimpleMotions {

	[Serializable]
	public abstract class Component 
	{
	}

	public sealed class Position : Component {
		public float X;
		public float Y;
	}

	public sealed class Scale : Component {
		public float Width;
		public float Height;
	}

	public sealed class Roll : Component {
		public float Angle;
	}

	[Serializable]
	public sealed class Color {
		public float R;
		public float G;
		public float B;
		public float A;
	}

	public sealed class Shape : Component {

		public enum Primitive {
			Triangle,
			Circle,
			Rect,
		}

		public Primitive PrimitiveShape;
		public Color color;
	}

	public sealed class Text : Component {
		public string Value;
	}

}