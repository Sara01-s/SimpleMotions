using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class TimelineCursorView : MonoBehaviour {

    [SerializeField] private Transform _cursorHandle;
    [SerializeField] private GameObject _keyframePrefab;
    [SerializeField] private RectTransform _sliderArea;
    [SerializeField] private GridLayoutGroup _framesHolder;
    [SerializeField] private Slider _cursor;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _cursor.onValueChanged.AddListener(value => _videoTimelineViewModel.OnFrameChanged.Execute((int)value));

        _videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorNewFrame);

        RefreshUI();
		
        //XD
        videoTimelineViewModel.ShowKeyframe.Subscribe(() => {
            var keyframe = Instantiate(_keyframePrefab, parent: _cursorHandle);
            var rect = keyframe.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 55.0f);
            rect.transform.SetParent(_cursor.transform);
        });
    }

    public void UpdateSliderArea(float contentXPos) {
        _sliderArea.anchoredPosition = new Vector2(contentXPos - _framesHolder.cellSize.x, _sliderArea.anchoredPosition.y);
    }

    public void SetCursorNewFrame(int currentFrame) {
        _cursor.value = currentFrame;
    }

    public void RefreshUI() {
        ConfigureCursorAreaSize();
        ConfigureCursor();
    }

    private void ConfigureCursorAreaSize() {
		float totalWidth = _framesHolder.cellSize.x * _videoTimelineViewModel.TotalFrameCount;
		_sliderArea.sizeDelta = new Vector2(totalWidth, _cursor.GetComponent<RectTransform>().sizeDelta.y);
	}

    private void ConfigureCursor() {
        _cursor.value = 0;
        _cursor.maxValue = _videoTimelineViewModel.TotalFrameCount;
    }

}