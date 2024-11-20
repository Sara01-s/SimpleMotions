using UnityEngine.UI;
using UnityEngine;

public class ColorPickerView : MonoBehaviour {

    [SerializeField] private GameObject _colorPicker;
    [SerializeField] private GameObject _alphaPicker;
    [SerializeField] private Button[] _openers;
    [SerializeField] private Button _closer;

    [SerializeField] private RectTransform _colorPickerPanel;

    [SerializeField] private ShapeComponentView _shapeComponentView;
    [SerializeField] private EditorSettingsView _editorSettingsView;
    [SerializeField] private VideoSettingsView _videoSettingsView;

    private Vector2 _defaultSize;
    private Vector2 _withoutAlpha;

    public void Configure() {
        _closer.onClick.AddListener(CloseColorPicker);

        for (int i = 0; i < _openers.Length; i++) {
            if (i == 0){
                _openers[i].onClick.AddListener(OpenShapeColorPicker);
            }
            else if (i == 1) {
                _openers[i].onClick.AddListener(OpenEditorColorPicker);
            }
            else if (i == 2) {
                _openers[i].onClick.AddListener(OpenVideoColorPicker);
            }
        }

        _defaultSize = _colorPickerPanel.GetComponent<RectTransform>().sizeDelta;
        _withoutAlpha = new Vector2(_defaultSize.x, _defaultSize.y - 45.0f);
    }

    private void OpenShapeColorPicker() {
        _shapeComponentView.SubscribeToColorPicker();

        ResizeColorPicker(withAlpha: true);

        _colorPicker.SetActive(true);
        _alphaPicker.SetActive(true);
    }

    private void OpenEditorColorPicker() {
        _editorSettingsView.SubscribeToColorPicker();

        ResizeColorPicker(withAlpha: false);
        
        _colorPicker.SetActive(true);
        _alphaPicker.SetActive(false);
    }

    private void OpenVideoColorPicker() {
        _videoSettingsView.SubscribeToColorPicker();

        ResizeColorPicker(withAlpha: false);

        _colorPicker.SetActive(true);
        _alphaPicker.SetActive(false);
    }

    private void CloseColorPicker() {
        _colorPicker.SetActive(false);
    }

    private void ResizeColorPicker(bool withAlpha) {
        if (withAlpha) {
            _colorPickerPanel.sizeDelta = _defaultSize;
        }
        else {
            _colorPickerPanel.sizeDelta = _withoutAlpha;
        }
    }
 
}