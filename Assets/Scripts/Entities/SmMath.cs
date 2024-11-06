using static System.Math;
using System.Runtime.CompilerServices;
using SimpleMotions.Internal;

namespace SimpleMotions {

	public static class SmMath {

		private const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;
		
		[MethodImpl(INLINE)]
		public static float lerp(float a, float b, float t) {
			return a * (1.0f - t) + b * t;
		}

		[MethodImpl(INLINE)]
		public static Color lerp(Color c, Color d, float t) {
			float dr = lerp(c.R, d.R, t);
			float dg = lerp(c.G, d.G, t);
			float db = lerp(c.B, d.B, t);
			float da = lerp(c.A, d.A, t);

			return new Color(dr, dg, db, da);
		}

		[MethodImpl(INLINE)]
		public static Position lerp(Position u, Position v, float t) {
			float dx = lerp(u.X, v.X, t);
			float dy = lerp(u.Y, v.Y, t);

			return new Position(dx, dy);
		}

		[MethodImpl(INLINE)]
		public static Scale lerp(Scale u, Scale v, float t) {
			float dw = lerp(u.Width, v.Width, t);
			float dh = lerp(u.Height, v.Height, t);

			return new Scale(dw, dh);
		}

		[MethodImpl(INLINE)]
		public static float clamp(float v, float min, float max) {
			return Clamp(v, min, max);
		}

		[MethodImpl(INLINE)]
		public static float min(float a, float b) {
			return Min(a, b);
		}

		[MethodImpl(INLINE)]
		public static float max(float a, float b) {
			return Max(a, b);
		}

		[MethodImpl(INLINE)]
		public static int min(int a, int b) {
			return Min(a, b);
		}

		[MethodImpl(INLINE)]
		public static int max(int a, int b) {
			return Max(a, b);
		}

	}
}