using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class ExportSettingsView : MonoBehaviour {

    [SerializeField] private TMP_InputField _framerate;
    [SerializeField] private TMP_InputField _outputFilePath;
    [SerializeField] private TMP_InputField _fileName;
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

        _export.onClick.AddListener(_exportSettingsViewModel.OnExport.Execute);
        _present.onClick.AddListener(_exportSettingsViewModel.OnPresent.Execute);
    }

    private void SetFramerate(string input) {
        if (int.TryParse(input, out var framerate)) {
            _exportSettingsViewModel.OnSetFramerate.Execute(framerate);
        }
    }

    private void SetOutputFilePath(string input) {
        input = _inputValidator.ValidateOutputFilePathInput(input);
        _exportSettingsViewModel.OnSetOutputFilePath.Execute(input);
    }

    private void SetFileName(string input) {
        _exportSettingsViewModel.OnSetFileName.Execute(input);
    }

}