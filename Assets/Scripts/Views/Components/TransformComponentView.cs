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
    [SerializeField] private Button _save;

    private ITransformComponentViewModel _transformComponentViewModel;

	public void Configure(ITransformComponentViewModel transformComponentViewModel) {
        _transformComponentViewModel = transformComponentViewModel;

		_positionX.onValueChanged.AddListener(SendPositionX);
        _positionY.onValueChanged.AddListener(SendPositionY);
        _scaleW.onValueChanged.AddListener(SendScaleW);
        _scaleH.onValueChanged.AddListener(SendScaleH);
        _roll.onValueChanged.AddListener(SendRollAngles);
	}

	private void SendPositionX(string value) {
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
		_transformComponentViewModel.RollAngles.Execute(value);
	}

    public void SetData(((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData) {
        _positionX.text = transformData.pos.x.ToString("0.0");
        _positionY.text = transformData.pos.y.ToString("0.0");

        _scaleW.text = transformData.scale.w.ToString("0.0");
        _scaleH.text = transformData.scale.h.ToString("0.0");

        _roll.text = transformData.rollAngleDegrees.ToString("0.0º");
    }

    public void RefreshUI() {
        
    }

}