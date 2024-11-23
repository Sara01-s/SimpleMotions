using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionGizmoBody : SelectionGizmo {
    
	private Vector2 _startDragOffset;

	public override void OnBeginDrag(PointerEventData eventData) {
		var (pos, _, _) = _GetSelectedEntityTransformData();
		var entityWorldPos = new Vector2(pos.x, pos.y);
		var pointerWorldPos = _GetPointerWorldPos(eventData.position);

		_startDragOffset = entityWorldPos - pointerWorldPos;
	}

	public override void OnDrag(PointerEventData eventData) {
		DragEntity(eventData.position);
	}

	private void DragEntity(Vector2 pointerScreenPos) {
		var pointerWorldPosition = _GetPointerWorldPos(pointerScreenPos);
		var newEntityWorldPosition = pointerWorldPosition + _startDragOffset;

		_SetSelectedEntityPosition(newEntityWorldPosition);
	}

	protected override void SyncGizmoWithEntity() {
		var (pos, _, _) = _GetSelectedEntityTransformData();
		var entityWorldPos = new Vector2(pos.x, pos.y);
		var entityScreenPos = _WorldToScreenPoint(entityWorldPos);

		_SelectionGizmoRect.localPosition = _ScreenPointToLocalPointInRectangle(entityScreenPos);
	}

}
