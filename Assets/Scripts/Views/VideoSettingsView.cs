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
    private IEditorPainterParser _editorPainterParser;

    public void Configure(IVideoSettingsViewModel videoSettingsViewModel, IVideoCanvasViewModel videoCanvasViewModel, IEditorPainterParser editorPainterParser) { 
        _videoSettingsViewModel = videoSettingsViewModel;
        _videoCanvasViewModel = videoCanvasViewModel;
        _editorPainterParser = editorPainterParser;

        _framerate.onValueChanged.AddListener(SetFramerate);
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(UpdateBackgroundColor);
    }

    private void UpdateBackgroundColor(Color color) {
        _videoCanvasViewModel.ChangeBackgroundColor(_editorPainterParser.UnityColorToSm(color));
        _currentColor.color = color;
    }

    private void SetFramerate(string newFramerate) {
        //_videoSettingsViewModel.OnFramerateUpdate.Execute(newFramerate);
    }

    private void UpdateFramerate(int currentFramerate) {
        _framerate.text = currentFramerate.ToString("0");
    }

}