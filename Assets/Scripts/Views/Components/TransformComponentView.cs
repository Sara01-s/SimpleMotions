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
	private const string DEGREES_SYMBOL = "º";

	public void Configure(ITransformComponentViewModel transformComponentViewModel, IInputValidator inputValidator) {
        _transformComponentViewModel = transformComponentViewModel;
        _inputValidator = inputValidator;

		_positionX.onSubmit.AddListener(SendPositionX);
        _positionY.onSubmit.AddListener(SendPositionY);
        _scaleW.onSubmit.AddListener(SendScaleW);
        _scaleH.onSubmit.AddListener(SendScaleH);
        _roll.onSubmit.AddListener(SendRollAngles);

		_addKeyframe.onClick.AddListener(() => transformComponentViewModel.SaveTransformKeyframe.Execute(GetTransformData()));
	}

	private ((string x, string y) pos, (string w, string h) scale, string rollAngleDegrees) GetTransformData() {
		var position = (_positionX.text, _positionY.text);
		var scale = (_scaleW.text, _scaleH.text);
		string rollAngleDegrees = _roll.text.Replace(DEGREES_SYMBOL, string.Empty);

		return (position, scale, rollAngleDegrees);
	}

	private void SendPositionX(string newInput) {
        newInput = _inputValidator.ValidateInput(newInput, _previousXInput);
		_transformComponentViewModel.PositionX.Execute(newInput);
	}

    private void SendPositionY(string value) {
        value = _inputValidator.ValidateInput(value, _previousYInput);
		_transformComponentViewModel.PositionY.Execute(value);
	}

    private void SendScaleW(string value) {
        value = _inputValidator.ValidateInput(value, _previousWInput);
		_transformComponentViewModel.ScaleW.Execute(value);
	}

    private void SendScaleH(string value) {
        value = _inputValidator.ValidateInput(value, _previousHInput);
		_transformComponentViewModel.ScaleH.Execute(value);
	}

    private void SendRollAngles(string value) {
        value = _inputValidator.ValidateInput(value, _previousRInput);
		_transformComponentViewModel.Roll.Execute(value.Replace(DEGREES_SYMBOL, string.Empty));
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

        firstInput = transformData.rollAngleDegrees.ToString("G");
        _previousRInput = firstInput;
        _roll.text = firstInput;
    }

    public void RefreshUI() {
        
    }

}