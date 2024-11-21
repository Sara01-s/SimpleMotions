using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoRotate : MonoBehaviour, IDragHandler {
    
	[SerializeField] private Canvas _editorCanvas;
	
	private RectTransform _parentRect;
    private IComponentViewModel _entityViewModel;
    private IEntitySelectorViewModel _entitySelectorViewModel;

    public void Configure(IComponentViewModel entityViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
		_parentRect = transform.parent as RectTransform;
        _entityViewModel = entityViewModel;
        _entitySelectorViewModel = entitySelectorViewModel;
    }

	public void OnDrag(PointerEventData eventData) {
		RollEntity(GetPointerWorldPos(eventData.position));
	}

	private Vector2 GetPointerWorldPos(Vector2 pointerScreenPos) {
		var camera = _editorCanvas.worldCamera;
		return camera.ScreenToWorldPoint(pointerScreenPos);
	}

	private void RollEntity(Vector2 pointerWorldPos) {
		// TODO - Roll entity sara...
        float angleDirRad = Mathf.Atan2(pointerWorldPos.y, pointerWorldPos.x);
		int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

        _entityViewModel.SetEntityRoll(selectedEntityId, angleDirRad * Mathf.Rad2Deg);
		SyncGizmoWithEntity();
	}

	private void SyncGizmoWithEntity() {
		int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
			_parentRect.localRotation = Quaternion.AngleAxis(transform.rollAngleDegrees, Vector3.forward);
		}
	}

}
