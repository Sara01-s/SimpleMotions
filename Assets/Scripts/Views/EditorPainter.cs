using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using SimpleMotions;

public class EditorPainter : MonoBehaviour {

	[field:SerializeField] public EditorThemeUnity Theme { get; private set; }

	[Header("Color Tags")]
	[SerializeField] private string _primaryColorTag 	 = "PrimaryColor";
	[SerializeField] private string _secondaryColorTag	 = "SecondaryColor";
	[SerializeField] private string _backgroundColorTag  = "BackgroundColor";
	[SerializeField] private string _accentColorTag 	 = "AccentColor";

	private Image[] _imagesWithPrimaryColor;
	private Image[] _imagesWithSecondaryColor;
	private Image[] _imagesWithAccentColor;
	private Image[] _imagesWithBackgroundColor;
	private TextMeshProUGUI[] _texts;

	public void Awake() {
		FindUI();
	}

	public void FindUI() {
		_imagesWithAccentColor = GetImagesWithTag(_primaryColorTag);
		_imagesWithSecondaryColor = GetImagesWithTag(_secondaryColorTag);
		_imagesWithBackgroundColor = GetImagesWithTag(_backgroundColorTag);
		_imagesWithAccentColor = GetImagesWithTag(_accentColorTag);
		_texts = FindObjectsOfType<TextMeshProUGUI>().ToArray();
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
		}
	}

	private Image[] GetImagesWithTag(string tag) {
		return GameObject.FindGameObjectsWithTag(tag)
                         .Where(go => go.TryGetComponent<Image>(out _))
                         .Select(go => go.GetComponent<Image>())
                         .ToArray();
	}
}