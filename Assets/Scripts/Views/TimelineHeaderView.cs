using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public class TimelineHeaderView : MonoBehaviour {

	[SerializeField] private RectTransform _headerContent;
	[SerializeField] private RectTransform _headerHolder;
    [SerializeField] private GameObject _timelineHeaderPrefab;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

    }

    public void RefreshUI() {
        DrawHeader();
    }

    public void SetContentValue(float contentXPos) {
        _headerContent.anchoredPosition = new Vector2(contentXPos, _headerContent.anchoredPosition.y);
    }

    private void DrawHeader() {
        ConfigureTimelineHeaderSize();

        for (int i = 0; i < _videoTimelineViewModel.TotalFrameCount + 1; i++) {
			var header = Instantiate(_timelineHeaderPrefab, parent: _headerHolder.transform);
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
    }

}