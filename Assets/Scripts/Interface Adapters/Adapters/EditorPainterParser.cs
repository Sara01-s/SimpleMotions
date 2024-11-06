
namespace SimpleMotions {
	
	[System.Serializable]
	public struct EditorThemeUnity {
		public UnityEngine.Color TextColor;
		public UnityEngine.Color BackgroundColor;
		public UnityEngine.Color PrimaryColor;
		public UnityEngine.Color SecondaryColor;
		public UnityEngine.Color AccentColor;
	}

	public interface IEditorPainterParser {

		Internal.EditorTheme EditorThemeUnityToSm(EditorThemeUnity editorThemeUnity);
		EditorThemeUnity SmEditorThemeToUnity(Internal.EditorTheme theme);
		Internal.Color UnityColorToSm(UnityEngine.Color color);
		UnityEngine.Color SmColorToUnity(Internal.Color color);
		
	}

	public class EditorPainterParser : IEditorPainterParser {

		public Internal.EditorTheme EditorThemeUnityToSm(EditorThemeUnity theme) {
			return new Internal.EditorTheme() {
				PrimaryColor = UnityColorToSm(theme.PrimaryColor),
				SecondaryColor = UnityColorToSm(theme.SecondaryColor),
				BackgroundColor = UnityColorToSm(theme.BackgroundColor),
				AccentColor = UnityColorToSm(theme.AccentColor),
				TextColor = UnityColorToSm(theme.TextColor)
			};
		}

		public EditorThemeUnity SmEditorThemeToUnity(Internal.EditorTheme theme) {
			return new EditorThemeUnity {
				PrimaryColor = SmColorToUnity(theme.PrimaryColor),
				SecondaryColor = SmColorToUnity(theme.SecondaryColor),
				BackgroundColor = SmColorToUnity(theme.BackgroundColor),
				AccentColor = SmColorToUnity(theme.AccentColor),
				TextColor = SmColorToUnity(theme.TextColor)
			};
		}

		public Internal.Color UnityColorToSm(UnityEngine.Color color) {
			return new Internal.Color() {
				R = color.r,
				G = color.g,
				B = color.b,
				A = color.a
			};
		}

		public UnityEngine.Color SmColorToUnity(Internal.Color color) {
			return new UnityEngine.Color() {
				r = color.R,
				g = color.G,
				b = color.B,
				a = color.A
			};
		}

	}
}	