using SimpleMotions;
using UnityEngine;

public class ColorPickerView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;

    private IColorPickerViewModel _colorPickerViewModel;

    public void Configure(IColorPickerViewModel colorPickerViewModel, FlexibleColorPicker flexibleColorPicker) {
        _colorPickerViewModel = colorPickerViewModel;
        _flexibleColorPicker = flexibleColorPicker;

        _flexibleColorPicker.OnColorChange.AddListener(GetColor);
    }

    private void GetColor(Color color) {
        _colorPickerViewModel.SetColorToEntity((color.r, color.g, color.b, color.a));
    }

}