using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class ShapeComponentView : MonoBehaviour {

    [SerializeField] private Button _addOrRemoveKeyframe;
    [SerializeField] private Image _keyframeImage;
    [SerializeField] private Sprite _addKeyframe, _removeKeyframe;
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

        _addOrRemoveKeyframe.onClick.AddListener(() => {
           /* if (_keyframeImage.sprite == _addKeyframe) {
                shapeComponentViewModel.SaveTransformKeyframe.Execute(GetShapeData());
                shapeComponentViewModel.OnDrawTransfromKeyframe.Execute();
                _keyframeImage.sprite = _removeKeyframe;
            }
            else {
                transformComponentViewModel.OnKeyframeDeleted.Execute();
                _keyframeImage.sprite = _addKeyframe;
            }*/
        });

		if (_shapeButtons.Length == 0) {
			Debug.LogError("Please assign shape buttons in inspector shape component.");
		}

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
            UpdateShape(shapeName);
		});
	}

    private void UpdateShape(string shapeName) {
        foreach (var shapeImage in _shapeTypes) {
            var shapeType = shapeImage.ShapeTypeUI;
            var image = shapeImage.GetComponent<Image>();

            if (shapeType.ToString().CompareTo(shapeName) == 0) {
                image.color = _editorPainter.Theme.AccentColor;
            }
            else {
                image.color = _editorPainter.Theme.TextColor;
            }
        }
    }

    private void SetEntityColor(Color color) {
        _shapeComponentViewModel.SetColor(_editorPainterParser.UnityColorToSm(color));
        _currentColor.color = color;
    }

}