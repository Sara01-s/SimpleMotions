using System.Collections.Generic;
using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System;

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

    //              EntityId    ->       Component     ->      Frame -> Keyframe
    private Dictionary<int, Dictionary<ComponentType, Dictionary<int, GameObject>>> _entityComponentKeyframes = new();  

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
        _videoTimelineViewModel = videoTimelineViewModel;

        _cursor.onValueChanged.AddListener(value => {
            _videoTimelineViewModel.OnFrameChanged.Execute((int)value);
        });

        _videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorNewFrame);

        RefreshUI();                                                        
		
        videoTimelineViewModel.OnDrawTransformKeyframe.Subscribe(transformEntityKeyframe => {
            var transformKeyframe = Instantiate(_keyframePrefab, parent: _cursorHandle);
            transformKeyframe.GetComponent<Image>().color = Color.blue;

            var rect = transformKeyframe.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, _transformKeyframeYPosition);
            rect.transform.SetParent(_keyframesHolder.transform);

            AddKeyframe(transformEntityKeyframe, transformKeyframe);
        });

        videoTimelineViewModel.OnTransfromKeyframeDeleted.Subscribe(transformEntityKeyframe => {
            RemoveKeyframe(transformEntityKeyframe);
        });

        videoTimelineViewModel.OnDrawShapeKeyframe.Subscribe(shapeEntityKeyframe => {
            var shapeKeyframe = Instantiate(_keyframePrefab, parent: _cursorHandle);
            shapeKeyframe.GetComponent<Image>().color = Color.red;

            var rect = shapeKeyframe.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, _shapeKeyframeYPosition);
            rect.transform.SetParent(_keyframesHolder.transform);

            AddKeyframe(shapeEntityKeyframe, shapeKeyframe);
        });

        videoTimelineViewModel.OnShapeKeyframeDeleted.Subscribe(shapeEntityKeyframe => {
            RemoveKeyframe(shapeEntityKeyframe);
        });

        entitySelectorViewModel.OnEntitySelected.Subscribe(entitySelected => {
            ShowEntityKeyframes(entitySelected.Id);
        });
    }

    private void AddKeyframe(EntityKeyframe entityKeyframe, GameObject keyframeToAdd) {
        if (!_entityComponentKeyframes.TryGetValue(entityKeyframe.Id, out var componentsByType)) {
                componentsByType = new Dictionary<ComponentType, Dictionary<int, GameObject>>();
                _entityComponentKeyframes[entityKeyframe.Id] = componentsByType;
            }

        if (!componentsByType.TryGetValue(entityKeyframe.ComponentType, out var frameByType)) {
                frameByType = new Dictionary<int, GameObject>();
                componentsByType[entityKeyframe.ComponentType] = frameByType;
        }

        frameByType[entityKeyframe.Frame] = keyframeToAdd;

        Debug.Log($"La entidad con la ID {entityKeyframe.Id}, del tipo {entityKeyframe.ComponentType} ha sido añadida en el frame {entityKeyframe.Frame}");
    }

    private void RemoveKeyframe(EntityKeyframe entityKeyframe) {
        if (_entityComponentKeyframes.TryGetValue(entityKeyframe.Id, out var componentsByType)) {
            if (componentsByType.TryGetValue(entityKeyframe.ComponentType, out var frameByType)) {
                if (frameByType.TryGetValue(entityKeyframe.Frame, out var keyframeToRemove)) {
                    frameByType.Remove(entityKeyframe.Frame);
                    Destroy(keyframeToRemove);

                    if (frameByType.Count == 0) {
                        componentsByType.Remove(entityKeyframe.ComponentType);
                    }

                    if (componentsByType.Count == 0) {
                        _entityComponentKeyframes.Remove(entityKeyframe.Id);
                    }

                    Debug.Log($"La entidad con la ID {entityKeyframe.Id}, del tipo {entityKeyframe.ComponentType} ha sido eliminada del frame {entityKeyframe.Frame}");
                }
            }
        }
    }

    private void ShowEntityKeyframes(int entityId) {
        if (_entityComponentKeyframes.TryGetValue(entityId, out var componentsByType)) {
            foreach (var componentType in componentsByType.Keys) {
                if (componentsByType.TryGetValue(componentType, out var frameByType)) {
                    foreach (var frame in frameByType.Keys) {
                        if (frameByType.TryGetValue(frame, out var keyframe)) {
                            keyframe.SetActive(true);

                            Debug.Log($"Activado el keyframe de nombre: {keyframe.name}");
                        }
                    }
                }
            }
        }
    }

    private void HideEntityKeyframes(int entityId) {
        
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