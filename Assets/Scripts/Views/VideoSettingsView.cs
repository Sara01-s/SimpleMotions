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

        _framerate.onValueChanged.AddListener(_videoSettingsViewModel.OnFramerateUpdate.Execute);
        _framerate.text = _videoSettingsViewModel.Framerate;
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(UpdateBackgroundColor);
    }

    private void UpdateBackgroundColor(Color color) {
        _videoCanvasViewModel.BackgroundColor.Value = (color.r, color.g, color.b, color.a); 
        _currentColor.color = color;
    }

}