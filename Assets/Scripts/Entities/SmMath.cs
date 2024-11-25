using static System.Math;
using System.Runtime.CompilerServices;
using SimpleMotions.Internal;

namespace SimpleMotions {

	public static class SmMath {

		private const MethodImplOptions INLINE = MethodImplOptions.AggressiveInlining;
		
		public const float PI = (float)System.Math.PI;
		public const float Deg2Rad = PI / 180.0F;

		[MethodImpl(INLINE)]
		public static float easeOutBack(float t) {
			const float smooth = 1.70158f;
			const float ending = smooth + 1.0f;

			return 1.0f + ending * pow(t - 1.0f, 3.0f) + smooth * pow(t - 1.0f, 2.0f);
		}
		
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
		public static float cos(float x) {
			return (float)Cos(x);
		}

		[MethodImpl(INLINE)]
		public static float sin(float x) {
			return (float)Sin(x);
		}

		[MethodImpl(INLINE)]
		public static float sqrt(float x) {
			return (float)Sqrt(x);
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

		[MethodImpl(INLINE)]
		public static float pow(float x, float power) {
			return (float)Pow(x, power);
		}

		[MethodImpl(INLINE)]
		public static ((float x, float y) min, (float x, float y) max) calculateBounds((float x, float y) pos, (float w, float h) scale, float angleDegrees) {
			var halfScale = new Scale(scale.w / 2.0f, scale.h / 2.0f);
			var localCorners = new Position[] {
				new(-halfScale.Width , -halfScale.Height),
				new( halfScale.Width , -halfScale.Height),
				new(-halfScale.Width ,  halfScale.Height),
				new( halfScale.Width ,  halfScale.Height),
			};

			// Calculate corners rotation
			float angleRadians = angleDegrees * Deg2Rad;
			float cosa = (float)Cos(angleRadians);
			float sina = (float)Sin(angleRadians);
			var rotatedCorners = new Position[4];

			for (int i = 0; i < localCorners.Length; i++) {
				var corner = localCorners[i];

				rotatedCorners[i] = new Position (
					corner.X * cosa - corner.Y * sina,
					corner.X * sina + corner.Y * cosa
				);

				rotatedCorners[i].X += pos.x;
				rotatedCorners[i].Y += pos.y;
			}

			// Calculate AABB corners without rotation
			var minCorner = rotatedCorners[0];
			var maxCorner = rotatedCorners[0];

			for (int i = 1; i < rotatedCorners.Length; i++) {
				minCorner = min(minCorner, rotatedCorners[i]);
				maxCorner = max(maxCorner, rotatedCorners[i]);
			}

			return ((minCorner.X, minCorner.Y), (maxCorner.X, maxCorner.Y));
		}

		[MethodImpl(INLINE)]
		public static Position min(Position a, Position b) {
			return new Position() {
				X = Min(a.X, b.X),
				Y = Min(a.X, b.X)
			};
		}

		[MethodImpl(INLINE)]
		public static Position max(Position a, Position b) {
			return new Position() {
				X = Max(a.X, b.X),
				Y = Max(a.X, b.X)
			};
		}

	}
}