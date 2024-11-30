using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface ISmParser {

		TransformDTO DeconstructSmTransform(Transform transform);
		EditorThemeDTO DeconstructSmEditorTheme(EditorTheme t);
		
	}

	public class SmParser : ISmParser {

		public TransformDTO DeconstructSmTransform(Transform t) {
			return new TransformDTO (
				(t.Position.X, t.Position.Y),
				(t.Scale.Width, t.Scale.Height),
				t.Roll.AngleDegrees	
			);
		}

		public EditorThemeDTO DeconstructSmEditorTheme(EditorTheme t) {
			var primary = new ColorDTO(t.PrimaryColor.R, t.PrimaryColor.G, t.PrimaryColor.B, t.PrimaryColor.A);
			var secondary = new ColorDTO(t.SecondaryColor.R, t.SecondaryColor.G, t.SecondaryColor.B, t.SecondaryColor.A);
			var background = new ColorDTO(t.BackgroundColor.R, t.BackgroundColor.G, t.BackgroundColor.B, t.BackgroundColor.A);
			var accent = new ColorDTO(t.AccentColor.R, t.AccentColor.G, t.AccentColor.B, t.AccentColor.A);
			var text = new ColorDTO(t.TextColor.R, t.TextColor.G, t.TextColor.B, t.TextColor.A);

			return new EditorThemeDTO(primary, secondary, background, accent, text);
		}

	}
}