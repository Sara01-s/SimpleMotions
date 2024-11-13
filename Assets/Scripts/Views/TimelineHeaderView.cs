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

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _headerScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        RefreshUI();
    }

    public void RefreshUI() {
        DrawHeader();
    }

    public void UpdateHeaderPositions(float contentXPos) {
        _headerContent.anchoredPosition = new Vector2(contentXPos, _headerContent.anchoredPosition.y);
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

        //_headerScrollbar.GetComponent<RectTransform>() = new Vector2(totalWidth, _headerScrollbar.GetComponent<RectTransform>().anchoredPosition.y);
        _headerScrollbar.size /= _videoTimelineViewModel.TotalFrameCount;
        print(_headerScrollbar.size);
    }

    public void OnScrollbarValueChanged(float normalizedValue) {
        var desnormalizedValue = normalizedValue * _videoTimelineViewModel.TotalFrameCount;
        var value = (int)Mathf.Floor(desnormalizedValue);


        //_timelineCursorView.SetCursorValue(value);
    }

}