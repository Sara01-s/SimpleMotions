using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class ExportSettingsView : MonoBehaviour {

    [SerializeField] private TMP_InputField _framerate;
    [SerializeField] private Button _export;
    [SerializeField] private Button _present;

    private IExportSettingsViewModel _exportSettingsViewModel;

    public void Configure(IExportSettingsViewModel exportSettingsViewModel) {
        _exportSettingsViewModel = exportSettingsViewModel;

        _framerate.text = _exportSettingsViewModel.Framerate;
        _framerate.onValueChanged.AddListener(SetFramerate);

        _export.onClick.AddListener(_exportSettingsViewModel.OnExport.Execute);
        _present.onClick.AddListener(_exportSettingsViewModel.OnPresent.Execute);
    }

    private void SetFramerate(string newFramerate) {
        if (int.TryParse(newFramerate, out var framerate)) {
            _exportSettingsViewModel.OnFramerateUpdate.Execute(framerate);
        }
    }

}