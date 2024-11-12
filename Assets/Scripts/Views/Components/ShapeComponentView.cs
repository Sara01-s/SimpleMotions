using UnityEngine.UI;
using UnityEngine;
using TMPro;
using SimpleMotions;

public class ShapeComponentView : MonoBehaviour {

    [SerializeField] private GameObject[] _entitiesUI;

    [SerializeField] private TMP_InputField _hex; 
    [SerializeField] private TMP_InputField _alpha; 

    private IShapeComponentViewModel _shapeComponentViewModel;
    private EditorPainter _editorPainter;

    public void Configure(IShapeComponentViewModel shapeComponentViewModel) {
        _shapeComponentViewModel = shapeComponentViewModel;

        _hex.onValueChanged.AddListener(SendColorHex);
        _alpha.onValueChanged.AddListener(SendColorAlpha);
    }

    private void SendColorHex(string value) {
        _shapeComponentViewModel.Hex.Execute(value);
    }

    private void SendColorAlpha(string value) {
        _shapeComponentViewModel.Alpha.Execute(value);
    }

    public void RefreshData(((float r, float g, float b, float a) color, string primitiveShape) shapeData, EditorPainter editorPainter) {
        _editorPainter = editorPainter;

        UpdateColor(shapeData.color.r, shapeData.color.g, shapeData.color.b, shapeData.color.a);
        UpdateShape(shapeData.primitiveShape);
    }

    private void UpdateColor(float red, float green, float blue, float alpha) {
        var color = new Color(red, green,blue, alpha);
        string hexColor = "#" + ColorUtility.ToHtmlStringRGB(color);
		
        _hex.text = hexColor;
        _alpha.text = alpha.ToString("0.0");
    }

    private void UpdateShape(string primitiveShape) {
        foreach (var entityUI in _entitiesUI) {
            var shapeType = entityUI.GetComponent<ShapeType>().ShapeTypeUI;

            if (shapeType.ToString() == primitiveShape) {
                entityUI.GetComponent<Image>().color = _editorPainter.Theme.AccentColor;
            }
        }
    }

}