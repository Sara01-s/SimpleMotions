using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AlwaysVisibleColor : MonoBehaviour {

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    public void SetColor(Color color) {
        if (_image != null) {
            _image.color = GetConstrastingColor(color);
        }

        if (_text != null) {
            _text.color = GetConstrastingColor(color);
        }
    }

    public Color GetConstrastingColor(Color color) {
        float luminance = GetLuminance(color);

        return luminance > 0.5f ? Color.black : Color.white;
    }

    private float GetLuminance(Color color) {
        return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
    }

}
