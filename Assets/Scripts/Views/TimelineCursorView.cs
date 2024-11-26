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

    //              		EntityId    ->       Component     ->      Frame -> Keyframe
    private readonly Dictionary<int, Dictionary<ComponentType, Dictionary<int, GameObject>>> _displayedEntityKeyframes = new();  

    public void Configure(IVideoTimelineViewModel videoTimelineViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
        _cursor.onValueChanged.AddListener(value => {
            _videoTimelineViewModel.OnFrameChanged.Execute((int)value);
        });

		entitySelectorViewModel.OnEntitySelected.Subscribe(entityDTO => {
            UpdateDisplayedKeyframes(entityDTO.Id);
        });

        videoTimelineViewModel.OnDrawTransformKeyframe.Subscribe(transformKeyframeDTO => {
            var transformKeyframe = Instantiate(_keyframePrefab, parent: _cursorHandle);
            var keyframeRect = transformKeyframe.GetComponent<RectTransform>();

            float keyframeXPosition;

            if (transformKeyframeDTO.Frame == 0) {
                transformKeyframe.transform.SetParent(_sliderArea);
                keyframeRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0.0f, transformKeyframe.GetComponent<RectTransform>().rect.height);
                keyframeXPosition = 21.0f;
            }
            else {
                keyframeXPosition = keyframeRect.anchoredPosition.x;
            }

            keyframeRect.anchoredPosition = new Vector2(keyframeXPosition, _transformKeyframeYPosition);
            keyframeRect.transform.SetParent(_keyframesHolder.transform);

            AddKeyframe(transformKeyframeDTO, transformKeyframe);
        });

        videoTimelineViewModel.OnDrawShapeKeyframe.Subscribe(shapeKeyframeDTO => {
            var shapeKeyframe = Instantiate(_keyframePrefab, parent: _cursorHandle);
            var keyframeRect = shapeKeyframe.GetComponent<RectTransform>();

			float keyframeXPosition;

            if (shapeKeyframeDTO.Frame == 0) {
                shapeKeyframe.transform.SetParent(_sliderArea);
                keyframeRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0.0f, shapeKeyframe.GetComponent<RectTransform>().rect.height);
                keyframeXPosition = 21.0f;
            }
            else {
                keyframeXPosition = keyframeRect.anchoredPosition.x;
            }

            keyframeRect.anchoredPosition = new Vector2(keyframeXPosition, _shapeKeyframeYPosition);
            keyframeRect.transform.SetParent(_keyframesHolder.transform);

            AddKeyframe(shapeKeyframeDTO, shapeKeyframe);
        });

		videoTimelineViewModel.OnTransfromKeyframeDeleted.Subscribe(RemoveKeyframe);
        videoTimelineViewModel.OnShapeKeyframeDeleted.Subscribe(RemoveKeyframe);
        videoTimelineViewModel.CurrentFrame.Subscribe(SetCursorNewFrame);
        _videoTimelineViewModel = videoTimelineViewModel;
        RefreshUI();
    }

    private void AddKeyframe(KeyframeDTO entityKeyframe, GameObject keyframeDisplay) {
        if (!_displayedEntityKeyframes.TryGetValue(entityKeyframe.Id, out var componentsByType)) {
			_displayedEntityKeyframes[entityKeyframe.Id] = componentsByType = new();
		}

        if (!componentsByType.TryGetValue(entityKeyframe.ComponentType, out var frameByType)) {
			componentsByType[entityKeyframe.ComponentType] = frameByType = new();
        }

        frameByType[entityKeyframe.Frame] = keyframeDisplay;

        Debug.Log($"La entidad con la ID {entityKeyframe.Id}, del tipo {entityKeyframe.ComponentType} ha sido añadida en el frame {entityKeyframe.Frame}");
    }

    private void RemoveKeyframe(KeyframeDTO entityKeyframe) {
        if (!_displayedEntityKeyframes.TryGetValue(entityKeyframe.Id, out var componentTypeToKeyframes)) {
			return;
		}

		if (!componentTypeToKeyframes.TryGetValue(entityKeyframe.ComponentType, out var frameToKeyframe)) {
			return;
		}

		if (!frameToKeyframe.TryGetValue(entityKeyframe.Frame, out var keyframeToRemove)) {
			return;
		}

		frameToKeyframe.Remove(entityKeyframe.Frame);
		Destroy(keyframeToRemove);

		if (frameToKeyframe.Count == 0) {
			componentTypeToKeyframes.Remove(entityKeyframe.ComponentType);
		}

		if (componentTypeToKeyframes.Count == 0) {
			_displayedEntityKeyframes.Remove(entityKeyframe.Id);
		}

		Debug.Log($"La entidad con la ID {entityKeyframe.Id}, del tipo {entityKeyframe.ComponentType} ha sido eliminada del frame {entityKeyframe.Frame}");
    }

	private void UpdateDisplayedKeyframes(int selectedEntityId) {
		foreach (var entityEntry in _displayedEntityKeyframes) {
			var entityId = entityEntry.Key;
			var componentTypeToKeyframes = entityEntry.Value;

			bool entityIsSelected = entityId == selectedEntityId;

			foreach (var componentEntry in componentTypeToKeyframes) {
				var componentType = componentEntry.Key;
				var frameToKeyframe = componentEntry.Value;

				foreach (var frameEntry in frameToKeyframe) {
					var keyframe = frameEntry.Value;

					keyframe.SetActive(entityIsSelected);
					//Debug.Log($"Keyframes de entidad: {entityId} {(entityIsSelected ? "Encendido" : "Apagado")}");
				}
			}
		}
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

	private void DrawKeyframe(ComponentType componentType) {
		var contentRect = new RectTransform();
		int currentFrame = 30;

		float keyframePosX = Remap(currentFrame, 0, 300, contentRect.rect.xMin, contentRect.rect.xMax);
	}

	private float RemapConcrete(float currentFrame, float firstFrame, float lastFrame, float contentRectMinX, float contentRectMaxX) {
		float t = Mathf.InverseLerp(firstFrame, lastFrame, currentFrame);
		return Mathf.Lerp(contentRectMinX, contentRectMaxX, t);

		// inverselerp(0, 300) -> cf = 30 = 0.3
		// lerp(0, 12_600) -> t = 0.3 = 30%
	}

	private static float Remap(float value, float iMin, float iMax, float oMin, float oMax) {
		float t = Mathf.InverseLerp(iMin, iMax, value);
		return Mathf.Lerp(oMin, oMax, t);
	}

}