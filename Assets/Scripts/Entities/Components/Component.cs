namespace SimpleMotions {

	public abstract class Component 
	{
	}

	public class Position : Component {
		public float X;
		public float Y;
	}

	public class Scale : Component {
		public float Width;
		public float Height;
	}

	public class Roll : Component {
		public float Angle;
	}

	public class Keyframe {
		public float Time;
		public float Value;
	}

	public class KeyframeStorage : Component {
		public System.Collections.Generic.List<(Keyframe, Keyframe)> KeyFramePairs;
	}

	public class Color {
		public float R;
		public float G;
		public float B;
		public float A;
	}

	public class Shape : Component {

		public enum Primitive {
			Triangle,
			Circle,
			Rect,
		}

		public Primitive PrimitiveShape;
		public Color color;
	}

	public class Text : Component {
		public string Value;
	}

}