using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class VideoSettingsView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;
    [SerializeField] private TMP_InputField _framerate;
    [SerializeField] private Image _currentColor;

    private IVideoSettingsViewModel _videoSettingsViewModel;
    private IVideoCanvasViewModel _videoCanvasViewModel;

    public void Configure(IVideoSettingsViewModel videoSettingsViewModel, IVideoCanvasViewModel videoCanvasViewModel) { 
        _videoSettingsViewModel = videoSettingsViewModel;
        _videoCanvasViewModel = videoCanvasViewModel;

        _framerate.text = _videoSettingsViewModel.Framerate;

        _framerate.onValueChanged.AddListener(input => {
            if (int.TryParse(input, out var value)) {
                if (value >= 12 && value <= 144) {
                    _videoSettingsViewModel.OnFramerateUpdate.Execute(input);
                }
                else {
                    // TODO - Feedback de error (sonido, etc)
                }
            }
        });
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(UpdateBackgroundColor);
    }

    private void UpdateBackgroundColor(Color color) {
        _videoCanvasViewModel.BackgroundColor.Value = (color.r, color.g, color.b, color.a); 
        _currentColor.color = color;
    }

}