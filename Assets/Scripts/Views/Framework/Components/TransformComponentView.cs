using SimpleMotions;
using UnityEngine;
using TMPro;

public class TransformComponentView : ComponentView {

    [SerializeField] private TMP_InputField _positionX;
    [SerializeField] private TMP_InputField _positionY;
    [SerializeField] private TMP_InputField _scaleW;
    [SerializeField] private TMP_InputField _scaleH;
    [SerializeField] private TMP_InputField _roll;

	private TMP_InputField[] _allInputFields;
    private IInputValidator _inputValidator;

    private bool _entityJustCreated;
	private bool _isSettingData;

    private string _previousXValue;
    private string _previousYValue;
    private string _previousWValue;
    private string _previousHValue;
    private string _previousRValue;

	public void Configure(ITransformComponentViewModel transformComponentViewModel, IInputValidator inputValidator) {
		// TODO - Pasar la posicion de Y (del input field) (Juli)
        _positionX.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _positionY.text, transformComponentViewModel.PositionX));
        _positionY.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _positionX.text, transformComponentViewModel.PositionY));
        _scaleW.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _scaleH.text, transformComponentViewModel.ScaleW));
        _scaleH.onValueChanged.AddListener(input => TrySendNewComponentValue(input, _scaleW.text, transformComponentViewModel.ScaleH));
        _roll.onValueChanged.AddListener(input => TrySendNewComponentValue(input, transformComponentViewModel.Roll));

        _positionX.onDeselect.AddListener(input => CorrectInput(input, _positionY.text, _previousXValue, transformComponentViewModel.PositionX));
        _positionY.onDeselect.AddListener(input => CorrectInput(input, _positionX.text, _previousYValue, transformComponentViewModel.PositionY));
        _scaleW.onDeselect.AddListener(input => CorrectInput(input, _scaleH.text, _previousWValue, transformComponentViewModel.ScaleW));
        _scaleH.onDeselect.AddListener(input => CorrectInput(input, _scaleW.text, _previousHValue, transformComponentViewModel.ScaleH));
        _roll.onDeselect.AddListener(input => CorrectInput(input, _previousRValue, transformComponentViewModel.Roll));

		_allInputFields = new [] { _positionX, _positionY, _scaleW, _scaleH, _roll };
        _FrameHasKeyframe = true;

        transformComponentViewModel.OnFirstKeyframe.Subscribe(() => {
            _KeyframeImage.sprite = _Unchangeable;
            _KeyframeImage.color = _EditorPainter.Theme.TextColor;
            _Update.color = _EditorPainter.Theme.PrimaryColor;
            _FrameHasKeyframe = true;
        });

        transformComponentViewModel.OnFrameHasTransformKeyframe.Subscribe(frameHasKeyframe => {
			_KeyframeImage.sprite = frameHasKeyframe ? _Remove : _Add;
			_Update.color = _EditorPainter.Theme.PrimaryColor;
            _KeyframeImage.color = _EditorPainter.CurrentAccentColor;
            _FrameHasKeyframe = frameHasKeyframe;
        });

		_AddOrRemoveKeyframe.onClick.AddListener(() => {
            if (!_FrameHasKeyframe) {
                transformComponentViewModel.OnSaveTransformKeyframe.Execute(GetTransformData());
                _KeyframeImage.sprite = _Remove;

                _FrameHasKeyframe = true;
            }
            else {
                transformComponentViewModel.OnDeleteTransformKeyframe.Execute();
                _KeyframeImage.sprite = _Add;

                _FrameHasKeyframe = false;
            }

			_UpdateKeyframeState(hasChanges: false);
        });

        _UpdateKeyframe.onClick.AddListener(() => {
			_UpdateKeyframeState(hasChanges: false);

            if (_FrameHasKeyframe) {
                transformComponentViewModel.OnUpdateTransformKeyframe.Execute(GetTransformData());
				
				foreach (var inputField in _allInputFields) {
					_FlashInputField(inputField);
				}
            }
        });

        transformComponentViewModel.OnEntityCreated.Subscribe(() => {
			_UpdateKeyframeState(hasChanges: false);
            _entityJustCreated = true;
        });

		_UpdateKeyframeState(hasChanges: false);

        _KeyframeImage.color = _EditorPainter.Theme.TextColor;
        _inputValidator = inputValidator;
	}

    private void TrySendNewComponentValue(string valueToChange, string valueToKeep, ReactiveValue<(string, string)> reactiveValue) {
        if (!_entityJustCreated) {
			_UpdateKeyframeState(hasChanges: true);
        }

        _entityJustCreated = false;

		if (_isSettingData) {
			return;
		}
		
        valueToChange = _inputValidator.ValidateComponentInput(valueToChange);
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(valueToChange);

        if (CanSendInput(valueToChange) && !hasInvalidCharacters) {
		    reactiveValue.Value = (valueToChange, valueToKeep);
        }
	}

    private void TrySendNewComponentValue(string valueToChange, ReactiveValue<string> reactiveValue) {
        if (!_entityJustCreated) {
			_UpdateKeyframeState(hasChanges: true);
        }

        _entityJustCreated = false;

		if (_isSettingData) {
			return;
		}
		
        valueToChange = _inputValidator.ValidateComponentInput(valueToChange);
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(valueToChange);

        if (CanSendInput(valueToChange) && !hasInvalidCharacters) {
		    reactiveValue.Value = valueToChange;
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

        var input = transformDTO.Position.x.ToString(floatFormat);
        _previousXValue = input;
        _positionX.text = input;

        input = transformDTO.Position.y.ToString(floatFormat);
        _previousYValue = input;
        _positionY.text = input;

        input = transformDTO.Scale.w.ToString(floatFormat);
        _previousWValue = input;
        _scaleW.text = input;

        input = transformDTO.Scale.h.ToString(floatFormat);
        _previousHValue = input;
        _scaleH.text = input;

        input = transformDTO.RollDegrees.ToString(floatFormat);
        _previousRValue = input;
        _roll.text = input;

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

    private void CorrectInput(string valueToChange, string valueToKeep, string previousValue, ReactiveValue<(string, string)> reactiveValue) {
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(valueToChange);

        if (hasInvalidCharacters) {
            reactiveValue.Value = (previousValue, valueToKeep);
        }
    }

    private void CorrectInput(string valueToChange, string previousValue, ReactiveValue<string> reactiveValue) {
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(valueToChange);

        if (hasInvalidCharacters) {
            reactiveValue.Value = previousValue;
        }
    }

}