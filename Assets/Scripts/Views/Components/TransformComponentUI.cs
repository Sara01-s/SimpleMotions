using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class TransformComponentUI : MonoBehaviour {

    [SerializeField] private TMP_InputField _positionX; 
    [SerializeField] private TMP_InputField _positionY; 
    [SerializeField] private Button _savePosition;
    [SerializeField] private TMP_InputField _scaleW; 
    [SerializeField] private TMP_InputField _scaleH; 
    [SerializeField] private Button _saveScale;
    [SerializeField] private TMP_InputField _roll; 
    [SerializeField] private Button _saveRoll;

    public void RefreshData(((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) transformData) {
        _positionX.text = transformData.pos.x.ToString("0.0");
        _positionY.text = transformData.pos.y.ToString("0.0");

        _scaleW.text = transformData.scale.w.ToString("0.0");
        _scaleH.text = transformData.scale.h.ToString("0.0");

        _roll.text = transformData.rollAngleDegrees.ToString("0.0");
    }

}