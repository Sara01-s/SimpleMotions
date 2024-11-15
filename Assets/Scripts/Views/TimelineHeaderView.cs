using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TimelineHeaderView : MonoBehaviour {

    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private RectTransform _content;
	[SerializeField] private RectTransform _headerHolder;
    [SerializeField] private GameObject _headerPrefab;
    [SerializeField] private TimelineCursorView _timelineCursorView;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    private List<GameObject> _header = new();
    private bool _alreadyPainted;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
    }

    public void RefreshUI() {
        DrawHeader();
    }

    private void DrawHeader() {
        ConfigureTimelineHeaderSize();

        for (int i = 0; i < _videoTimelineViewModel.TotalFrameCount + 1; i++) {
			var header = Instantiate(_headerPrefab, parent: _headerHolder.transform);
            header.name = $"Header_{i}";

            _header.Add(header);

            var headerNumber = header.GetComponentInChildren<TextMeshProUGUI>();
			headerNumber.text = i.ToString();
		}
    }

    private void ConfigureTimelineHeaderSize() {
        if (_alreadyPainted) {
			DeleteTimeline();
		}

        var gridLayout = _headerHolder.GetComponent<GridLayoutGroup>();

		float totalWidth = gridLayout.cellSize.x * _videoTimelineViewModel.TotalFrameCount + (gridLayout.cellSize.x * 2);

		_content.sizeDelta = new Vector2(totalWidth, _content.sizeDelta.y);
        _headerHolder.sizeDelta = new Vector2(totalWidth, _headerHolder.sizeDelta.y);
        _scrollbar.GetComponent<RectTransform>().sizeDelta = new Vector2(totalWidth - gridLayout.cellSize.x, _scrollbar.GetComponent<RectTransform>().sizeDelta.y);

        _alreadyPainted = true;
    }

    private void DeleteTimeline() {
		foreach (var frame in _header) {
			Destroy(frame);
		}

		_header.Clear();
		_alreadyPainted = false;
	}

    public void UpdateHeaderPositions(float contentXPos) {
        _content.anchoredPosition = new Vector2(contentXPos, _content.anchoredPosition.y);
        _scrollbar.GetComponent<RectTransform>().anchoredPosition = new Vector2(contentXPos, _scrollbar.GetComponent<RectTransform>().anchoredPosition.y);
    }

    public void OnScrollbarValueChanged(float normalizedValue) {
        if (RectTransformUtility.RectangleContainsScreenPoint(_scrollbar.GetComponent<RectTransform>(), Input.mousePosition, Camera.main)) {
            float normalizedPosition = GetCursorNormalizedPosition();
            _scrollbar.value = normalizedPosition;
        }

        var value = normalizedValue * _videoTimelineViewModel.TotalFrameCount;
        _timelineCursorView.SetCursorNewFrame((int)Mathf.Floor(value));
    }

    private float GetCursorNormalizedPosition() {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_scrollbar.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out Vector2 localPoint);
        float normalizedPosition = Mathf.Clamp01(localPoint.x / _scrollbar.GetComponent<RectTransform>().rect.width);
        
        return normalizedPosition;
    }

}