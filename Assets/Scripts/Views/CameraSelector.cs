using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class CameraSelector : MonoBehaviour {

    [SerializeField] private GameObject _panelBlocker;
    [SerializeField] private GameObject _exportingPanel;
    [SerializeField] private Camera _exportingCamera;
    [SerializeField] private Camera _defaultCamera;
    [SerializeField] private Canvas _editorCanvas;
    [SerializeField] private int _defaultSortingOrder = 2;

    public void Configure(ICameraSelectorViewModel cameraSelectorViewModel) {
        _exportingCamera.gameObject.SetActive(false);
        _exportingPanel.SetActive(false);
        _panelBlocker.SetActive(false);

        cameraSelectorViewModel.OnExportStarted.Subscribe(() => {
            _editorCanvas.worldCamera = _exportingCamera;
            _exportingCamera.gameObject.SetActive(true);
            _exportingPanel.SetActive(true);
            _panelBlocker.SetActive(true);

            _editorCanvas.sortingOrder = _defaultSortingOrder;
        });

        cameraSelectorViewModel.OnExportEnded.Subscribe(() => {
            _editorCanvas.worldCamera = _defaultCamera;
            _exportingCamera.gameObject.SetActive(false);
            _exportingPanel.SetActive(false);
            _panelBlocker.SetActive(false);
        });
    }

}