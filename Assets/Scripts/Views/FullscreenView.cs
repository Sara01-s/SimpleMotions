using UnityEngine.UI;
using UnityEngine;

public class FullscreenView : MonoBehaviour {

    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private RectTransform _videoCanvas;
    [SerializeField] private RectTransform _videoPlayback;

    [SerializeField] private Canvas _editorCanvas;
    [SerializeField] private GameObject _defaultCanvasParent;
    [SerializeField] private GameObject _defaultPlaybackParent;
    [SerializeField] private GameObject _fullscreenCanvas;

    [SerializeField] private GameObject _blocker;

    private Vector2 _defaultCanvasSize;
    private Vector2 _defaultPlaybackSize;

    public void Configure() {
        _defaultCanvasSize = _videoCanvas.sizeDelta;
        _defaultPlaybackSize = _videoPlayback.sizeDelta;

        _fullscreenToggle.onValueChanged.AddListener(isFullscreen => {
            if (isFullscreen) {
                SetFullscreen();
                _blocker.SetActive(true);
            }
            else {
                SetDefaultScreen();
                _blocker.SetActive(false);
            }
        });
    }

    private void SetFullscreen() {
        _videoCanvas.SetParent(_fullscreenCanvas.transform);
        _videoPlayback.SetParent(_fullscreenCanvas.transform);

        _videoCanvas.anchoredPosition = Vector2.zero;
        _videoCanvas.sizeDelta = new Vector2(1920, 1030);
        _videoCanvas.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, _videoCanvas.rect.height);

        _videoPlayback.anchoredPosition = Vector2.zero;
        _videoPlayback.sizeDelta = new Vector2(1920, 50);
        _videoPlayback.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, _videoPlayback.rect.height);

        _editorCanvas.sortingLayerName = "UI - BG";
    }

    private void SetDefaultScreen() {
        _videoCanvas.SetParent(_defaultCanvasParent.transform);
        _videoPlayback.SetParent(_defaultPlaybackParent.transform);

        _videoCanvas.anchorMin = new Vector2(0.5f, 0.5f);
        _videoCanvas.anchorMax = new Vector2(0.5f, 0.5f);
        _videoCanvas.anchoredPosition = Vector2.zero;
        _videoCanvas.sizeDelta = _defaultCanvasSize;

        _videoPlayback.anchorMin = new Vector2(0.5f, 0.5f);
        _videoPlayback.anchorMax = new Vector2(0.5f, 0.5f);
        _videoPlayback.anchoredPosition = Vector2.zero;
        _videoPlayback.sizeDelta = _defaultPlaybackSize;

        _editorCanvas.sortingLayerName = "UI - Front";
    }

}