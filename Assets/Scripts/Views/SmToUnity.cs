using UnityEngine;

public static class SmToUnity {

	public static void FromSM(this RectTransform transform, SimpleMotions.Transform smTransform) {
		var position = new Vector2(smTransform.Position.X, smTransform.Position.Y);
		var scale = new Vector2(smTransform.Scale.Width, smTransform.Scale.Height);
		var rotation = Quaternion.Euler(0.0f, 0.0f, smTransform.Roll.AngleDegrees);

		transform.anchoredPosition = position;
		transform.localScale = scale;
		transform.rotation = rotation;
	}

	internal static Color GetColorFrom(SimpleMotions.Color smColor) {
		return new Color {
			r = smColor.R,
			g = smColor.G,
			b = smColor.B,
			a = smColor.A
		};
	}

}