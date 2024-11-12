using SimpleMotions.Internal;

public interface ISmParser {

	(float posX, float posY, float width, float height, float angleDeg) DeconstructSmTransform(Transform transform);
	(float r, float g, float b, float a) DeconstructSmColor(Color c);

	((float r, float g, float b, float a) primaryColor,
	(float r, float g, float b, float a) secondaryColor,
	(float r, float g, float b, float a) backgroundColor,
	(float r, float g, float b, float a) accentColor,
	(float r, float g, float b, float a) textColor) 
	DeconstructSmEditorTheme(EditorTheme t);

	Color ConstructSmColor((float r, float g, float b, float a) color);
	
}

public class SmParser : ISmParser {

	public (float posX, float posY, float width, float height, float angleDeg) DeconstructSmTransform(Transform t) {
		return (t.Position.X, t.Position.Y, t.Scale.Width, t.Scale.Height, t.Roll.AngleDegrees);
	}

	public (float r, float g, float b, float a) DeconstructSmColor(Color c) {
		return (c.R, c.G, c.B, c.A);
	}

	public ((float r, float g, float b, float a) primaryColor,
			(float r, float g, float b, float a) secondaryColor,
			(float r, float g, float b, float a) backgroundColor,
			(float r, float g, float b, float a) accentColor,
			(float r, float g, float b, float a) textColor) 
	DeconstructSmEditorTheme(EditorTheme t) {
		return ((t.PrimaryColor.R, t.PrimaryColor.G, t.PrimaryColor.B, t.PrimaryColor.A),
				(t.SecondaryColor.R, t.SecondaryColor.G, t.SecondaryColor.B, t.SecondaryColor.A),
				(t.BackgroundColor.R, t.BackgroundColor.G, t.BackgroundColor.B, t.BackgroundColor.A),
				(t.AccentColor.R, t.AccentColor.G, t.AccentColor.B, t.AccentColor.A),
				(t.TextColor.R, t.TextColor.G, t.TextColor.B, t.TextColor.A));
	}

    public Color ConstructSmColor((float r, float g, float b, float a) color) {
		return new Color(color.r, color.g, color.b, color.a);
    }

}