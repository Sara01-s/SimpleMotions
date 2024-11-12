using UnityEngine;

public class ColorPickerView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;

    public void Configure(FlexibleColorPicker flexibleColorPicker) {
        _flexibleColorPicker = flexibleColorPicker;


    }

    

}