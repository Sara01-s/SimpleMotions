using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoBody : MonoBehaviour, IDragHandler, IBeginDragHandler {
    
	[SerializeField] private Canvas _editorCanvas;

	private RectTransform _rectTransform;
    private IComponentViewModel _entityViewModel;
    private IEntitySelector _entitySelector;

	private Vector2 _dragOffset;

    public void Configure(IComponentViewModel entityViewModel, IEntitySelector entitySelector) {
		_rectTransform = GetComponent<RectTransform>();
        _entityViewModel = entityViewModel;
        _entitySelector = entitySelector;
    }

	private Vector2 GetPointerWorldPos(Vector2 pointerScreenPos) {
		var camera = _editorCanvas.worldCamera;
		return camera.ScreenToWorldPoint(pointerScreenPos);
	}

	public void OnBeginDrag(PointerEventData eventData) {
		int selectedEntityId = _entitySelector.SelectedEntity.Id;

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
		int selectedEntityId = _entitySelector.SelectedEntity.Id;

		_entityViewModel.SetEntityPosition(selectedEntityId, (newEntityWorldPosition.x, newEntityWorldPosition.y));

		SyncGizmoWithEntity();
	}

	private void SyncGizmoWithEntity() {
		int selectedEntityId = _entitySelector.SelectedEntity.Id;

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
