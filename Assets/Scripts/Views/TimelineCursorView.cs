using System.Collections.Generic;
using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;

public class TimelineCursorView : MonoBehaviour {

    [SerializeField] private Transform _cursorHandle;
    [SerializeField] private GameObject _keyframePrefab;
    [SerializeField] private RectTransform _sliderArea;
    [SerializeField] private GridLayoutGroup _framesHolder;
    [SerializeField] private Slider _cursor;
    [SerializeField] private GameObject _keyframesHolder;

    [SerializeField] private float _transformKeyframeYPosition = 41.125f;
    [SerializeField] private float _shapeKeyframeYPosition = -24.25f;
    //[SerializeField] private float _textKeyframeYPosition = -89.625f;

    private IVideoTimelineViewModel _videoTimelineViewModel;

    private Dictionary<int, GameObject> _transformKeyframes = new();
    private Dictionary<int, GameObject> _shapeKeyframes = new();

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _cursor.onValueChanged.AddListener(value => {
            _videoTimelineViewModel.OnFrameChanged.Execute((int)value);
        });

        _videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorNewFrame);

        RefreshUI();                                                        
		
        videoTimelineViewModel.OnDrawTransformKeyframe.Subscribe(() => {
            var transformKeyframe = Instantiate(_keyframePrefab, parent: _cursorHandle);
            transformKeyframe.GetComponent<Image>().color = Color.blue;

            var rect = transformKeyframe.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, _transformKeyframeYPosition);
            rect.transform.SetParent(_keyframesHolder.transform);

            _transformKeyframes.Add((int)_cursor.value, transformKeyframe);
        });

        videoTimelineViewModel.OnTransfromKeyframeDeleted.Subscribe(() => {
            if (_transformKeyframes.TryGetValue((int)_cursor.value, out var transformKeyframe)) {
                _transformKeyframes.Remove((int)_cursor.value);
                Destroy(transformKeyframe);
            }
            
            Debug.Log($"TransformKeyframe {(int)_cursor.value} eliminado del diccionario.");
        });

        videoTimelineViewModel.OnDrawShapeKeyframe.Subscribe(() => {
            var shapeKeyframe = Instantiate(_keyframePrefab, parent: _cursorHandle);
            shapeKeyframe.GetComponent<Image>().color = Color.red;

            var rect = shapeKeyframe.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, _shapeKeyframeYPosition);
            rect.transform.SetParent(_keyframesHolder.transform);

            _shapeKeyframes.Add((int)_cursor.value, shapeKeyframe);
        });

        videoTimelineViewModel.OnShapeKeyframeDeleted.Subscribe(() => {
            if (_shapeKeyframes.TryGetValue((int)_cursor.value, out var shapeKeyframe)) {
                _shapeKeyframes.Remove((int)_cursor.value);
                Destroy(shapeKeyframe);
            }

            Debug.Log($"ShapeKeyframe {(int)_cursor.value} eliminado del diccionario.");
        });
    }

    public void SetCursorNewFrame(int currentFrame) {
        _cursor.value = currentFrame;
    }

    public void UpdateSliderArea(float contentXPos) {
        _sliderArea.anchoredPosition = new Vector2(contentXPos - _framesHolder.cellSize.x, _sliderArea.anchoredPosition.y);
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