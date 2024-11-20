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

    private string _previousXInput;
    private string _previousYInput;
    private string _previousWInput;
    private string _previousHInput;
    private string _previousRInput;

    private ITransformComponentViewModel _transformComponentViewModel;

	public void Configure(ITransformComponentViewModel transformComponentViewModel, IInputValidator inputValidator) {
        _transformComponentViewModel = transformComponentViewModel;
        _inputValidator = inputValidator;

		_positionX.onValueChanged.AddListener(input => SendPosition(input, _transformComponentViewModel.PositionX));
        _positionY.onValueChanged.AddListener(input => SendPosition(input, _transformComponentViewModel.PositionY));
        _scaleW.onValueChanged.AddListener(input => SendPosition(input, _transformComponentViewModel.ScaleW));
        _scaleH.onValueChanged.AddListener(input => SendPosition(input, _transformComponentViewModel.ScaleH));
        _roll.onValueChanged.AddListener(input => SendPosition(input, _transformComponentViewModel.Roll));

        _positionX.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.PositionX, _previousXInput));
        _positionY.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.PositionY, _previousYInput));
        _scaleW.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.ScaleW, _previousWInput));
        _scaleH.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.ScaleH, _previousHInput));
        _roll.onDeselect.AddListener(input => CorrectInput(input, _transformComponentViewModel.Roll, _previousRInput));

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

	private ((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees) GetTransformData() {
		var position = (_positionX.text, _positionY.text);
		var scale = (_scaleW.text, _scaleH.text);
		string rollAngleDegrees = _roll.text;

		return (position, scale, rollAngleDegrees);
	}

	private void SendPosition(string input, ReactiveCommand<string> reactiveCommand) {
        bool hasInvalidCharacters = _inputValidator.ContainsInvalidCharacters(input);

        input = _inputValidator.ValidateTransformComponentInput(input);

        if (CanSendInput(input) && !hasInvalidCharacters) {
		    reactiveCommand.Execute(input);
        }
	}

    public void SetData(((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData) {
		// General numeric formatting, see https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings
		const string floatFormat = "G";

        var firstInput = transformData.pos.x.ToString(floatFormat);
        _previousXInput = firstInput;
        _positionX.text = firstInput;

        firstInput = transformData.pos.y.ToString(floatFormat);
        _previousYInput = firstInput;
        _positionY.text = firstInput;

        firstInput = transformData.scale.w.ToString(floatFormat);
        _previousWInput = firstInput;
        _scaleW.text = firstInput;

        firstInput = transformData.scale.h.ToString(floatFormat);
        _previousHInput = firstInput;
        _scaleH.text = firstInput;

        firstInput = transformData.rollAngleDegrees.ToString(floatFormat);
        _previousRInput = firstInput;
        _roll.text = firstInput;
    }

    private bool CanSendInput(string input) {
        return input == "0" || !input.EndsWith("0") && !input.EndsWith(",") && input != string.Empty;
    }

    private void CorrectInput(string input, ReactiveCommand<string> reactiveCommand, string previousInput) {
        bool hasInvalidCharacters = _inputValidator.ContainsInvalidCharacters(input);

        if (hasInvalidCharacters) {
            reactiveCommand.Execute(previousInput);
        }
    }

}