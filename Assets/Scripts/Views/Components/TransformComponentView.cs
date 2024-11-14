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

	private void SendPositionX(string value) {
        value = _inputValidator.ValidateInput(value);
        print(value);
		_transformComponentViewModel.PositionX.Execute(value);
	}

    private void SendPositionY(string value) {
		_transformComponentViewModel.PositionY.Execute(value);
	}

    private void SendScaleW(string value) {
		_transformComponentViewModel.ScaleW.Execute(value);
	}

    private void SendScaleH(string value) {
		_transformComponentViewModel.ScaleH.Execute(value);
	}

    private void SendRollAngles(string value) {
		_transformComponentViewModel.Roll.Execute(value.Replace(DEGREES_SYMBOL, string.Empty));
	}

    public void SetData(((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData) {
        _positionX.text = transformData.pos.x.ToString("G");
        _positionY.text = transformData.pos.y.ToString("G");

        _scaleW.text = transformData.scale.w.ToString("G");
        _scaleH.text = transformData.scale.h.ToString("G");

        var rollText = transformData.rollAngleDegrees.ToString("G");
        _roll.text = rollText + DEGREES_SYMBOL;
    }

    public void RefreshUI() {
        
    }

}