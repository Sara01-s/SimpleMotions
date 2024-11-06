namespace SimpleMotions.Internal {

	[System.Serializable]
	public sealed class EditorData {
		public EditorTheme Theme = new();
	}

	[System.Serializable]
	public sealed class EditorTheme {
		public static EditorTheme Empty = new();

		public Color TextColor = Color.White;
		public Color BackgroundColor = Color.White;
		public Color PrimaryColor = Color.White;
		public Color SecondaryColor = Color.White;
		public Color AccentColor = Color.White;

		public EditorTheme() {}

		public EditorTheme(Color text, Color bg, Color primary, Color secondary, Color accent) {
			TextColor = text;
			BackgroundColor = bg;
			PrimaryColor = primary;
			SecondaryColor = secondary;
			AccentColor = accent;
		}

#if DEBUG
		public override string ToString() {
			return $"Text: {TextColor}, BG: {BackgroundColor}, Primary: {PrimaryColor}, Secondary: {SecondaryColor}, Accent: {AccentColor}";
		}
#endif

		public static bool operator==(EditorTheme theme, EditorTheme otherTheme) {
			return theme.TextColor == otherTheme.TextColor && theme.BackgroundColor == otherTheme.BackgroundColor
					&& theme.PrimaryColor == otherTheme.PrimaryColor && theme.SecondaryColor == otherTheme.SecondaryColor
					&& theme.AccentColor == otherTheme.AccentColor;
		}

		public static bool operator!=(EditorTheme theme, EditorTheme otherTheme) {
			return theme.TextColor != otherTheme.TextColor || theme.BackgroundColor != otherTheme.BackgroundColor
					|| theme.PrimaryColor != otherTheme.PrimaryColor || theme.SecondaryColor != otherTheme.SecondaryColor
					|| theme.AccentColor != otherTheme.AccentColor;
		}

		public override bool Equals(object obj) {
			if (obj is not EditorTheme otherTheme) {
				return false;
			}

			return this == otherTheme;
		}

		public override int GetHashCode() {
			return base.GetHashCode();
		}
	}

}