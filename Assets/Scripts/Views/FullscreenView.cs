using UnityEngine.UI;
using UnityEngine;
using SimpleMotions;

public class FullscreenView : MonoBehaviour {

    [SerializeField] private Toggle _fullscreenToggle;
	[SerializeField] private GameObject _worldMouseBlocker;

    [SerializeField] private RectTransform _videoCanvas;
    [SerializeField] private RectTransform _videoPlayback;
    [SerializeField] private RectTransform _background;

    [SerializeField] private GameObject _defaultCanvasParent;
    [SerializeField] private GameObject _defaultPlaybackParent;

	[SerializeField] private Camera _editorCamera;

    [SerializeField] private Canvas _editorCanvas;

    [SerializeField] private GameObject _fullscreenCanvasParent;
    [SerializeField] private GameObject _fullscreenPlaybackParent;

	[SerializeField] private Transform _videoCanvasOrigin;

    [SerializeField] private float _fullscreenWidth = 1920.0f;
    [SerializeField] private float _fullscreenHeight = 1080.0f;

    [SerializeField] private int _fullscreenSortingOrder = -2;
    [SerializeField] private int _defaultScreenSortingOrder = 2;

    [SerializeField] private float _defaultOrtographicSize = 10.0f;

    private Vector2 _defaultCanvasSize;
    private Vector2 _defaultPlaybackSize;
	private Vector3 _defaultCanvasOrigin;
    private Vector3 _defaultBackgroundSize;
    private Vector3 _defaultBackgroundPosition;

    private IFullscreenViewModel _fullscreenViewModel;

    public void Configure(IFullscreenViewModel fullscreenViewModel) {
        _defaultCanvasSize = _videoCanvas.sizeDelta;
        _defaultPlaybackSize = _videoPlayback.sizeDelta;
        _defaultBackgroundSize = _background.sizeDelta;
        _defaultBackgroundPosition = _background.anchoredPosition;

        _fullscreenToggle.onValueChanged.AddListener(isFullscreen => {
            if (isFullscreen) {
                SetFullscreen(withPlayback: true);
            }
            else {
                SetDefaultScreen();
            }
        });

		CenterVideoCanvasOrigin();
		_defaultCanvasOrigin = _videoCanvasOrigin.transform.position;
        _fullscreenViewModel = fullscreenViewModel;
    }

	private void CenterVideoCanvasOrigin() {
		_videoCanvasOrigin.SetParent(_videoCanvas);
		_videoCanvasOrigin.localPosition = Vector2.zero; // _videoCanvas.rect.center (in world space)
		_videoCanvasOrigin.SetParent(null);
	}

    public void SetFullscreen(bool withPlayback) {
        if (!withPlayback) {
            _videoPlayback.gameObject.SetActive(false);
			_editorCanvas.sortingLayerID = SortingLayer.NameToID("UI - Back");
        }
		else {
			_editorCanvas.sortingLayerID = SortingLayer.NameToID("Default");
		}

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

        _background.anchoredPosition = Vector2.zero;
        _background.sizeDelta = new Vector2(_fullscreenWidth, _fullscreenHeight);

        _editorCanvas.sortingOrder = _fullscreenSortingOrder;
		_worldMouseBlocker.SetActive(false);

        _fullscreenViewModel.OnFullscreen.Execute();
    }

    public void SetDefaultScreen() {
        _videoPlayback.gameObject.SetActive(true);
        _fullscreenPlaybackParent.SetActive(false);
        _fullscreenPlaybackParent.GetComponent<FullscreenPlaybackView>().IsFullscreen = false;

        _videoCanvas.SetParent(_defaultCanvasParent.transform);
		_videoCanvas.SetAsFirstSibling();
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

        _background.sizeDelta = _defaultBackgroundSize;
        _background.anchoredPosition = _defaultBackgroundPosition;

		_editorCanvas.sortingLayerID = SortingLayer.NameToID("UI - Back");
        _editorCanvas.sortingOrder = _defaultScreenSortingOrder;
		_worldMouseBlocker.SetActive(true);

        _fullscreenViewModel.OnFullscreen.Execute();
    }

    private float TranslateCameraToWorldPosition() {
        var newOrtographicSize = _defaultCanvasSize.x / _fullscreenWidth;
        return newOrtographicSize * _defaultOrtographicSize;
    }

}