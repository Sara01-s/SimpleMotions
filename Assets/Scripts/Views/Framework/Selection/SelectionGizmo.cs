using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public abstract class SelectionGizmo : MonoBehaviour, IDragHandler, IBeginDragHandler {
    
	protected int SelectedEntityId => _entitySelectorViewModel.SelectedEntityId;

    [SerializeField] private Canvas _editorCanvas;

    private IComponentViewModel _entityViewModel;
    private IEntitySelectorViewModel _entitySelectorViewModel;

    public virtual void Configure(IComponentViewModel entityViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
        _entityViewModel = entityViewModel;
        _entitySelectorViewModel = entitySelectorViewModel;
    }

    public abstract void OnBeginDrag(PointerEventData eventData);
	public abstract void OnDrag(PointerEventData eventData);
	public abstract void SyncGizmoWithEntity();

    protected Vector2 GetPointerWorldPos(Vector2 pointerScreenPos) {
        var camera = _editorCanvas.worldCamera;

        if (camera == null) {
            Debug.LogError("Canvas camera is not assigned.");
            return Vector2.zero;
        }

        return camera.ScreenToWorldPoint(new Vector3(pointerScreenPos.x, pointerScreenPos.y, camera.nearClipPlane));
    }

	protected void SetSelectedEntityPosition(Vector2 worldPosition) {
		_entityViewModel.SetEntityPosition(SelectedEntityId, (worldPosition.x, worldPosition.y));
		SyncGizmoWithEntity();
	}

	protected void SetSelectedEntityScale(Vector2 scale) {
        _entityViewModel.SetEntityScale(SelectedEntityId, (scale.x, scale.y));
		SyncGizmoWithEntity();
	}

	protected void SetSelectedEntityRoll(float angleDegrees) {
		_entityViewModel.SetEntityRoll(SelectedEntityId, angleDegrees);
		SyncGizmoWithEntity();
	}

}
