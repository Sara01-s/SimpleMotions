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
    [SerializeField] private Button _addKeyframe;

    private IInputValidator _inputValidator;

    private string _previousXInput;
    private string _previousYInput;
    private string _previousWInput;
    private string _previousHInput;
    private string _previousRInput;

    private ITransformComponentViewModel _transformComponentViewModel;
	private const char DEGREES_SYMBOL = 'º';

	public void Configure(ITransformComponentViewModel transformComponentViewModel, IInputValidator inputValidator) {
        _transformComponentViewModel = transformComponentViewModel;
        _inputValidator = inputValidator;

		_positionX.onValueChanged.AddListener(SendPositionX);
        _positionY.onValueChanged.AddListener(SendPositionY);
        _scaleW.onValueChanged.AddListener(SendScaleW);
        _scaleH.onValueChanged.AddListener(SendScaleH);
        _roll.onValueChanged.AddListener(SendRollAngles);

		_addKeyframe.onClick.AddListener(() => transformComponentViewModel.SaveTransformKeyframe.Execute(GetTransformData()));
	}

	private ((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees) GetTransformData() {
		var position = (_positionX.text, _positionY.text);
		var scale = (_scaleW.text, _scaleH.text);
		string rollAngleDegrees = _roll.text.Replace(DEGREES_SYMBOL, ' ');

		return (position, scale, rollAngleDegrees);
	}

	private void SendPositionX(string newInput) {
        newInput = _inputValidator.ValidateInput(newInput, _previousXInput);

        if (newInput[^1] != ',' && newInput != string.Empty) {
		    _transformComponentViewModel.PositionX.Execute(newInput);
        }
	}

    private void SendPositionY(string newInput) {
        newInput = _inputValidator.ValidateInput(newInput, _previousYInput);

        if (newInput[^1] != ',') {
		    _transformComponentViewModel.PositionY.Execute(newInput);
        }
	}

    private void SendScaleW(string newInput) {
        newInput = _inputValidator.ValidateInput(newInput, _previousWInput);

        if (newInput[^1] != ',') {
		    _transformComponentViewModel.ScaleW.Execute(newInput);
        }
	}

    private void SendScaleH(string newInput) {
        newInput = _inputValidator.ValidateInput(newInput, _previousHInput);

        if (newInput[^1] != ',') {
		    _transformComponentViewModel.ScaleH.Execute(newInput);
        }
	}

    private void SendRollAngles(string newInput) {
        newInput = _inputValidator.ValidateInput(newInput, _previousRInput);

        if (newInput[^1] != ',') {
		    _transformComponentViewModel.Roll.Execute(newInput.Replace(DEGREES_SYMBOL, ' '));
        }
	}

    public void SetData(((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData) {
        var firstInput = transformData.pos.x.ToString("G");
        _previousXInput = firstInput;
        _positionX.text = firstInput;

        firstInput = transformData.pos.y.ToString("G");
        _previousYInput = firstInput;
        _positionY.text = firstInput;

        firstInput = transformData.scale.w.ToString("G");
        _previousWInput = firstInput;
        _scaleW.text = firstInput;

        firstInput = transformData.scale.h.ToString("G");
        _previousHInput = firstInput;
        _scaleH.text = firstInput;

        firstInput = transformData.rollAngleDegrees.ToString("G") + DEGREES_SYMBOL;
        _previousRInput = firstInput;
        _roll.text = firstInput;
    }

    public void RefreshUI() {
        
    }

}