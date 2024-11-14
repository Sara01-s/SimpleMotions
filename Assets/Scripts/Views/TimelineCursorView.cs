using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System;

public class TimelineCursorView : MonoBehaviour {

    [SerializeField] private RectTransform _sliderArea;
    [SerializeField] private GridLayoutGroup _framesHolder;
    [SerializeField] private Slider _cursor;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorValue);
        _cursor.onValueChanged.AddListener(value => _videoTimelineViewModel.OnSetCurrentFrame.Execute((int)value));

        RefreshUI();
    }

    public void RefreshUI() {
        ConfigureCursorAreaSize();
        ConfigureCursor();
    }

    private void ConfigureCursorAreaSize() {
		float totalWidth = _framesHolder.cellSize.x * _videoTimelineViewModel.TotalFrameCount + _framesHolder.cellSize.x;
		_sliderArea.sizeDelta = new Vector2(totalWidth, _cursor.GetComponent<RectTransform>().sizeDelta.y);
	}

    private void ConfigureCursor() {
        _cursor.value = 0;
        _cursor.maxValue = _videoTimelineViewModel.TotalFrameCount;
    }

    public void SetCursorValue(int currentFrame) {
        _cursor.value = currentFrame;
    }

    public void UpdateToCurrentFrame(float frameNormalized, float contentXPos) {
        _sliderArea.anchoredPosition = new Vector2(contentXPos - _framesHolder.cellSize.x, _sliderArea.anchoredPosition.y);
        
        var currentFrame = (int)Math.Floor(frameNormalized * _videoTimelineViewModel.TotalFrameCount);
        
        _videoTimelineViewModel.CurrentFrame.Value = currentFrame;
        _videoTimelineViewModel.OnSetCurrentFrame.Execute(currentFrame);
    }

}