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
    [SerializeField] private Button _updateKeyframe;
    [SerializeField] private GameObject _blocker;
    
    [SerializeField] private EditorPainter _editorPainter;

    [SerializeField] private Image _keyframeImage;
    [SerializeField] private Sprite _add;
    [SerializeField] private Sprite _remove;
    [SerializeField] private Sprite _unchangeable;
    [SerializeField] private Image _update;

    private ITransformComponentViewModel _transformComponentViewModel;
    private IInputValidator _inputValidator;

    private bool _frameHasKeyframe;
    private string _previousValue;
	private bool _isSettingData;

	public void Configure(ITransformComponentViewModel transformComponentViewModel, IInputValidator inputValidator) {
        InitializeInputFields();

        transformComponentViewModel.OnFirstKeyframe.Subscribe(() => {
            _keyframeImage.sprite = _unchangeable;
            _blocker.SetActive(true);
        });

        transformComponentViewModel.OnFrameHasTransformKeyframe.Subscribe(frameHasKeyframe => {
            if (frameHasKeyframe) {
                _keyframeImage.sprite = _remove;
                _update.color = _editorPainter.CurrentAccentColor;
            }
            else {
                _keyframeImage.sprite = _add;
                _update.color = _editorPainter.Theme.TextColor;
            }

            _blocker.SetActive(false);
            _frameHasKeyframe = frameHasKeyframe;
        });

		_addOrRemoveKeyframe.onClick.AddListener(() => {
            if (!_frameHasKeyframe) {
                transformComponentViewModel.OnSaveTransformKeyframe.Execute(GetTransformData());
                _update.color = _editorPainter.CurrentAccentColor;
                _keyframeImage.sprite = _remove;

                _frameHasKeyframe = true;
            }
            else {
                transformComponentViewModel.OnDeleteTransformKeyframe.Execute();
                _update.color = _editorPainter.Theme.TextColor;
                _keyframeImage.sprite = _add;

                _frameHasKeyframe = false;
            }
        });

        _updateKeyframe.onClick.AddListener(() => {
            if (_frameHasKeyframe) {
                transformComponentViewModel.OnUpdateTransformKeyframe.Execute(GetTransformData());
            }
        });

        _transformComponentViewModel = transformComponentViewModel;
        _inputValidator = inputValidator;
	}

    private void TrySendNewComponentValue(string newValue, ReactiveCommand<string> reactiveCommand) {
		if (_isSettingData) {
			return;
		}
		
        newValue = _inputValidator.ValidateComponentInput(newValue);

        bool hasInvalidCharacters = _inputValidator.IsComponentNewInputInvalid(newValue);

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
		_isSettingData = true;

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

		_isSettingData = false;
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