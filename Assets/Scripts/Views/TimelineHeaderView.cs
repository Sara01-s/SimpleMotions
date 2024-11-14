using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class TimelineHeaderView : MonoBehaviour {

    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private RectTransform _content;
	[SerializeField] private RectTransform _headerHolder;
    [SerializeField] private GameObject _headerPrefab;
    [SerializeField] private TimelineCursorView _timelineCursorView;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        RefreshUI();
    }

    public void RefreshUI() {
        DrawHeader();
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

		float totalWidth = gridLayout.cellSize.x * _videoTimelineViewModel.TotalFrameCount + (gridLayout.cellSize.x * 2.0f);

		_content.sizeDelta = new Vector2(totalWidth, _content.sizeDelta.y);
        _headerHolder.sizeDelta = new Vector2(totalWidth, _headerHolder.sizeDelta.y);
        _scrollbar.GetComponent<RectTransform>().sizeDelta = new Vector2(totalWidth - gridLayout.cellSize.x, _scrollbar.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void UpdateHeaderPositions(float scrollbarValue, float contentXPos) {
        _content.anchoredPosition = new Vector2(contentXPos, _content.anchoredPosition.y);
        _scrollbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(contentXPos, _scrollbar.GetComponent<RectTransform>().anchoredPosition.y);
        _scrollbar.value = scrollbarValue;
    }

    public void OnScrollbarValueChanged(float normalizedValue) {
        if (RectTransformUtility.RectangleContainsScreenPoint(_scrollbar.GetComponent<RectTransform>(), Input.mousePosition, Camera.main)) {
            float normalizedPosition = GetCursorNormalizedPosition();
            _scrollbar.value = normalizedPosition;
        }

        var value = normalizedValue * _videoTimelineViewModel.TotalFrameCount;
        _timelineCursorView.SetCursorValue((int)Mathf.Floor(value));
    }

    private float GetCursorNormalizedPosition() {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_scrollbar.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPoint);
        float normalizedPosition = Mathf.Clamp01(localPoint.x / _scrollbar.GetComponent<RectTransform>().rect.width);
        
        return normalizedPosition;
    }

}