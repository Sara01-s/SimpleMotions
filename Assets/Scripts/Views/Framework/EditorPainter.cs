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

	public void Awake() {
		FindUI();
	}

	public void FindUI() {
		_imagesWithPrimaryColor = GetImagesWithTag(_primaryColorTag);
		_imagesWithSecondaryColor = GetImagesWithTag(_secondaryColorTag);
		_imagesWithBackgroundColor = GetImagesWithTag(_backgroundColorTag);
		_imagesWithAccentColor = GetImagesWithTag(_accentColorTag);
		_texts = FindObjectsOfType<TextMeshProUGUI>(includeInactive: true).ToArray();
	}

	public void ApplyThemeIfNotEmpty(EditorThemeUnity newTheme) {
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

	public void ApplyTheme(EditorThemeUnity theme) {
		PaintImages(_imagesWithPrimaryColor, theme.PrimaryColor);
		PaintImages(_imagesWithSecondaryColor, theme.SecondaryColor);
		PaintImages(_imagesWithAccentColor, theme.AccentColor);
		PaintImages(_imagesWithBackgroundColor, theme.BackgroundColor);

		PaintTexts(_texts, theme.TextColor);
	}

	private void PaintImages(Image[] images, Color color) {
		if (images == null || images.Length <= 0) {
			return;
		}

		foreach (var image in images) {
			image.color = color;
		}
	}

	private void PaintTexts(TextMeshProUGUI[] texts, Color color) {
		if (texts == null || texts.Length <= 0) {
			return;
		}

		foreach (var text in texts) {
			text.color = color;
			text.font = _font;
		}
	}

	private Image[] GetImagesWithTag(string tag) {
		return GameObject.FindGameObjectsWithTag(tag)
                         .Where(go => go.TryGetComponent<Image>(out _))
                         .Select(go => go.GetComponent<Image>())
                         .ToArray();
	}

	[ContextMenu("Apply Editor Theme")]
	private void ApplyEditorTheme() {
		FindUI();
		ApplyTheme(Theme);
	}
	
}