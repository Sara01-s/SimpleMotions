using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System.Linq;
using TMPro;

public class EditorPainter : MonoBehaviour {

	[field:SerializeField] public EditorThemeUnity Theme { get; private set; }
	[SerializeField] private TMP_FontAsset _font;

	[Header("Color Tags")]
	[SerializeField] private string _primaryColorTag 	 = "PrimaryColor";
	[SerializeField] private string _secondaryColorTag	 = "SecondaryColor";
	[SerializeField] private string _backgroundColorTag  = "BackgroundColor";
	[SerializeField] private string _accentColorTag 	 = "AccentColor";

	[Space(20.0f)]
	[SerializeField] private Image[] _imagesWithPrimaryColor;
	[SerializeField] private Image[] _imagesWithSecondaryColor;
	[SerializeField] private Image[] _imagesWithAccentColor;
	[SerializeField] private Image[] _imagesWithBackgroundColor;
	[SerializeField] private TextMeshProUGUI[] _texts;
	[SerializeField] private TMP_InputField[] _inputFields;

	[Space(20.0f)]
	[SerializeField] private bool _findUIOnConfigure;

	private Image[] _previousImagesWithAccentColor;

	public Color CurrentAccentColor { get; private set; }

	public void Configure() {
		if (_findUIOnConfigure) {
			FindNewUI();
		}

		CurrentAccentColor = Theme.AccentColor;
	}

	private void OnDisable() {
		ClearCache();
	}

	public void FindNewUI() {
		_imagesWithPrimaryColor = GetImagesWithTag(_primaryColorTag);
		_imagesWithSecondaryColor = GetImagesWithTag(_secondaryColorTag);
		_imagesWithBackgroundColor = GetImagesWithTag(_backgroundColorTag);
		_imagesWithAccentColor = GetImagesWithTag(_accentColorTag);
		_texts = FindObjectsOfType<TextMeshProUGUI>(includeInactive: true).ToArray();
		_inputFields = FindObjectsOfType<TMP_InputField>(includeInactive: true).ToArray();
	}

	public void ApplyThemeIfNotEmpty(EditorThemeUnity newTheme, bool checkForNewUI = false) {
		if (checkForNewUI) {
			FindNewUI();
		}
		
		var themeColors = new Color[] {
			newTheme.PrimaryColor, newTheme.SecondaryColor, newTheme.BackgroundColor, 
			newTheme.AccentColor, newTheme.TextColor
		};

		// Editor themes with all colors set on white are considered "empty" :)
		if (themeColors.All(color => color == Color.white)) {
			ApplyTheme(Theme);
		}
		else {
			ApplyTheme(newTheme);
		}
	}

	public void ApplyTheme(EditorThemeUnity theme, bool checkForNewUI = false) {
		if (checkForNewUI) {
			FindNewUI();
		}

		PaintImages(_imagesWithPrimaryColor, theme.PrimaryColor);
		PaintImages(_imagesWithSecondaryColor, theme.SecondaryColor);
		PaintImages(_imagesWithAccentColor, theme.AccentColor);
		PaintImages(_imagesWithBackgroundColor, theme.BackgroundColor);

		PaintTexts(_texts, theme.TextColor);
		PaintInputFields(_inputFields, theme.TextColor, theme.AccentColor);
	}

	private void PaintImages(Image[] images, Color color) {
		if (images == null || images.Length <= 0) {
			return;
		}

		foreach (var image in images) {
			image.color = color;
		}
	}

	private void PaintTexts(TextMeshProUGUI[] texts, Color textColor) {
		if (texts == null || texts.Length <= 0) {
			return;
		}

		foreach (var text in texts) {
			text.color = textColor;
			text.font = _font;
		}
	}

	private void PaintInputFields(TMP_InputField[] inputFields, Color textColor, Color accentColor) {
		if (inputFields == null || inputFields.Length <= 0) {
			return;
		}

		foreach (var inputField in inputFields) {
			inputField.textComponent.color = textColor;
			inputField.selectionColor = accentColor;
		}
	}

	private void ClearCache() {
		ClearArray(_imagesWithPrimaryColor);
		ClearArray(_imagesWithSecondaryColor);
		ClearArray(_imagesWithBackgroundColor);
		ClearArray(_imagesWithAccentColor);
		ClearArray(_texts);
	}

	private static void ClearArray(System.Array array) {
		System.Array.Clear(array, 0, array.Length);
	}

	private Image[] GetImagesWithTag(string tag) {
		return GameObject.FindGameObjectsWithTag(tag)
                         .Where(go => go.TryGetComponent<Image>(out _))
                         .Select(go => go.GetComponent<Image>())
                         .ToArray();
	}

	[ContextMenu("Apply Editor Theme")]
	private void ApplyEditorTheme() {
		FindNewUI();
		ApplyTheme(Theme);
	}

	public void ChangeEditorAccentColor(Color accentColor) {
		if (_previousImagesWithAccentColor == null || _imagesWithAccentColor != _previousImagesWithAccentColor) {
			// Different arrays.
			_imagesWithAccentColor = GetImagesWithTag(_accentColorTag);
		}

		PaintImages(_imagesWithAccentColor, accentColor);
		_previousImagesWithAccentColor = _imagesWithAccentColor;
		CurrentAccentColor = accentColor;
    }

}