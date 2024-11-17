using SimpleMotions;
using UnityEngine;

public class VideoSettingsView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;

    private IVideoCanvasViewModel _videoCanvasViewModel;
    private IEditorPainterParser _editorPainterParser;

    public void Configure(IVideoCanvasViewModel videoCanvasViewModel, IEditorPainterParser editorPainterParser) { 
        _videoCanvasViewModel = videoCanvasViewModel;
        _editorPainterParser = editorPainterParser;
    }

    public void SubscribeToColorPicker() {
        _flexibleColorPicker.SubscribeToColorChange(UpdateBackgroundColor);
    }

    private void UpdateBackgroundColor(Color color) {
        _videoCanvasViewModel.ChangeBackgroundColor(_editorPainterParser.UnityColorToSm(color));
    }

}