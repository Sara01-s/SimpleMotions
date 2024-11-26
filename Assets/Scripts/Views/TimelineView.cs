using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System.Collections.Generic;

public sealed class TimelineView : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Scrollbar _horizontalScrollbar;
	[SerializeField] private EditorPainter _editorPainter;

	[Header("Draw")]
	[SerializeField] private RectTransform _content;
	[SerializeField] private RectTransform _framesHolder;
	[SerializeField] private GameObject _keyframePrefab;

	[Header("Relationed Views")]
	[SerializeField] private TimelineHeaderView _headerView;
	[SerializeField] private TimelineCursorView _cursorView;

	[Header("Keyframe Y Positions")]
	[SerializeField] private float _transformKeyframeYPosition = 41.125f;
    [SerializeField] private float _shapeKeyframeYPosition = -24.25f;
    //[SerializeField] private float _textKeyframeYPosition = -89.625f;

	private IVideoTimelineViewModel _videoTimelineViewModel;

	private List<GameObject> _timeline = new();

	//              		EntityId    ->       Component     ->      Frame -> Keyframe
    private readonly Dictionary<int, Dictionary<ComponentType, Dictionary<int, GameObject>>> _displayedEntityKeyframes = new();  

	private float _frameWidth = 42.0f;
	private bool _alreadyPainted;

	public void Configure(IVideoTimelineViewModel videoTimelineViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
		_videoTimelineViewModel = videoTimelineViewModel;

		_horizontalScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

		videoTimelineViewModel.OnTransfromKeyframeDeleted.Subscribe(RemoveKeyframe);
        videoTimelineViewModel.OnShapeKeyframeDeleted.Subscribe(RemoveKeyframe);

		videoTimelineViewModel.OnDrawTransformKeyframe.Subscribe(transformKeyframeDTO => {
            AddKeyframe(transformKeyframeDTO, CreateKeyframe(transformKeyframeDTO.Frame, _transformKeyframeYPosition));
        });

		videoTimelineViewModel.OnDrawShapeKeyframe.Subscribe(shapeKeyframeDTO => {
            AddKeyframe(shapeKeyframeDTO, CreateKeyframe(shapeKeyframeDTO.Frame, _shapeKeyframeYPosition));
        });

		entitySelectorViewModel.OnEntitySelected.Subscribe(entityDTO => {
            UpdateDisplayedKeyframes(entityDTO.Id);
        });
	}

	private GameObject CreateKeyframe(float frame, float keyframeYPosition) {
		float keyframeXPosition = GetKeyframeXPosition();

		var keyframe = Instantiate(_keyframePrefab, _content);
		var keyframeRect = keyframe.GetComponent<RectTransform>();

		keyframeRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0.0f, keyframeRect.rect.height);

		if (frame == 0) {
			keyframeRect.anchoredPosition = new Vector2(_frameWidth, keyframeYPosition);
		}
		else { 
			keyframeRect.anchoredPosition = new Vector2(keyframeXPosition + _frameWidth, keyframeYPosition);
		}

		keyframe.GetComponent<Image>().color = _editorPainter.CurrentAccentColor;

		return keyframe;
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

	private float GetKeyframeXPosition() {
		return Remap (
			_videoTimelineViewModel.CurrentFrame.Value, 
			iMin: 0.0f, 
			_videoTimelineViewModel.TotalFrameCount, 
			_content.rect.xMin, 
			_content.rect.xMax - (_frameWidth * 2)
		);
	}

	//private float RemapConcrete(float currentFrame, float firstFrame, float lastFrame, float contentRectMinX, float contentRectMaxX) {
	//	float t = Mathf.InverseLerp(firstFrame, lastFrame, currentFrame);
	//	return Mathf.Lerp(contentRectMinX, contentRectMaxX, t);
//
	//	// inverselerp(0, 300) -> cf = 30 = 0.3
	//	// lerp(0, 12_600) -> t = 0.3 = 30%
	//}

	// TODO - Mover
	private static float Remap(float value, float iMin, float iMax, float oMin, float oMax) {
		float t = Mathf.InverseLerp(iMin, iMax, value);
		return Mathf.Lerp(oMin, oMax, t);
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

}