using UnityEngine.UI;
using UnityEngine;

public class FullscreenView : MonoBehaviour {

	[SerializeField] private Camera _editorCamera;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private RectTransform _videoCanvas;
	[SerializeField] private Transform _videoCanvasOrigin;
    [SerializeField] private RectTransform _videoPlayback;

    [SerializeField] private Canvas _editorCanvas;
    [SerializeField] private GameObject _defaultCanvasParent;
    [SerializeField] private GameObject _defaultPlaybackParent;
    [SerializeField] private GameObject _fullscreenCanvasParent;
    [SerializeField] private GameObject _fullscreenPlaybackParent;

    [SerializeField] private GameObject _blocker;

    private Vector2 _defaultCanvasSize;
    private Vector2 _defaultPlaybackSize;

	private Vector3 _initCanvasOrigin;

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

		CenterVideoCanvasOrigin();
		_initCanvasOrigin = _videoCanvasOrigin.transform.position;
    }

	private void CenterVideoCanvasOrigin() {
		_videoCanvasOrigin.SetParent(_videoCanvas);
		_videoCanvasOrigin.localPosition = Vector2.zero; // _videoCanvas.rect.center (in world space)
		_videoCanvasOrigin.SetParent(null);
	}

    private void SetFullscreen() {
        _fullscreenPlaybackParent.SetActive(true);
        _fullscreenPlaybackParent.GetComponent<FullscreenPlaybackView>().IsFullscreen = true;

        _videoCanvas.SetParent(_fullscreenCanvasParent.transform);
        _videoPlayback.SetParent(_fullscreenPlaybackParent.transform);

        _videoCanvas.anchoredPosition = Vector2.zero;
        _videoCanvas.sizeDelta = new Vector2(1920, 1080);

		CenterVideoCanvasOrigin();
		_editorCamera.orthographicSize = 3.0f;

        _videoPlayback.anchoredPosition = Vector2.zero;
        _videoPlayback.sizeDelta = new Vector2(1920, 50);
        _videoPlayback.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, _videoPlayback.rect.height);

        _editorCanvas.sortingLayerName = "UI - BG";
    }

    private void SetDefaultScreen() {
        _fullscreenPlaybackParent.SetActive(false);
        _fullscreenPlaybackParent.GetComponent<FullscreenPlaybackView>().IsFullscreen = false;

        _videoCanvas.SetParent(_defaultCanvasParent.transform);
        _videoPlayback.SetParent(_defaultPlaybackParent.transform);

        _videoCanvas.anchorMin = new Vector2(0.5f, 0.5f);
        _videoCanvas.anchorMax = new Vector2(0.5f, 0.5f);
        _videoCanvas.anchoredPosition = Vector2.zero;
        _videoCanvas.sizeDelta = _defaultCanvasSize;

		_videoCanvasOrigin.transform.position = _initCanvasOrigin;
		_editorCamera.orthographicSize = 5.0f;

        _videoPlayback.anchorMin = new Vector2(0.5f, 0.5f);
        _videoPlayback.anchorMax = new Vector2(0.5f, 0.5f);
        _videoPlayback.anchoredPosition = Vector2.zero;
        _videoPlayback.sizeDelta = _defaultPlaybackSize;

        _editorCanvas.sortingLayerName = "UI - Front";
    }

}