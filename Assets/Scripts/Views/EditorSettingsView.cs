using UnityEngine.UI;
using UnityEngine;

public class EditorSettingsView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;
    [SerializeField] private EditorPainter _editorPainter;
    [SerializeField] private Image _currentColor;
    [SerializeField] private AlwaysVisibleColor _colorPickerIcon;
    [SerializeField] private AlwaysVisibleColor _createEntity;
    [SerializeField] private AlwaysVisibleColor _cursorNumber;

    public void Configure() {}

    public void SubscribeToColorPicker() {
        SetColor();
        _flexibleColorPicker.SubscribeToColorChange(ChangeAccentColor);
    }

    private void ChangeAccentColor(Color color) {
        _editorPainter.ChangeEditorAccentColor(color);
        _currentColor.color = color;
        
        _colorPickerIcon.SetColor(color);
        _createEntity.SetColor(color);
        _cursorNumber.SetColor(color);
    }

    private void SetColor() {
        _flexibleColorPicker.SetColorWithoutNotification(_editorPainter.CurrentAccentColor);
        _currentColor.color = _flexibleColorPicker.Color;
    }

}