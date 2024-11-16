using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class ShapeComponentView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;
    [SerializeField] private GameObject[] _entitiesUI;

    private IEditorPainterParser _editorPainterParser;
    private IShapeComponentViewModel _shapeComponentViewModel;
    private EditorPainter _editorPainter;

    public void Configure(IShapeComponentViewModel shapeComponentViewModel, IEditorPainterParser editorPainterParser) {
        _shapeComponentViewModel = shapeComponentViewModel;
        _editorPainterParser = editorPainterParser;
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(SetEntityColor);
    }

    public void RefreshData(((float r, float g, float b, float a) color, string primitiveShape) shapeData, EditorPainter editorPainter) {
        _editorPainter = editorPainter;

        UpdateShape(shapeData.primitiveShape);
    }

    private void UpdateShape(string primitiveShape) {
        foreach (var entityUI in _entitiesUI) {
            var shapeType = entityUI.GetComponent<ShapeType>().ShapeTypeUI;

            if (shapeType.ToString() == primitiveShape) {
                entityUI.GetComponent<Image>().color = _editorPainter.Theme.AccentColor;
            }
        }
    }

    private void SetEntityColor(Color color) {
        _shapeComponentViewModel.SetColor(_editorPainterParser.UnityColorToSm(color));
    }

}