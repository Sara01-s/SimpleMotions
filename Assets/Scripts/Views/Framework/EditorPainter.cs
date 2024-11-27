using System.Collections.Generic;
using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

public class EditorPainter : MonoBehaviour {

	// TODO - Dejar chance a sus victimas

	public Action<Color> OnAccentColorUpdate;

	[field:SerializeField] public EditorThemeUnity Theme { get; private set; }
	[SerializeField] private TMP_FontAsset _font;

	[Header("Color Tags")]
	[SerializeField] private string _primaryColorTag 	 = "PrimaryColor";
	[SerializeField] private string _secondaryColorTag	 = "SecondaryColor";
	[SerializeField] private string _backgroundColorTag  = "BackgroundColor";
	[SerializeField] private string _accentColorTag 	 = "AccentColor";
	[SerializeField] private string _excludeFromThemeTag = "ExcludeFromTheme";

	[Space(20.0f)]
	[SerializeField] private List<Image> _imagesWithPrimaryColor;
	[SerializeField] private List<Image> _imagesWithSecondaryColor;
	[SerializeField] private List<Image> _imagesWithAccentColor;
	[SerializeField] private List<Image> _imagesWithBackgroundColor;
	[SerializeField] private List<TextMeshProUGUI> _texts;
	[SerializeField] private List<TMP_InputField> _inputFields;

	[Space(20.0f)]
	[SerializeField] private bool _findUIOnConfigure;

	private List<Image> _previousImagesWithAccentColor;

	public Color CurrentAccentColor { get; private set; }

	public void Configure() {
		if (_findUIOnConfigure) {
			FindNewUI();
		}

		CurrentAccentColor = Theme.AccentColor;
		OnAccentColorUpdate?.Invoke(CurrentAccentColor);
	}

	private void OnDisable() {
		_imagesWithPrimaryColor.Clear();
		_imagesWithSecondaryColor.Clear();
		_imagesWithAccentColor.Clear();
		_imagesWithBackgroundColor.Clear();
		_texts.Clear();

		_previousImagesWithAccentColor?.Clear();
		_previousImagesWithAccentColor = null;
	}

	public void FindNewUI()
	{
		_imagesWithPrimaryColor = GetImagesWithTag(_primaryColorTag);
		_imagesWithSecondaryColor = GetImagesWithTag(_secondaryColorTag);
		_imagesWithBackgroundColor = GetImagesWithTag(_backgroundColorTag);
		_imagesWithAccentColor = GetImagesWithTag(_accentColorTag);
		_texts = GetTexts();
		_inputFields = FindObjectsOfType<TMP_InputField>(includeInactive: true).ToList();
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

	private void PaintImages(IEnumerable<Image> images, Color color) {
		if (images == null || images.Count() <= 0) {
			return;
		}

		foreach (var image in images) {
			image.color = color;
		}
	}

	private void PaintTexts(IEnumerable<TextMeshProUGUI> texts, Color textColor) {
		if (texts == null || texts.Count() <= 0) {
			return;
		}

		foreach (var text in texts) {
			text.color = textColor;
			text.font = _font;
		}
	}

	private void PaintInputFields(IEnumerable<TMP_InputField> inputFields, Color textColor, Color accentColor) {
		if (inputFields == null || inputFields.Count() <= 0) {
			return;
		}

		foreach (var inputField in inputFields) {
			inputField.textComponent.color = textColor;
			inputField.selectionColor = accentColor;
		}
	}

	private List<TextMeshProUGUI> GetTexts() {
		return FindObjectsOfType<TextMeshProUGUI>(includeInactive: true)
				.Where(text => !text.CompareTag(_excludeFromThemeTag)).ToList();
	}

	private List<Image> GetImagesWithTag(string tag) {
		return GameObject.FindGameObjectsWithTag(tag)
                         .Where(go => go.TryGetComponent<Image>(out _))
                         .Select(go => go.GetComponent<Image>()).ToList();
	}

	[ContextMenu("Apply Editor Theme")]
	private void ApplyEditorTheme() {
		FindNewUI();
		ApplyTheme(Theme);
	}

	public void ChangeEditorAccentColor(Color newAccentColor) {
		_imagesWithAccentColor = GetImagesWithTag(_accentColorTag);

		if (_previousImagesWithAccentColor == null || _imagesWithAccentColor != _previousImagesWithAccentColor) {
			// Different arrays.
			_imagesWithAccentColor = GetImagesWithTag(_accentColorTag);
			_previousImagesWithAccentColor = _imagesWithAccentColor;
		}

		PaintImages(_imagesWithAccentColor, newAccentColor);

		CurrentAccentColor = newAccentColor;
		OnAccentColorUpdate?.Invoke(CurrentAccentColor);
    }

}