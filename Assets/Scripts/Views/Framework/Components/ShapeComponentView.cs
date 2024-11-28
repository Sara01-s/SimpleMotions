using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using SFB;

public class ShapeComponentView : ComponentView {

	[SerializeField] private Button[] _shapeButtons;
	[SerializeField] private Button _imageButton;
    [SerializeField] private Image _currentColor;

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;

    private IShapeComponentViewModel _shapeComponentViewModel;
    private IEditorPainterParser _editorPainterParser;
    private ShapeType[] _shapeTypes;
    private ShapeType _currentShape;

    public void Configure(IShapeComponentViewModel shapeComponentViewModel, IEditorPainterParser editorPainterParser) {
		_shapeTypes = new ShapeType[_shapeButtons.Length];

        shapeComponentViewModel.OnFirstKeyframe.Subscribe(() => {
            _KeyframeImage.sprite = _Unchangeable;
            _KeyframeImage.color = _EditorPainter.Theme.TextColor;
            _Update.color = _EditorPainter.CurrentAccentColor;
            _AddOrRemoveBlocker.SetActive(true);
            _Updateblocker.SetActive(false);
            _FrameHasKeyframe = true;
        });

        shapeComponentViewModel.OnFrameHasShapeKeyframe.Subscribe(frameHasKeyframe => {
            if (frameHasKeyframe) {
                _KeyframeImage.sprite = _Remove;
                _Update.color = _EditorPainter.CurrentAccentColor;
                _Updateblocker.SetActive(false);
            }
            else {
                _KeyframeImage.sprite = _Add;
                _Update.color = _EditorPainter.Theme.TextColor;
                _Updateblocker.SetActive(true);
            }
            
            _AddOrRemoveBlocker.SetActive(false);
            _KeyframeImage.color = _EditorPainter.CurrentAccentColor;
            _FrameHasKeyframe = frameHasKeyframe;
        });

        _AddOrRemoveKeyframe.onClick.AddListener(() => {
            if (!_FrameHasKeyframe) {
                shapeComponentViewModel.OnSaveShapeKeyframe.Execute(GetShapeData());
                _Update.color = _EditorPainter.CurrentAccentColor;
                _KeyframeImage.sprite = _Remove;

                _Updateblocker.SetActive(false);
                _FrameHasKeyframe = true;
            }
            else {
                shapeComponentViewModel.OnDeleteShapeKeyframe.Execute();
                _Update.color = _EditorPainter.Theme.TextColor;
                _KeyframeImage.sprite = _Add;

                _Updateblocker.SetActive(true);
                _FrameHasKeyframe = false;
            }
        });

        _UpdateKeyframe.onClick.AddListener(() => {
            if (_FrameHasKeyframe) {
                shapeComponentViewModel.OnUpdateShapeKeyframe.Execute(GetShapeData());
            }
        });

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
			if (imageFilePath == null || imageFilePath.Length <= 0) {
				return;
			}

			shapeComponentViewModel.OnImageSelected.Execute(imageFilePath[0]);
		});

		if (_shapeButtons.Length == 0) {
			Debug.LogError("Please assign shape buttons in inspector shape component.");
		}

		for (int i = 0; i < _shapeButtons.Length; i++) {
			_shapeTypes[i] = _shapeButtons[i].GetComponentInChildren<ShapeType>();
			MapButtons(_shapeButtons[i], _shapeTypes[i]);
		}

        _KeyframeImage.color = _EditorPainter.Theme.TextColor;
        _Update.color = _EditorPainter.Theme.AccentColor;
        _AddOrRemoveBlocker.SetActive(true);
        _Updateblocker.SetActive(false);
        _FrameHasKeyframe = true;

        _shapeComponentViewModel = shapeComponentViewModel;
        _editorPainterParser = editorPainterParser;
    }

    public (string shapeName, float r, float g, float b, float a) GetShapeData() {
        var currentColor = _flexibleColorPicker.Color;
        return (_currentShape.ShapeTypeUI.ToString(), currentColor.r, currentColor.g, currentColor.b, currentColor.a);
    }

    private void MapButtons(Button button, ShapeType shapeType) {
		button.onClick.AddListener(() => {
			string shapeName = shapeType.ShapeTypeUI.ToString();
			_shapeComponentViewModel.SetShape(shapeName);
            UpdateShape(shapeName);
		});
	}

    public void RefreshData(ShapeDTO shapeDTO) {
        UpdateShape(shapeDTO.PrimitiveShape);
    }

    private void UpdateShape(string shapeName) {
        foreach (var shapeImage in _shapeTypes) {
            var shapeType = shapeImage.ShapeTypeUI;
            var image = shapeImage.GetComponent<Image>();

            if (shapeType.ToString().CompareTo(shapeName) == 0) {
                image.color = _EditorPainter.Theme.AccentColor;
                _currentShape = shapeImage;
            }
            else {
                image.color = _EditorPainter.Theme.TextColor;
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