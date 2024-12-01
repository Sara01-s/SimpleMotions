using static SimpleMotions.SmMath;
using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using System.Collections.Generic;

public sealed class TimelineView : MonoBehaviour {

	[Header("References")]
	[SerializeField] private Scrollbar _horizontalScrollbar;
	[SerializeField] private EditorPainter _editorPainter;
	[SerializeField] private GameObject _editKeyframePanel;
	[SerializeField] private GameObject _keyframesHolder;

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
    [SerializeField] private float _textKeyframeYPosition = -89.625f;

	private IVideoTimelineViewModel _videoTimelineViewModel;

	//              		EntityId    ->       Component     ->      Frame -> Keyframe
    public readonly Dictionary<int, Dictionary<ComponentDTO, Dictionary<int, GameObject>>> DisplayedEntityKeyframes = new();  

	private const float FRAME_WIDTH = 42.0f;
	private bool _alreadyPainted;

	public void Configure(IVideoTimelineViewModel videoTimelineViewModel, IEntitySelectorViewModel entitySelectorViewModel,
						  IEditKeyframeViewModel editKeyframeViewModel) {
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
            SetKeyframesVisibilityByEntity(entityDTO.Id);
        });

		editKeyframeViewModel.NewKeyframeFrame.Subscribe(EditEntityKeyframe);
	}

	private GameObject CreateKeyframe(float frame, float keyframeYPosition) {
		float keyframeXPosition = GetKeyframeXPosition(frame);

		var keyframe = Instantiate(_keyframePrefab, parent: _content);
		var keyframeRect = keyframe.GetComponent<RectTransform>();

		keyframeRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0.0f, keyframeRect.rect.height);

		if (frame == 0) {
			keyframeRect.anchoredPosition = new Vector2(FRAME_WIDTH, keyframeYPosition);
		}
		else { 
			keyframeRect.anchoredPosition = new Vector2(keyframeXPosition + FRAME_WIDTH, keyframeYPosition);
		}

		keyframe.GetComponent<RectTransform>().SetParent(_keyframesHolder.transform);
		keyframe.GetComponent<Image>().color = _editorPainter.CurrentAccentColor;

		return keyframe;
	}

	private float GetKeyframeXPosition(float targetFrame) {
		return remap (
			targetFrame, 
			iMin: 0.0f, 
			_videoTimelineViewModel.TotalFrameCount, 
			_content.rect.xMin, 
			_content.rect.xMax - (FRAME_WIDTH * 2)
		);
	}

	private void AddKeyframe(KeyframeDTO entityKeyframe, GameObject keyframeDisplay) {
        if (!DisplayedEntityKeyframes.TryGetValue(entityKeyframe.EntityId, out var componentsByType)) {
			DisplayedEntityKeyframes[entityKeyframe.EntityId] = componentsByType = new();
		}

        if (!componentsByType.TryGetValue(entityKeyframe.ComponentDTO, out var frameByType)) {
			componentsByType[entityKeyframe.ComponentDTO] = frameByType = new();
        }

        frameByType[entityKeyframe.Frame] = keyframeDisplay;

		var keyframeSelector = keyframeDisplay.GetComponent<KeyframeSelector>();
		keyframeSelector.Configure(entityKeyframe.EntityId, entityKeyframe.ComponentDTO, entityKeyframe.Frame, entityKeyframe.Ease, _editKeyframePanel);

		// TODO - ¿Después hay que borrarlo?

        //Debug.Log($"La entidad con la ID {entityKeyframe.EntityId}, del tipo {entityKeyframe.ComponentDTO} ha sido añadida en el frame {entityKeyframe.Frame}");
    }

    private void RemoveKeyframe(KeyframeDTO entityKeyframe) {
        if (!DisplayedEntityKeyframes.TryGetValue(entityKeyframe.EntityId, out var keyframeComponents)) {
			return;
		}

		if (!keyframeComponents.TryGetValue(entityKeyframe.ComponentDTO, out var componentFrames)) {
			return;
		}

		if (!componentFrames.TryGetValue(entityKeyframe.Frame, out var frameKeyframe)) {
			return;
		}

		componentFrames.Remove(entityKeyframe.Frame);
		Destroy(frameKeyframe);

		if (componentFrames.Count == 0) {
			keyframeComponents.Remove(entityKeyframe.ComponentDTO);
		}

		if (keyframeComponents.Count == 0) {
			DisplayedEntityKeyframes.Remove(entityKeyframe.EntityId);
		}

		//Debug.Log($"La entidad con la ID {entityKeyframe.EntityId}, del tipo {entityKeyframe.ComponentDTO} ha sido eliminada del frame {entityKeyframe.Frame}");
    }

	private void SetKeyframesVisibility() {
		 foreach (var entityId in DisplayedEntityKeyframes.Keys) {
			var entityComponents = DisplayedEntityKeyframes[entityId];

			foreach (var component in entityComponents.Keys) {
				var entityKeyframes = entityComponents[component];

				foreach (var keyframes in entityKeyframes.Keys) {
					var keyframe = entityKeyframes[keyframes];

					keyframe.SetActive(keyframes <= _videoTimelineViewModel.TotalFrameCount);
				}
			}
		}
	}

	private void SetKeyframesVisibilityByEntity(int selectedEntityId) {
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

	private void EditEntityKeyframe((KeyframeDTO originalKeyframe, int targetFrame) values) {
		var entityId = values.originalKeyframe.EntityId;
		var componentDTO = values.originalKeyframe.ComponentDTO;
		var frame = values.originalKeyframe.Frame;
		var ease = values.originalKeyframe.Ease;

		var previousKeyframe = new KeyframeDTO(componentDTO, entityId, frame, ease);
		RemoveKeyframe(previousKeyframe);

		var newKeyframe = new KeyframeDTO(componentDTO, entityId, values.targetFrame, ease);

		if (componentDTO == ComponentDTO.Transform) {
			AddKeyframe(newKeyframe, CreateKeyframe(values.targetFrame, _transformKeyframeYPosition));
		}
		else if (componentDTO == ComponentDTO.Shape) {
			AddKeyframe(newKeyframe, CreateKeyframe(values.targetFrame, _shapeKeyframeYPosition));
		}
		else if (componentDTO == ComponentDTO.Text) {
			AddKeyframe(newKeyframe, CreateKeyframe(values.targetFrame, _textKeyframeYPosition));
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

		SetKeyframesVisibility();

		_content.sizeDelta = new Vector2(totalWidth, gridLayout.cellSize.y);
		_framesHolder.sizeDelta = new Vector2(totalWidth, _framesHolder.sizeDelta.y);

		_alreadyPainted = true;
		
	}

	private void DeleteTimeline() {
		_alreadyPainted = false;
	}

}