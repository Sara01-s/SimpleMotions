using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class VideoSettingsView : MonoBehaviour {

    [SerializeField] private FlexibleColorPicker _flexibleColorPicker;
    [SerializeField] private TMP_InputField _framerate;
    [SerializeField] private Image _currentColor;
	[SerializeField] private AudioPlayer _audioPlayer;
	[SerializeField] private int _minFrameRate, _maxFrameRate;
    [SerializeField] private IconColor _colorPickerIcon;

    private IVideoSettingsViewModel _videoSettingsViewModel;
    private IVideoCanvasViewModel _videoCanvasViewModel;

    private bool _isSubmited;
    private bool _justOpened;

    public void Configure(IVideoSettingsViewModel videoSettingsViewModel, IVideoCanvasViewModel videoCanvasViewModel) { 
        _videoSettingsViewModel = videoSettingsViewModel;
        _videoCanvasViewModel = videoCanvasViewModel;

        _framerate.text = _videoSettingsViewModel.Framerate;

        _framerate.onValueChanged.AddListener(input => {
			// TODO - Ver cuando reproducir sonido de error.
            if (int.TryParse(input, out var value)) {
                if (value >= _minFrameRate && value <= _maxFrameRate) {
                    _videoSettingsViewModel.OnFramerateUpdate.Execute(input);
                }
                else if (value > _maxFrameRate) {
					_audioPlayer.PlayErrorSound();
                }
            }

            _isSubmited = false;
        });

        _framerate.onDeselect.AddListener(input => {
            if (int.TryParse(input, out var value)) {
                if (value < 12 && !_isSubmited) {
                    _audioPlayer.PlayErrorSound();
                }
            }
        });

        _framerate.onSubmit.AddListener(input => {
            if (int.TryParse(input, out var value)) {
                if (value < 12) {
                    _audioPlayer.PlayErrorSound();
                    _isSubmited = true;
                }
            }
        });
    }

    public void SubscribeToColorPicker() {
        SetColor();
        _flexibleColorPicker.SubscribeToColorChange(UpdateBackgroundColor);
    }

    private void UpdateBackgroundColor(Color color) {
        _videoCanvasViewModel.BackgroundColor.Value = new ColorDTO(color.r, color.g, color.b, color.a);
        _currentColor.color = color;

        _colorPickerIcon.SetIconColor(color);
    }

    private void SetColor() {
        var colorValues = _videoCanvasViewModel.BackgroundColor.Value;
        var color = new Color(colorValues.R, colorValues.G, colorValues.B, a: 100.0f); // Max opacity (opaque).

        _flexibleColorPicker.SetColorWithoutNotification(color);
        _currentColor.color = _flexibleColorPicker.Color;
    }

}