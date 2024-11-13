using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class TimelineHeaderView : MonoBehaviour {

    [SerializeField] private RectTransform _headerContent;
	[SerializeField] private RectTransform _headerHolder;
    [SerializeField] private Scrollbar _headerScrollbar;
    [SerializeField] private GameObject _headerPrefab;

    [SerializeField] private TimelineCursorView _timelineCursorView;

    private IVideoTimelineViewModel _videoTimelineViewModel;
    private RectTransform _scrollbarPosition;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _scrollbarPosition = _headerScrollbar.GetComponent<RectTransform>();

        _headerScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        RefreshUI();
    }

    public void RefreshUI() {
        DrawHeader();
    }

    public void UpdateHeaderPositions(float scrollbarValue, float contentXPos) {
        _headerContent.anchoredPosition = new Vector2(contentXPos, _headerContent.anchoredPosition.y);
        _scrollbarPosition.anchoredPosition = new Vector2(contentXPos, _scrollbarPosition.anchoredPosition.y);
        _headerScrollbar.value = scrollbarValue;
    }

    private void DrawHeader() {
        ConfigureTimelineHeaderSize();

        for (int i = 0; i < _videoTimelineViewModel.TotalFrameCount + 1; i++) {
			var header = Instantiate(_headerPrefab, parent: _headerHolder.transform);
            header.name = $"Header_{i}";

            var headerNumber = header.GetComponentInChildren<TextMeshProUGUI>();
			headerNumber.text = i.ToString();
		}
    }

    private void ConfigureTimelineHeaderSize() {
        var gridLayout = _headerHolder.GetComponent<GridLayoutGroup>();

		float totalWidth = gridLayout.cellSize.x * _videoTimelineViewModel.TotalFrameCount + (gridLayout.cellSize.x * 2);

		_headerContent.sizeDelta = new Vector2(totalWidth, _headerContent.sizeDelta.y);
        _headerHolder.sizeDelta = new Vector2(totalWidth, _headerHolder.sizeDelta.y);

        _scrollbarPosition.sizeDelta = new Vector2(totalWidth - 42, _scrollbarPosition.sizeDelta.y);
    }

    public void OnScrollbarValueChanged(float normalizedValue) {
        if (RectTransformUtility.RectangleContainsScreenPoint(_scrollbarPosition, Input.mousePosition, Camera.main)) {
            float normalizedPosition = GetCursorNormalizedPosition();
            _headerScrollbar.value = normalizedPosition;
        }

        var desnormalizedValue = normalizedValue * _videoTimelineViewModel.TotalFrameCount;
        var value = (int)Mathf.Floor(desnormalizedValue);
        _timelineCursorView.SetCursorValue(value);
    }

    private float GetCursorNormalizedPosition() {
        // Convierte la posición del cursor a un valor entre 0 y 1
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_scrollbarPosition, Input.mousePosition, Camera.main, out Vector2 localPoint);

        // Normaliza el valor para que esté entre 0 (inicio) y 1 (fin)
        float normalizedPosition = Mathf.Clamp01(localPoint.x / _scrollbarPosition.rect.width);
        
        return normalizedPosition;
    }

}