using UnityEngine;

public static class SmToUnity {

	[System.Serializable]
	public struct EditorThemeUnity {
		public Color TextColor;
		public Color BackgroundColor;
		public Color PrimaryColor;
		public Color SecondaryColor;
		public Color AccentColor;
	}

	public static void FromSmTransform(this RectTransform transform, SimpleMotions.Transform smTransform) {
		var position = new Vector2(smTransform.Position.X, smTransform.Position.Y);
		var scale = new Vector2(smTransform.Scale.Width, smTransform.Scale.Height);
		var rotation = Quaternion.Euler(0.0f, 0.0f, smTransform.Roll.AngleDegrees);

		transform.anchoredPosition = position;
		transform.localScale = scale;
		transform.rotation = rotation;
	}

	public static SimpleMotions.Color UnityToSmColor(UnityEngine.Color unityColor) {
		return new SimpleMotions.Color {
			R = unityColor.r,
			G = unityColor.g,
			B = unityColor.b,
			A = unityColor.a
		};
	}

	public static Color SmToUnityColor(SimpleMotions.Color smColor) {
		return new Color {
			r = smColor.R,
			g = smColor.G,
			b = smColor.B,
			a = smColor.A
		};
	}

	public static SimpleMotions.EditorTheme ToSmTheme(this EditorThemeUnity unityTheme) {
		return new SimpleMotions.EditorTheme {
			TextColor = UnityToSmColor(unityTheme.TextColor),
			BackgroundColor = UnityToSmColor(unityTheme.BackgroundColor),
			PrimaryColor = UnityToSmColor(unityTheme.PrimaryColor),
			SecondaryColor = UnityToSmColor(unityTheme.SecondaryColor),
			AccentColor = UnityToSmColor(unityTheme.AccentColor)
		};
	}

}