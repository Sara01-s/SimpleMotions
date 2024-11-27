using SimpleMotions;
using UnityEngine;
using TMPro;

public class TransformComponentView : ComponentView {

    [SerializeField] private TMP_InputField _positionX;
    [SerializeField] private TMP_InputField _positionY;
    [SerializeField] private TMP_InputField _scaleW;
    [SerializeField] private TMP_InputField _scaleH;
    [SerializeField] private TMP_InputField _roll;
    [SerializeField] private TMP_Dropdown _easeDropdown;

    private ITransformComponentViewModel _transformComponentViewModel;
	private TMP_InputField[] _allInputFields;
    private IInputValidator _inputValidator;

    private string _previousValue;
	private bool _isSettingData;

	public void Configure(ITransformComponentViewModel transformComponentViewModel, IInputValidator inputValidator) {
        InitializeInputFields();
		_allInputFields = new [] { _positionX, _positionY, _scaleW, _scaleH, _roll };

        transformComponentViewModel.OnFirstKeyframe.Subscribe(() => {
            _KeyframeImage.sprite = _Unchangeable;
            _KeyframeImage.color = _EditorPainter.Theme.TextColor;
            _Update.color = _EditorPainter.CurrentAccentColor;
            _AddOrRemoveBlocker.SetActive(true);
            _Updateblocker.SetActive(false);
            _FrameHasKeyframe = true;
        });

        transformComponentViewModel.OnFrameHasTransformKeyframe.Subscribe(frameHasKeyframe => {
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

        _easeDropdown.onValueChanged.AddListener(newEase => {
            transformComponentViewModel.EaseDropdown.Value = newEase;
        });

		_AddOrRemoveKeyframe.onClick.AddListener(() => {
            if (!_FrameHasKeyframe) {
                transformComponentViewModel.OnSaveTransformKeyframe.Execute(GetTransformData());
                _Update.color = _EditorPainter.CurrentAccentColor;
                _KeyframeImage.sprite = _Remove;

                _Updateblocker.SetActive(false);
                _FrameHasKeyframe = true;
            }
            else {
                transformComponentViewModel.OnDeleteTransformKeyframe.Execute();
                _Update.color = _EditorPainter.Theme.TextColor;
                _KeyframeImage.sprite = _Add;

                _Updateblocker.SetActive(true);
                _FrameHasKeyframe = false;
            }
        });

        _UpdateKeyframe.onClick.AddListener(() => {
            if (_FrameHasKeyframe) {
                transformComponentViewModel.OnUpdateTransformKeyframe.Execute(GetTransformData());
				foreach (var inputField in _allInputFields) {
					_FlashInputField(inputField);
				}
            }
        });

        _KeyframeImage.color = _EditorPainter.Theme.TextColor;
        _Update.color = _EditorPainter.Theme.AccentColor;
        _AddOrRemoveBlocker.SetActive(true);
        _Updateblocker.SetActive(false);
        _FrameHasKeyframe = true;

        _transformComponentViewModel = transformComponentViewModel;
        _inputValidator = inputValidator;
	}

    private void TrySendNewComponentValue(string newValue, ReactiveValue<string> reactiveValue) {
		if (_isSettingData) {
			return;
		}
		
        newValue = _inputValidator.ValidateComponentInput(newValue);
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(newValue);

        if (CanSendInput(newValue) && !hasInvalidCharacters) {
		    reactiveValue.Value = newValue;
            _previousValue = newValue;
        }
	}

	private TransformDTO GetTransformData() {
		var position = (_positionX.text, _positionY.text);
		var scale = (_scaleW.text, _scaleH.text);
		string rollAngleDegrees = _roll.text;

		return new TransformDTO(position, scale, rollAngleDegrees);
	}

    public void RefreshData(TransformDTO transformDTO) {
		_isSettingData = true;

		// General numeric formatting, see https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
		const string floatFormat = "G";

        var firstInput = transformDTO.Position.x.ToString(floatFormat);
        _positionX.text = firstInput;

        firstInput = transformDTO.Position.y.ToString(floatFormat);
        _positionY.text = firstInput;

        firstInput = transformDTO.Scale.w.ToString(floatFormat);
        _scaleW.text = firstInput;

        firstInput = transformDTO.Scale.h.ToString(floatFormat);
        _scaleH.text = firstInput;

        firstInput = transformDTO.RollDegrees.ToString(floatFormat);
        _roll.text = firstInput;

		_isSettingData = false;
    }

    private bool CanSendInput(string newValue) {
        if (newValue == string.Empty) {
            return false;
        }

        if (newValue == "0") {
            return true;
        }

        if ("123456789".Contains(newValue[0])) {
            return true;
        }

        return !newValue.EndsWith("0") && !newValue.EndsWith(",");
    }

    private void CorrectInput(string newValue, ReactiveValue<string> reactiveValue) {
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(newValue);

        if (hasInvalidCharacters && CanSendInput(newValue)) {
            reactiveValue.Value = _previousValue;
        }
    }

    private void InitializeInputFields() {
        _positionX.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _transformComponentViewModel.PositionX));
        _positionY.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _transformComponentViewModel.PositionY));
        _scaleW.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _transformComponentViewModel.ScaleW));
        _scaleH.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _transformComponentViewModel.ScaleH));
        _roll.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _transformComponentViewModel.Roll));

        _positionX.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.PositionX));
        _positionY.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.PositionY));
        _scaleW.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.ScaleW));
        _scaleH.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.ScaleH));
        _roll.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.Roll));
    }

}