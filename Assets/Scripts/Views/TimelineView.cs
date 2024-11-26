using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System.Collections.Generic;

public sealed class TimelineView : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Scrollbar _horizontalScrollbar;

	[Header("Draw")]
	[SerializeField] private RectTransform _content;
	[SerializeField] private RectTransform _framesHolder;

	[Header("Relationed Views")]
	[SerializeField] private TimelineHeaderView _headerView;
	[SerializeField] private TimelineCursorView _cursorView;

	private IVideoTimelineViewModel _videoTimelineViewModel;

	private List<GameObject> _timeline = new();

	private bool _alreadyPainted;

	public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
		_videoTimelineViewModel = videoTimelineViewModel;

		_horizontalScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
	}

	public void RefreshUI() {
		DrawTimeline();
		_headerView.RefreshUI();
		_cursorView.RefreshUI();
	}

	private void OnScrollbarValueChanged(float frameNormalized) {
		_headerView.UpdateHeaderPositions(_content.anchoredPosition.x);
		_cursorView.UpdateSliderArea(_content.anchoredPosition.x);
	}


	private void DrawTimeline() {
		ConfigureTimelineSize();
	}

	private void ConfigureTimelineSize() {
		if (_alreadyPainted) {
			DeleteTimeline();
		}

		var gridLayout = _framesHolder.GetComponent<GridLayoutGroup>();

		float totalWidth = gridLayout.cellSize.x * _videoTimelineViewModel.TotalFrameCount + (gridLayout.cellSize.x * 2);

		_content.sizeDelta = new Vector2(totalWidth, gridLayout.cellSize.y);
		_framesHolder.sizeDelta = new Vector2(totalWidth, _framesHolder.sizeDelta.y);

		_alreadyPainted = true;
	}

	private void DeleteTimeline() {
		foreach (var frame in _timeline) {
			Destroy(frame);
		}

		_timeline.Clear();
		_alreadyPainted = false;
	}

}