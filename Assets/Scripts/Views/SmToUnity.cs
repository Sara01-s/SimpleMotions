using UnityEngine;
using SimpleMotions.Internal;

public static class SmToUnity {

	[System.Serializable]
	public struct EditorThemeUnity {
		public UnityEngine.Color TextColor;
		public UnityEngine.Color BackgroundColor;
		public UnityEngine.Color PrimaryColor;
		public UnityEngine.Color SecondaryColor;
		public UnityEngine.Color AccentColor;
	}

	public static void FromSmTransform(this RectTransform transform, SimpleMotions.Internal.Transform smTransform) {
		var position = new Vector2(smTransform.Position.X, smTransform.Position.Y);
		var scale = new Vector2(smTransform.Scale.Width, smTransform.Scale.Height);
		var rotation = Quaternion.Euler(0.0f, 0.0f, smTransform.Roll.AngleDegrees);

		transform.anchoredPosition = position;
		transform.localScale = scale;
		transform.rotation = rotation;
	}

	public static SimpleMotions.Internal.Color UnityToSmColor(UnityEngine.Color unityColor) {
		return new SimpleMotions.Internal.Color {
			R = unityColor.r,
			G = unityColor.g,
			B = unityColor.b,
			A = unityColor.a
		};
	}

	public static UnityEngine.Color SmToUnityColor(SimpleMotions.Internal.Color smColor) {
		return new UnityEngine.Color {
			r = smColor.R,
			g = smColor.G,
			b = smColor.B,
			a = smColor.A
		};
	}

	public static UnityEngine.Color ToUnityColor(this SimpleMotions.Internal.Color color) {
		return new UnityEngine.Color {
			r = color.R,
			g = color.G,
			b = color.B,
			a = color.A
		};
	}

	public static EditorTheme ToSmTheme(this EditorThemeUnity unityTheme) {
		return new EditorTheme {
			TextColor = UnityToSmColor(unityTheme.TextColor),
			BackgroundColor = UnityToSmColor(unityTheme.BackgroundColor),
			PrimaryColor = UnityToSmColor(unityTheme.PrimaryColor),
			SecondaryColor = UnityToSmColor(unityTheme.SecondaryColor),
			AccentColor = UnityToSmColor(unityTheme.AccentColor)
		};
		
	}
}