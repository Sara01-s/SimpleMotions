namespace SimpleMotions {

	[System.Serializable]
	public sealed class EditorData {
		public EditorTheme Theme;
	}

	[System.Serializable]
	public sealed class EditorTheme {
		public Color TextColor;
		public Color BackgroundColor;
		public Color PrimaryColor;
		public Color SecondaryColor;
		public Color AccentColor;
	}

}