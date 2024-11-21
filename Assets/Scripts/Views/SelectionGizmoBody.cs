using static UnityEngine.Mathf;
using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoBody : MonoBehaviour, IDragHandler, IBeginDragHandler {
    
	[SerializeField] private Canvas _editorCanvas;
	[SerializeField] private RectTransform _videoCanvas;

	private RectTransform _rectTransform;
    private IComponentViewModel _entityViewModel;
    private IEntitySelectorViewModel _entitySelectorViewModel;

	private Vector2 _dragOffset;

    public void Configure(IComponentViewModel entityViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
		_rectTransform = GetComponent<RectTransform>();
        _entityViewModel = entityViewModel;
        _entitySelectorViewModel = entitySelectorViewModel;
    }

	private Vector2 GetPointerWorldPos(Vector2 pointerScreenPos) {
		var camera = _editorCanvas.worldCamera;
		return camera.ScreenToWorldPoint(pointerScreenPos);
	}

	public void OnBeginDrag(PointerEventData eventData) {
		int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
			var entityWorldPos = new Vector2(transform.pos.x, transform.pos.y);
			var pointerWorldPos = GetPointerWorldPos(eventData.position);

			_dragOffset = entityWorldPos - pointerWorldPos;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		DragEntity(eventData.position);
	}

	private void DragEntity(Vector2 pointerScreenPos) {
		var pointerWorldPosition = GetPointerWorldPos(pointerScreenPos);
		var newEntityWorldPosition = pointerWorldPosition + _dragOffset;
		int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

		_entityViewModel.SetEntityPosition(selectedEntityId, (newEntityWorldPosition.x, newEntityWorldPosition.y));

		SyncGizmoWithEntity();
	}

	private void SyncGizmoWithEntity() {
		int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
			var entityWorldPos = new Vector2(transform.pos.x, transform.pos.y);
			var entityScreenPos = RectTransformUtility.WorldToScreenPoint(_editorCanvas.worldCamera, entityWorldPos);

			RectTransformUtility.ScreenPointToLocalPointInRectangle (
				(RectTransform)_editorCanvas.transform,
				entityScreenPos,
				_editorCanvas.worldCamera,
				out var entityLocalPositionInRect
			);

			_rectTransform.localPosition = entityLocalPositionInRect;
		}
	}

}
