using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class TimelineCursorView : MonoBehaviour {

    [SerializeField] private RectTransform _framesHolder;
    [SerializeField] private RectTransform _sliderArea;
    [SerializeField] private Slider _cursor;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorValue);
        _cursor.onValueChanged.AddListener(value => _videoTimelineViewModel.OnSetCurrentFrame.Execute((int)value));
    }

    public void RefreshUI() {
        ConfigureCursorAreaSize();
        ConfigureCursor();
    }

    private void ConfigureCursorAreaSize() {
		var gridLayout = _framesHolder.GetComponent<GridLayoutGroup>();

		float totalWidth = gridLayout.cellSize.x * _videoTimelineViewModel.TotalFrameCount + gridLayout.cellSize.x;

		_sliderArea.sizeDelta = new Vector2(totalWidth, _cursor.GetComponent<RectTransform>().sizeDelta.y);
	}

    private void ConfigureCursor() {
        _cursor.value = 0;
        _cursor.maxValue = _videoTimelineViewModel.TotalFrameCount;
    }

    public void SetCursorValue(int currentFrame) {
        _cursor.value = currentFrame;
    }

    public void SetCursorValue(float value, float contentXPos) {
        var desnormalizedValue = _videoTimelineViewModel.TotalFrameCount * value;
        _cursor.value = desnormalizedValue;

        _sliderArea.anchoredPosition = new Vector2(contentXPos - 42, _sliderArea.anchoredPosition.y);
    }

}