using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class TransformComponentView : MonoBehaviour {

    [SerializeField] private TMP_InputField _positionX;
    [SerializeField] private TMP_InputField _positionY;
    [SerializeField] private TMP_InputField _scaleW;
    [SerializeField] private TMP_InputField _scaleH;
    [SerializeField] private TMP_InputField _roll;

    [SerializeField] private Button _addOrRemoveKeyframe;
    [SerializeField] private Image _keyframeImage;
    [SerializeField] private Sprite _addKeyframe;
    [SerializeField] private Sprite _removeKeyframe;

    private IInputValidator _inputValidator;

    private string _previousValue;

    private ITransformComponentViewModel _transformComponentViewModel;

	public void Configure(ITransformComponentViewModel transformComponentViewModel, IInputValidator inputValidator) {
        _transformComponentViewModel = transformComponentViewModel;
        _inputValidator = inputValidator;

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

        transformComponentViewModel.AddOrRemoveKeyframe.Subscribe(addKeyframe => {
            if (addKeyframe) {
                _keyframeImage.sprite = _addKeyframe;
            }
            else {
                _keyframeImage.sprite = _removeKeyframe;
            }
        });

		_addOrRemoveKeyframe.onClick.AddListener(() => {
            if (_keyframeImage.sprite == _addKeyframe) {
                transformComponentViewModel.SaveTransformKeyframe.Execute(GetTransformData());
            }
            else {
                transformComponentViewModel.DeleteKeyFrame.Execute();
            }
        });
	}

    private void TrySendNewComponentValue(string newValue, ReactiveCommand<string> reactiveCommand) {
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(newValue);

        newValue = _inputValidator.ValidateComponentInput(newValue);

        if (CanSendInput(newValue) && !hasInvalidCharacters) {
		    reactiveCommand.Execute(newValue);
            _previousValue = newValue;
        }
	}

	private ((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees) GetTransformData() {
		var position = (_positionX.text, _positionY.text);
		var scale = (_scaleW.text, _scaleH.text);
		string rollAngleDegrees = _roll.text;

		return (position, scale, rollAngleDegrees);
	}

    public void SetData(((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData) {
		// General numeric formatting, see https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
		const string floatFormat = "G";

        var firstInput = transformData.pos.x.ToString(floatFormat);
        _positionX.text = firstInput;

        firstInput = transformData.pos.y.ToString(floatFormat);
        _positionY.text = firstInput;

        firstInput = transformData.scale.w.ToString(floatFormat);
        _scaleW.text = firstInput;

        firstInput = transformData.scale.h.ToString(floatFormat);
        _scaleH.text = firstInput;

        firstInput = transformData.rollAngleDegrees.ToString(floatFormat);
        _roll.text = firstInput;
    }

    private bool CanSendInput(string newValue) {
        return newValue == "0" || !newValue.EndsWith("0") && !newValue.EndsWith(",") && newValue != string.Empty;
    }

    private void CorrectInput(string newValue, ReactiveCommand<string> reactiveCommand) {
        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(newValue);

        if (hasInvalidCharacters && CanSendInput(newValue)) {
            reactiveCommand.Execute(_previousValue);
        }
    }

}