using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using SFB;

public class ShapeComponentView : MonoBehaviour {

    [SerializeField] private Button _addOrRemoveKeyframe;
	[SerializeField] private Button[] _shapeButtons;
	[SerializeField] private Button _imageButton;
    [SerializeField] private Image _keyframeImage;
    [SerializeField] private Image _currentColor;
    [SerializeField] private Sprite _addKeyframe, _removeKeyframe;
    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;

    private IEditorPainterParser _editorPainterParser;
    private IShapeComponentViewModel _shapeComponentViewModel;
    private EditorPainter _editorPainter;
    private ShapeType[] _shapeTypes;
    private ShapeType _currentShape;

    public void Configure(IShapeComponentViewModel shapeComponentViewModel, IEditorPainterParser editorPainterParser) {
        _shapeComponentViewModel = shapeComponentViewModel;
        _editorPainterParser = editorPainterParser;
		_shapeTypes = new ShapeType[_shapeButtons.Length];

		_imageButton.onClick.AddListener(() => {
			var imageExtensions = new [] {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
            };

			string[] imageFilePath = StandaloneFileBrowser.OpenFilePanel (
				title: "Open Image",
				directory: string.Empty,
				imageExtensions,
				multiselect: false
			);
			
			// TODO - Validate
			if (imageFilePath == null) {
				return;
			}
			
			shapeComponentViewModel.OnImageSelected.Execute(imageFilePath[0]);
		});

        shapeComponentViewModel.OnFrameHasShapeKeyframe.Subscribe(hasKeyframe => {
            if (hasKeyframe) {
                _keyframeImage.sprite = _removeKeyframe;
            }
            else {
                _keyframeImage.sprite = _addKeyframe;
            }
        });

        _addOrRemoveKeyframe.onClick.AddListener(() => {
            if (_keyframeImage.sprite == _addKeyframe) {
                shapeComponentViewModel.SaveShapeKeyframe.Execute(GetShapeData());
                shapeComponentViewModel.OnDrawShapeKeyframe.Execute();
                _keyframeImage.sprite = _removeKeyframe;
            }
            else {
                shapeComponentViewModel.OnShapeKeyframeDeleted.Execute();
                _keyframeImage.sprite = _addKeyframe;
            }
        });

		if (_shapeButtons.Length == 0) {
			Debug.LogError("Please assign shape buttons in inspector shape component.");
		}

		for (int i = 0; i < _shapeButtons.Length; i++) {
			_shapeTypes[i] = _shapeButtons[i].GetComponentInChildren<ShapeType>();
			MapButtons(_shapeButtons[i], _shapeTypes[i]);
		}
    }

    public (string shapeName, float r, float g, float b, float a) GetShapeData() {
        var currentColor = _flexibleColorPicker.Color;
        return (_currentShape.ShapeTypeUI.ToString(), currentColor.r, currentColor.g, currentColor.b, currentColor.a);
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
                _currentShape = shapeImage;
            }
            else {
                image.color = _editorPainter.Theme.TextColor;
            }
        }
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(SetEntityColor);
    }

    private void SetEntityColor(Color color) {
        _shapeComponentViewModel.SetColor(_editorPainterParser.UnityColorToSm(color));
        _currentColor.color = color;
    }

}