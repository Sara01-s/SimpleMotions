using UnityEngine.UI;
using UnityEngine;

public class IconColor : MonoBehaviour {

    [SerializeField] private Image _colorPickerIcon;

    public void SetIconColor(Color color) {
        _colorPickerIcon.color = GetConstrastingColor(color);
    }

    public Color GetConstrastingColor(Color color) {
        float luminance = GetLuminance(color);

        return luminance > 0.5f ? Color.black : Color.white;
    }

    private float GetLuminance(Color color) {
        return 0.2126f * color.r + 0.7152f * color.g + 0.0722f * color.b;
    }

}
