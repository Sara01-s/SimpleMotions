using UnityEngine.UI;
using UnityEngine;

public class ColorPickerView : MonoBehaviour {

    [SerializeField] private GameObject _colorPicker;
    [SerializeField] private Button[] _openers;
    [SerializeField] private Button _closer;

    [SerializeField] private ShapeComponentView _shapeComponentView;
    [SerializeField] private EditorSettingsView _editorSettingsView;
    [SerializeField] private VideoSettingsView _videoSettingsView;

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
    }

    private void OpenShapeColorPicker() {
        _shapeComponentView.SubscribeToColorPicker();
        _colorPicker.gameObject.SetActive(true);
    }

    private void OpenEditorColorPicker() {
        _editorSettingsView.SubscribeToColorPicker();
        _colorPicker.gameObject.SetActive(true);
    }

    private void OpenVideoColorPicker() {
        _videoSettingsView.SubscribeToColorPicker();
        _colorPicker.gameObject.SetActive(true);
    }

    private void CloseColorPicker() {
        _colorPicker.gameObject.SetActive(false);
    }

}