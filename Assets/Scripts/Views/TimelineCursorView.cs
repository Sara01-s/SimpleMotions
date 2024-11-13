using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class TimelineCursorView : MonoBehaviour {

    [SerializeField] private RectTransform _framesHolder;
    [SerializeField] private Slider _cursor;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorValue);
        _cursor.onValueChanged.AddListener(value => _videoTimelineViewModel.OnSetCurrentFrame.Execute((int)value));
    }

    public void RefreshUI() {
        ConfigureCursor();
    }

    private void ConfigureCursor() {
        _cursor.value = 0;
        _cursor.maxValue = _videoTimelineViewModel.TotalFrameCount;
    }

    public void SetCursorValue(int currentFrame) {
        print(currentFrame);
        _cursor.value = currentFrame;
    }

    public void SetCursorValue(float value) {
        var desnormalizedValue = _videoTimelineViewModel.TotalFrameCount * value;
        _cursor.value = desnormalizedValue;
    }

}