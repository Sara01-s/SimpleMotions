using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoRotate : MonoBehaviour, IDragHandler {
    
	private RectTransform _parentRect;
    private IComponentViewModel _entityViewModel;
    private IEntitySelectorViewModel _entitySelectorViewModel;

    public void Configure(IComponentViewModel entityViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
		_parentRect = transform.parent as RectTransform;
        _entityViewModel = entityViewModel;
        _entitySelectorViewModel = entitySelectorViewModel;
    }

	public void OnDrag(PointerEventData eventData) {
		RollEntity(eventData.position);
	}

	private void RollEntity(Vector2 pointerScreenPos) {
        var angleDirRad = Mathf.Atan2(pointerScreenPos.y, pointerScreenPos.x);
        var angleDirDeg = 180.0f * angleDirRad / Mathf.PI;
        float angleDegrees = 360.0f * Mathf.Round(angleDirDeg) % 360.0f;
		int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

        _entityViewModel.SetEntityRoll(selectedEntityId, angleDegrees);
		SyncGizmoWithEntity();
	}

	private void SyncGizmoWithEntity() {
		int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
			_parentRect.localRotation = Quaternion.AngleAxis(transform.rollAngleDegrees, Vector3.forward);
		}
	}

}
