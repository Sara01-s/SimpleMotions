using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;
    
public sealed class VideoTimelineView : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Button _createTestEntity;
	[SerializeField] private Slider _cursor;
	[SerializeField] private Sprite _whiteTexture;

	[Header("Draw")]
	[SerializeField] private GameObject _content;
	[SerializeField] private GameObject _framesHolder;
	[SerializeField] private GameObject _framePrefab;

	private IVideoTimelineViewModel _videoTimelineViewModel;

	public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
		_videoTimelineViewModel = videoTimelineViewModel; 

		_createTestEntity.onClick.AddListener(() => videoTimelineViewModel.OnCreateTestEntity.Execute(null));
		_cursor.onValueChanged.AddListener(value => videoTimelineViewModel.OnSetCurrentFrame.Execute((int)value));

		_videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorValue);

		RefreshUI();
	}

	private void RefreshUI() {
		_videoTimelineViewModel.RefreshData();
		DrawTimeline(_videoTimelineViewModel.TotalFrameCount);
	}

	private void DrawTimeline(int framesCount) {
		ConfigureCursor(framesCount);
		ConfigureTimelineSize(framesCount);

		for (int i = 0; i < framesCount; i++) {
			var frame = Instantiate(_framePrefab, parent: _framesHolder.transform);
			var frameNumber = frame.GetComponentInChildren<TextMeshProUGUI>();

			frame.name = $"Frame_{i}";
			frameNumber.text = i.ToString();
		}
	}

	private void ConfigureCursor(int framesCount) {
		_cursor.value = 0;
		_cursor.maxValue = framesCount;
	}

	private void ConfigureTimelineSize(int framesCount) {
		var content = _content.GetComponent<RectTransform>();
		var holder = _framesHolder.GetComponent<RectTransform>();
		var cursor = _cursor.GetComponent<RectTransform>();
		var gridLayout = _framesHolder.GetComponent<GridLayoutGroup>();
		float frameWidth = gridLayout.cellSize.x;
		float timelineWidth = frameWidth * (framesCount + 1); // TODO - SUS

		cursor.sizeDelta = new Vector3(timelineWidth, cursor.sizeDelta.y);
		content.sizeDelta = new Vector2(timelineWidth, content.sizeDelta.y);
		holder.sizeDelta = new Vector2(timelineWidth, holder.sizeDelta.y);
	}

	private void SetCursorValue(int currentFrame) {
		_cursor.value = currentFrame;
	}

}