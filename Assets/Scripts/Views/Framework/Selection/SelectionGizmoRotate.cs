using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoRotate : MonoBehaviour, IDragHandler, IBeginDragHandler {
    
    [SerializeField] private Canvas _editorCanvas;

    private RectTransform _parentRect;
    private IComponentViewModel _entityViewModel;
    private IEntitySelectorViewModel _entitySelectorViewModel;
    private float _startOffsetAngle;

    public void Configure(IComponentViewModel entityViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
        _parentRect = transform.parent as RectTransform;
        _entityViewModel = entityViewModel;
        _entitySelectorViewModel = entitySelectorViewModel;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        var pointerWorldPos = GetPointerWorldPos(eventData.position);
        Vector2 gizmoWorldPos = _parentRect.position;

        var direction = pointerWorldPos - gizmoWorldPos;
        float initialPointerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

        if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
            _startOffsetAngle = transform.rollAngleDegrees - initialPointerAngle;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        RollEntity(GetPointerWorldPos(eventData.position));
    }

    private Vector2 GetPointerWorldPos(Vector2 pointerScreenPos) {
        var camera = _editorCanvas.worldCamera;

        if (camera == null) {
            Debug.LogError("Canvas camera is not assigned.");
            return Vector2.zero;
        }

        return camera.ScreenToWorldPoint(new Vector3(pointerScreenPos.x, pointerScreenPos.y, camera.nearClipPlane));
    }

    private void RollEntity(Vector2 pointerWorldPos) {
        Vector2 gizmoWorldPos = _parentRect.position;
        var direction = pointerWorldPos - gizmoWorldPos;

        float currentPointerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float finalAngle = currentPointerAngle + _startOffsetAngle;
        int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;

        _entityViewModel.SetEntityRoll(selectedEntityId, finalAngle);
        SyncGizmoWithEntity();
    }

    private void SyncGizmoWithEntity() {
        int selectedEntityId = _entitySelectorViewModel.SelectedEntityId;
        if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
            _parentRect.localRotation = Quaternion.AngleAxis(transform.rollAngleDegrees, Vector3.forward);
        }
    }
}
