using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public sealed class TimelineView : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Scrollbar _horizontalScrollbar;

	[Header("Draw")]
	[SerializeField] private RectTransform _content;
	[SerializeField] private RectTransform _framesHolder;
	[SerializeField] private GameObject _framePrefab;

	[Header("Relationed Views")]
	[SerializeField] private TimelineHeaderView _headerView;
	[SerializeField] private TimelineCursorView _cursorView;

	private IVideoTimelineViewModel _videoTimelineViewModel;

	public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
		_videoTimelineViewModel = videoTimelineViewModel;

		_horizontalScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

		RefreshUI();
	}

	private void RefreshUI() {
		DrawTimeline();
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

		float totalWidth = gridLayout.cellSize.x * _videoTimelineViewModel.TotalFrameCount + gridLayout.cellSize.x;

		_content.sizeDelta = new Vector2(totalWidth, gridLayout.cellSize.y);
		_framesHolder.sizeDelta = new Vector2(totalWidth, _framesHolder.sizeDelta.y);
	}

	private void OnScrollbarValueChanged(float frameNormalized) {
		_headerView.UpdateHeaderPositions(_content.anchoredPosition.x);
		_cursorView.UpdateSliderArea(_content.anchoredPosition.x);
	}

}