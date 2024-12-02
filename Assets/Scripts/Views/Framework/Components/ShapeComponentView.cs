using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using SFB;

public class ShapeComponentView : ComponentView {

	[SerializeField] private Button[] _shapeButtons;
	[SerializeField] private Button _imageButton;
    [SerializeField] private Image _currentColor;

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;
    [SerializeField] private AlwaysVisibleColor _colorPickerIcon;

    private IShapeComponentViewModel _shapeComponentViewModel;
    private IEditorPainterParser _editorPainterParser;
    private ShapeType[] _shapeTypes;
    private ShapeType _currentShape;
	private bool _imageSelected;

    private void OnDisable() {
        _EditorPainter.OnAccentColorUpdate -= UpdateShapeAccentColor;
    }

    public void Configure(IShapeComponentViewModel shapeComponentViewModel, IEditorPainterParser editorPainterParser) {
		_shapeTypes = new ShapeType[_shapeButtons.Length];
        _AddOrRemoveKeyframe.interactable = false;
        _FrameHasKeyframe = true;

        shapeComponentViewModel.OnFirstKeyframe.Subscribe(() => {
            _AddOrRemoveKeyframe.interactable = false;
            _KeyframeImage.sprite = _Unchangeable;
            _KeyframeImage.color = _EditorPainter.Theme.TextColor;
            _FrameHasKeyframe = true;
            _UpdateKeyframeState(hasChanges: false);
        });

        shapeComponentViewModel.OnFrameHasShapeKeyframe.Subscribe(frameHasKeyframe => {
            _AddOrRemoveKeyframe.interactable = true;
			_KeyframeImage.sprite = frameHasKeyframe ? _Remove : _Add;
			_KeyframeImage.color = _EditorPainter.CurrentAccentColor;
            _FrameHasKeyframe = frameHasKeyframe;
			_UpdateKeyframeState(hasChanges: false);
        });

        _AddOrRemoveKeyframe.onClick.AddListener(() => {
            if (!_FrameHasKeyframe) {
                shapeComponentViewModel.OnSaveShapeKeyframe.Execute(SendShapeDTO());
                _KeyframeImage.sprite = _Remove;

                _FrameHasKeyframe = true;
            }
            else {
                shapeComponentViewModel.OnDeleteShapeKeyframe.Execute();
                _KeyframeImage.sprite = _Add;

                _FrameHasKeyframe = false;
            }

			_UpdateKeyframeState(hasChanges: false);
        });

        _UpdateKeyframe.onClick.AddListener(() => {
            if (_FrameHasKeyframe) {
				_UpdateKeyframeState(hasChanges: false);
                shapeComponentViewModel.OnUpdateShapeKeyframe.Execute(SendShapeDTO());
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

            var image = _imageButton.GetComponentInChildren<Image>();
			image.color = _EditorPainter.CurrentAccentColor;

			shapeComponentViewModel.OnImageSelected.Execute(imageFilePath[0]);
			_imageSelected = true;
			_UpdateKeyframeState(hasChanges: true);
		});

        shapeComponentViewModel.OnSelectedEntity.Subscribe(color => {
            var unityColor = _editorPainterParser.SmColorToUnity(color);
            _flexibleColorPicker.SetColorWithoutNotification(unityColor);
            _currentColor.color = unityColor;
            _UpdateKeyframeState(hasChanges: false);
        });

        _EditorPainter.OnAccentColorUpdate += UpdateShapeAccentColor;

		if (_shapeButtons.Length == 0) {
			Debug.LogError("Please assign shape buttons in inspector shape component.");
		}

		for (int i = 0; i < _shapeButtons.Length; i++) {
			_shapeTypes[i] = _shapeButtons[i].GetComponentInChildren<ShapeType>();
			MapButtons(_shapeButtons[i], _shapeTypes[i]);
		}

        _KeyframeImage.color = _EditorPainter.Theme.TextColor;
        _shapeComponentViewModel = shapeComponentViewModel;
        _editorPainterParser = editorPainterParser;
		_UpdateKeyframeState(hasChanges: false);
    }

    public ShapeDTO SendShapeDTO() {
        var currentColor = _flexibleColorPicker.Color;
		
		return new ShapeDTO (
			primitiveShape: _currentShape.PrimiviteDTO.ToString(),
			color: new ColorDTO(currentColor.r, currentColor.g, currentColor.b, currentColor.a)
		);
    }

    private void MapButtons(Button button, ShapeType shapeType) {
		button.onClick.AddListener(() => {
			string shapeName = shapeType.PrimiviteDTO.ToString();
			
			_shapeComponentViewModel.SetShape(shapeName);
            UpdateShape(shapeName);
			_UpdateKeyframeState(hasChanges: true);
		});
	}

    public void RefreshData(ShapeDTO shapeDTO) {
        UpdateShape(shapeDTO.PrimitiveShape);
    }

    private void UpdateShape(string shapeName) {
        foreach (var shapeImage in _shapeTypes) {
            var shapeType = shapeImage.PrimiviteDTO;
            var image = shapeImage.GetComponent<Image>();

            if (shapeType.ToString().CompareTo(shapeName) == 0) {
				_imageButton.GetComponentInChildren<Image>().color = _EditorPainter.Theme.TextColor;
				_imageSelected = false;
                image.color = _EditorPainter.CurrentAccentColor;
                _currentShape = shapeImage;
            }
			else {
				image.color = _EditorPainter.Theme.TextColor;
			}
        }
    }

    private void UpdateShapeAccentColor(Color accentColor) {
		if (_imageSelected) {
			_imageButton.GetComponentInChildren<Image>().color = accentColor;
			return;
		}

        foreach (var shapeType in _shapeTypes) {
            if (shapeType == _currentShape) {
                var image = shapeType.GetComponent<Image>();
                image.color = accentColor;
            }   
        }
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(SetEntityColor);
    }

    private void SetEntityColor(Color color) {
        if (_FrameHasKeyframe) {
            _shapeComponentViewModel.SetColor(_editorPainterParser.UnityColorToSm(color));
            _currentColor.color = color;
            _colorPickerIcon.SetColor(color);
		    _UpdateKeyframeState(hasChanges: true);
        }
    }

}