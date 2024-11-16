using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;


public class ColorPickerView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;
    [SerializeField] private Button[] _openers;
    [SerializeField] private Button _closer;

    private IShapeComponentViewModel _shapeComponentViewModel;
    private IEditorPainterParser _editorPainterParser;

    public void Configure(IShapeComponentViewModel shapeComponentViewModel, FlexibleColorPicker flexibleColorPicker, IEditorPainterParser editorPainterParser) {
        _shapeComponentViewModel = shapeComponentViewModel;
        _flexibleColorPicker = flexibleColorPicker;
        _editorPainterParser = editorPainterParser;

        _flexibleColorPicker.OnColorChange.AddListener(SetColor);
        _closer.onClick.AddListener(CloseColorPicker);

        foreach (var opener in _openers) {
            opener.onClick.AddListener(OpenColorPicker);
        }
    }

    private void SetColor(Color color) {
        _shapeComponentViewModel.SetColor(_editorPainterParser.UnityColorToSm(color));
    }

    private void OpenColorPicker() {
        _flexibleColorPicker.gameObject.SetActive(true);
    }

    private void CloseColorPicker() {
        _flexibleColorPicker.gameObject.SetActive(false);
    }

}