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
    [SerializeField] private Button _present;

    private IExportSettingsViewModel _exportSettingsViewModel;

    private IInputValidator _inputValidator;

    public void Configure(IExportSettingsViewModel exportSettingsViewModel, IInputValidator inputValidator) {
        _exportSettingsViewModel = exportSettingsViewModel;
        _inputValidator = inputValidator;

        _framerate.text = _exportSettingsViewModel.Framerate.Value.ToString();
        _exportSettingsViewModel.Framerate.Subscribe(framerate => _framerate.text = framerate.ToString());

        _framerate.onValueChanged.AddListener(SetFramerate);
        _outputFilePath.onValueChanged.AddListener(SetOutputFilePath);
        _fileName.onValueChanged.AddListener(SetFileName);

		_openFileExplorer.onClick.AddListener(SetOutputDirectory);
        _export.onClick.AddListener(_exportSettingsViewModel.OnExport.Execute);
        _present.onClick.AddListener(_exportSettingsViewModel.OnPresent.Execute);
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
	}

    private void SetFramerate(string input) {
        if (int.TryParse(input, out var framerate)) {
            _exportSettingsViewModel.OnSetFramerate.Execute(framerate);
        }
    }

    private void SetOutputFilePath(string filePath) {
		print(filePath);
        _exportSettingsViewModel.OnSetOutputFilePath.Execute(filePath);
    }

    private void SetFileName(string input) {
        _exportSettingsViewModel.OnSetFileName.Execute(input);
    }

}