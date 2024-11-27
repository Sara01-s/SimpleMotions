using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System.Collections.Generic;

public sealed class TimelineView : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Scrollbar _horizontalScrollbar;
	[SerializeField] private EditorPainter _editorPainter;
	[SerializeField] private GameObject _editKeyframePanel;

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
    public readonly Dictionary<int, Dictionary<ComponentDTO, Dictionary<int, GameObject>>> DisplayedEntityKeyframes = new();  

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

	private float GetKeyframeXPosition() {
		return Remap (
			_videoTimelineViewModel.CurrentFrame.Value, 
			iMin: 0.0f, 
			_videoTimelineViewModel.TotalFrameCount, 
			_content.rect.xMin, 
			_content.rect.xMax - (_frameWidth * 2)
		);
	}

	// TODO - Mover
	private static float Remap(float value, float iMin, float iMax, float oMin, float oMax) {
		float t = Mathf.InverseLerp(iMin, iMax, value);
		return Mathf.Lerp(oMin, oMax, t);
	}

	private void AddKeyframe(KeyframeDTO entityKeyframe, GameObject keyframeDisplay) {
        if (!DisplayedEntityKeyframes.TryGetValue(entityKeyframe.Id, out var componentsByType)) {
			DisplayedEntityKeyframes[entityKeyframe.Id] = componentsByType = new();
		}

        if (!componentsByType.TryGetValue(entityKeyframe.ComponentDTO, out var frameByType)) {
			componentsByType[entityKeyframe.ComponentDTO] = frameByType = new();
        }

        frameByType[entityKeyframe.Frame] = keyframeDisplay;

		var keyframeSelector = keyframeDisplay.GetComponent<KeyframeSelector>();
		keyframeSelector.Configure(entityKeyframe.Id, entityKeyframe.ComponentDTO, entityKeyframe.Frame, entityKeyframe.Ease, _editKeyframePanel);

		// TODO - ¿Después hay que borrarlo?

        Debug.Log($"La entidad con la ID {entityKeyframe.Id}, del tipo {entityKeyframe.ComponentDTO} ha sido añadida en el frame {entityKeyframe.Frame}");
    }

    private void RemoveKeyframe(KeyframeDTO entityKeyframe) {
        if (!DisplayedEntityKeyframes.TryGetValue(entityKeyframe.Id, out var componentTypeToKeyframes)) {
			return;
		}

		if (!componentTypeToKeyframes.TryGetValue(entityKeyframe.ComponentDTO, out var frameToKeyframe)) {
			return;
		}

		if (!frameToKeyframe.TryGetValue(entityKeyframe.Frame, out var keyframeToRemove)) {
			return;
		}

		frameToKeyframe.Remove(entityKeyframe.Frame);
		Destroy(keyframeToRemove);

		if (frameToKeyframe.Count == 0) {
			componentTypeToKeyframes.Remove(entityKeyframe.ComponentDTO);
		}

		if (componentTypeToKeyframes.Count == 0) {
			DisplayedEntityKeyframes.Remove(entityKeyframe.Id);
		}

		Debug.Log($"La entidad con la ID {entityKeyframe.Id}, del tipo {entityKeyframe.ComponentDTO} ha sido eliminada del frame {entityKeyframe.Frame}");
    }

	private void UpdateDisplayedKeyframes(int selectedEntityId) {
		foreach (var entityEntry in DisplayedEntityKeyframes) {
			var entityId = entityEntry.Key;
			var componentTypeToKeyframes = entityEntry.Value;

			bool entityIsSelected = entityId == selectedEntityId;

			foreach (var componentEntry in componentTypeToKeyframes) {
				var componentType = componentEntry.Key;
				var frameToKeyframe = componentEntry.Value;

				foreach (var frameEntry in frameToKeyframe) {
					var keyframe = frameEntry.Value;

					keyframe.SetActive(entityIsSelected);
				}
			}
		}
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