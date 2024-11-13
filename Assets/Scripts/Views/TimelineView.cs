using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
    
public sealed class TimelineView : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Button _createTestEntity;
	[SerializeField] private Sprite _whiteTexture;
	[SerializeField] private Scrollbar _horizontalTimelineScrollbar;

	[Header("Draw")]
	[SerializeField] private RectTransform _timelineContent;
	[SerializeField] private RectTransform _framesHolder;
	[SerializeField] private GameObject _framePrefab;

	[Header("Relationed Views")]
	[SerializeField] private TimelineHeaderView _timelineHeaderView;
	[SerializeField] private TimelineCursorView _timelineCursorView;

	private IVideoTimelineViewModel _videoTimelineViewModel;

	public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
		_videoTimelineViewModel = videoTimelineViewModel;

		_horizontalTimelineScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

		RefreshUI();
	}

	private void RefreshUI() {
		_videoTimelineViewModel.RefreshData();
		DrawTimeline();
		
		_timelineHeaderView.RefreshUI();
		_timelineCursorView.RefreshUI();
	}

	private void OnScrollbarValueChanged(float value) {
		_timelineHeaderView.SetContentValue(_timelineContent.anchoredPosition.x);
		_timelineCursorView.SetCursorValue(value);
	}

	private void DrawTimeline() {
		ConfigureTimelineSize();

		for (int i = 0; i < _videoTimelineViewModel.TotalFrameCount; i++) {
			var frame = Instantiate(_framePrefab, parent: _framesHolder.transform);
			frame.name = $"Frame_{i}";
		}
	}

	private void ConfigureTimelineSize() {
		var gridLayout = _framesHolder.GetComponent<GridLayoutGroup>();

		float timelineWidth = gridLayout.cellSize.x * _videoTimelineViewModel.TotalFrameCount + gridLayout.cellSize.x;

		_timelineContent.sizeDelta = new Vector2(timelineWidth, gridLayout.cellSize.y);
		_framesHolder.sizeDelta = new Vector2(timelineWidth, _framesHolder.sizeDelta.y);
	}

}