using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class ExportSettingsView : MonoBehaviour {

    [SerializeField] private TMP_InputField _framerate;
    [SerializeField] private TMP_InputField _outputFilePath;
    [SerializeField] private Button _export;
    [SerializeField] private Button _present;

    private IExportSettingsViewModel _exportSettingsViewModel;

    public void Configure(IExportSettingsViewModel exportSettingsViewModel) {
        _exportSettingsViewModel = exportSettingsViewModel;

        _framerate.text = _exportSettingsViewModel.Framerate.Value.ToString();

        _exportSettingsViewModel.Framerate.Subscribe(framerate => _framerate.text = framerate.ToString());

        _framerate.onValueChanged.AddListener(SetFramerate);
        _outputFilePath.onValueChanged.AddListener(SetOutputFilePath);

        _export.onClick.AddListener(_exportSettingsViewModel.OnExport.Execute);
        _present.onClick.AddListener(_exportSettingsViewModel.OnPresent.Execute);
        
        _outputFilePath.onSubmit.AddListener(value => _exportSettingsViewModel.OnExport.Execute());
    }

    private void SetFramerate(string newFramerate) {
        if (int.TryParse(newFramerate, out var framerate)) {
            _exportSettingsViewModel.OnSetFramerate.Execute(framerate);
        }
    }

    private void SetOutputFilePath(string newOutputFilePath) {
        _exportSettingsViewModel.OnSetOutputFilePath.Execute(newOutputFilePath);
    }

}