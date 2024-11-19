using UnityEngine.UI;
using UnityEngine;

public class FullscreenView : MonoBehaviour {

    [SerializeField] private Toggle _fullscreenToggle;

    [SerializeField] private RectTransform _videoCanvas;
    [SerializeField] private RectTransform _videoPlayback;
    [SerializeField] private GameObject _uiBlocker;

    [SerializeField] private GameObject _defaultCanvasParent;
    [SerializeField] private GameObject _defaultPlaybackParent;

	[SerializeField] private Camera _editorCamera;
    [SerializeField] private Canvas _editorCanvas;

    [SerializeField] private GameObject _fullscreenCanvasParent;
    [SerializeField] private GameObject _fullscreenPlaybackParent;

	[SerializeField] private Transform _videoCanvasOrigin;

    [SerializeField] private float _fullscreenWidth = 1920.0f;
    [SerializeField] private float _fullscreenHeight = 1080.0f;

    [SerializeField] private string _backgroundLayerName = "UI - BG";
    [SerializeField] private string _frontLayerName = "UI - Front";

    private Vector2 _defaultCanvasSize;
    private Vector2 _defaultPlaybackSize;
	private Vector3 _defaultCanvasOrigin;

    private float _defaultOrtographicSize;

    public void Configure() {
        _defaultCanvasSize = _videoCanvas.sizeDelta;
        _defaultPlaybackSize = _videoPlayback.sizeDelta;
        _defaultOrtographicSize = _editorCamera.orthographicSize;

        _fullscreenToggle.onValueChanged.AddListener(isFullscreen => {
            if (isFullscreen) {
                SetFullscreen();
                _uiBlocker.SetActive(true);
            }
            else {
                SetDefaultScreen();
                _uiBlocker.SetActive(false);
            }
        });

		CenterVideoCanvasOrigin();
		_defaultCanvasOrigin = _videoCanvasOrigin.transform.position;
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
        _videoCanvas.sizeDelta = new Vector2(_fullscreenWidth, _fullscreenHeight);

		CenterVideoCanvasOrigin();
		_editorCamera.orthographicSize = TranslateCameraToWorldPosition();

        _videoPlayback.anchoredPosition = Vector2.zero;
        _videoPlayback.sizeDelta = new Vector2(_fullscreenWidth, _videoPlayback.sizeDelta.y);
        _videoPlayback.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0.0f, _videoPlayback.rect.height);

        _editorCanvas.sortingLayerName = _backgroundLayerName;
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

		_videoCanvasOrigin.transform.position = _defaultCanvasOrigin;
		_editorCamera.orthographicSize = _defaultOrtographicSize;

        _videoPlayback.anchorMin = new Vector2(0.5f, 0.5f);
        _videoPlayback.anchorMax = new Vector2(0.5f, 0.5f);
        _videoPlayback.anchoredPosition = Vector2.zero;
        _videoPlayback.sizeDelta = _defaultPlaybackSize;

        _editorCanvas.sortingLayerName = _frontLayerName;
    }

    private float TranslateCameraToWorldPosition() {
        var newOrtographicSize = _defaultCanvasSize.x / _fullscreenWidth;
        return newOrtographicSize * _defaultOrtographicSize;
    }

}