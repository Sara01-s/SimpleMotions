using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;
using SFB;

public class ExportSettingsView : MonoBehaviour {

    [SerializeField] private TMP_InputField _framerate;
    [SerializeField] private TMP_InputField _outputFilePath;
    [SerializeField] private TMP_InputField _fileName;
	[SerializeField] private Button _openFileExplorer;
    [SerializeField] private Button _export;
    [SerializeField] private AudioPlayer _audioPlayer;

    private IExportSettingsViewModel _exportSettingsViewModel;
    private IInputValidator _inputValidator;

    private bool _isOutputValid;
    private bool _isNameValid;

    public void Configure(IExportSettingsViewModel exportSettingsViewModel, IInputValidator inputValidator) {
        _exportSettingsViewModel = exportSettingsViewModel;
        _inputValidator = inputValidator;

        _framerate.text = _exportSettingsViewModel.Framerate.Value.ToString();
        _exportSettingsViewModel.Framerate.Subscribe(framerate => _framerate.text = framerate.ToString());

        _framerate.onValueChanged.AddListener(SetFramerate);
        _outputFilePath.onValueChanged.AddListener(SetOutputFilePath);
        _fileName.onValueChanged.AddListener(SetFileName);

		_openFileExplorer.onClick.AddListener(SetOutputDirectory);

        _export.onClick.AddListener(() => {
            if (_isNameValid && _isOutputValid) {
                _exportSettingsViewModel.OnExport.Execute();
            }
            else {
                _audioPlayer.PlayErrorSound();
            }
        });
    }

	private void SetOutputDirectory() {
		string[] outputDirectory = StandaloneFileBrowser.OpenFolderPanel (
			title: "Select Output Directory",
			directory: string.Empty,
			multiselect: false
		);

		if (_inputValidator.ValidateDirectory(outputDirectory[0])) {
			SetOutputFilePath(outputDirectory[0]);
		}
        else {
            _audioPlayer.PlayErrorSound();
            _isOutputValid = false;
        }
	}

    private void SetFramerate(string input) {
        if (int.TryParse(input, out var framerate)) {
            _exportSettingsViewModel.OnSetFramerate.Execute(framerate);
        }
    }

    private void SetOutputFilePath(string filePath) {
        _exportSettingsViewModel.OnSetOutputFilePath.Execute(filePath);
        _outputFilePath.text = filePath;
        _isOutputValid = true;
    }

    private void SetFileName(string fileName) {
        if (_inputValidator.ValidateFileName(fileName)) {
            _exportSettingsViewModel.OnSetFileName.Execute(fileName);
            _isNameValid = true;
        }
        else {
            _isNameValid = false;
        }
    }

}