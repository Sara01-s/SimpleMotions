using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class EditorPainter : MonoBehaviour {

	public Image[] _imagesWithPrimaryColor;
	public Image[] _imagesWithSecondaryColor;
	public Image[] _imagesWithAccentColor;
	public Image[] _imagesWithBackgroundColor;
	public TextMeshProUGUI[] _texts;

	public void PaintUI(SmToUnity.EditorThemeUnity editorTheme) {
		PaintImages(_imagesWithPrimaryColor, editorTheme.PrimaryColor);
		PaintImages(_imagesWithSecondaryColor, editorTheme.SecondaryColor);
		PaintImages(_imagesWithAccentColor, editorTheme.AccentColor);
		PaintImages(_imagesWithBackgroundColor, editorTheme.BackgroundColor);

		PaintTexts(_texts, editorTheme.TextColor);
	}

	public void FindUI() {
		_imagesWithAccentColor = GetImagesWithTag("PrimaryColor");
		_imagesWithSecondaryColor = GetImagesWithTag("SecondaryColor");
		_imagesWithAccentColor = GetImagesWithTag("AccentColor");
		_imagesWithBackgroundColor = GetImagesWithTag("BackgroundColor");
		_texts = FindObjectsOfType<TextMeshProUGUI>().ToArray();
	}

	private void PaintImages(Image[] images, Color color) {
		if (images.Length <= 0) {
			return;
		}

		foreach (var image in images) {
			image.color = color;
		}
	}

	private void PaintTexts(TextMeshProUGUI[] texts, Color color) {
		if (texts.Length <= 0) {
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