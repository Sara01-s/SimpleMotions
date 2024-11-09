using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShapeComponentUI : MonoBehaviour {

    [SerializeField] private GameObject _rect;
    [SerializeField] private GameObject _circle;
    [SerializeField] private GameObject _line;
    [SerializeField] private GameObject _triangle;
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _hexagon;
    [SerializeField] private GameObject _octagon;
    [SerializeField] private GameObject _heart;

    [SerializeField] private TMP_InputField _colorHex; 
    [SerializeField] private TMP_InputField _colorAlpha; 

    public void RefreshData(((float r, float g, float b, float a) color, string primitiveShape) shapeData) {
        UpdateColor(shapeData.color.r, shapeData.color.g, shapeData.color.b, shapeData.color.a);
        UpdateShape(shapeData.primitiveShape);
    }

    private void UpdateColor(float red, float green, float blue, float alpha) {
        var color = new Color(red, green,blue, alpha);
        string hexColor = "#" + ColorUtility.ToHtmlStringRGB(color);
        _colorHex.text = hexColor;

        _colorAlpha.text = alpha.ToString("0.0");
    }

    private void UpdateShape(string primitiveShape) {
        switch (primitiveShape) {
            case "Rect":
                _rect.GetComponent<Image>().color = Color.yellow;
            break;

            case "Circle":
                _circle.GetComponent<Image>().color = Color.yellow; // Ejemplo de feedback.
            break;

            case "Triangle":
                _triangle.GetComponent<Image>().color = Color.yellow;
            break;
        }
    }

}