using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class ShapeComponentView : MonoBehaviour {

	[SerializeField] private Button[] _shapeButtons;
    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;
    [SerializeField] private Image _currentColor;

    private IEditorPainterParser _editorPainterParser;
    private IShapeComponentViewModel _shapeComponentViewModel;
    private EditorPainter _editorPainter;
    private ShapeType[] _shapeTypes;

    public void Configure(IShapeComponentViewModel shapeComponentViewModel, IEditorPainterParser editorPainterParser) {
        _shapeComponentViewModel = shapeComponentViewModel;
        _editorPainterParser = editorPainterParser;
		_shapeTypes = new ShapeType[_shapeButtons.Length];

		for (int i = 0; i < _shapeButtons.Length; i++) {
			_shapeTypes[i] = _shapeButtons[i].GetComponentInChildren<ShapeType>();
			MapButtons(_shapeButtons[i], _shapeTypes[i]);
		}
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(SetEntityColor);
    }

    public void RefreshData(((float r, float g, float b, float a) color, string primitiveShape) shapeData, EditorPainter editorPainter) {
        _editorPainter = editorPainter;
        UpdateShape(shapeData.primitiveShape);
    }

	private void MapButtons(Button button, ShapeType shapeType) {
		button.onClick.AddListener(() => {
			string shapeName = shapeType.ShapeTypeUI.ToString();
			_shapeComponentViewModel.SetShape(shapeName);
		});
	}

    private void UpdateShape(string primitiveShape) {
        foreach (var shapeImage in _shapeTypes) {
            var shapeType = shapeImage.ShapeTypeUI;

            if (shapeType.ToString() == primitiveShape) {
                shapeImage.GetComponent<Image>().color = _editorPainter.Theme.AccentColor;
            }
        }
    }

    private void SetEntityColor(Color color) {
        _shapeComponentViewModel.SetColor(_editorPainterParser.UnityColorToSm(color));
        _currentColor.color = color;
    }

}