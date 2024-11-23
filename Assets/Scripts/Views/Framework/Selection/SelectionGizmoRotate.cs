using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionGizmoRotate : SelectionGizmo {

	private float _startOffsetAngle;

    public override void OnBeginDrag(PointerEventData eventData) {
        var pointerWorldPos = _GetPointerWorldPos(eventData.position);
        Vector2 gizmoWorldPos = _SelectionGizmoRect.position;

        var direction = pointerWorldPos - gizmoWorldPos;
        float initialPointerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		var (_, _, rollAngleDegrees) = _GetSelectedEntityTransformData();
		_startOffsetAngle = rollAngleDegrees - initialPointerAngle;
    }

    public override void OnDrag(PointerEventData eventData) {
        RollEntity(_GetPointerWorldPos(eventData.position));
    }

    private void RollEntity(Vector2 pointerWorldPos) {
        Vector2 gizmoWorldPos = _SelectionGizmoRect.position;
        var direction = pointerWorldPos - gizmoWorldPos;

        float currentPointerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float finalAngle = currentPointerAngle + _startOffsetAngle;

		_SetSelectedEntityRoll(finalAngle);
    }

    protected override void SyncGizmoWithEntity() {
		var (_, _, rollAngleDegrees) = _GetSelectedEntityTransformData();
		_SelectionGizmoRect.localRotation = Quaternion.AngleAxis(rollAngleDegrees, Vector3.forward);
    }
}
