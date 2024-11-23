using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public abstract class SelectionGizmo : MonoBehaviour, IDragHandler, IBeginDragHandler {
    
	protected RectTransform _SelectionGizmoRect => (RectTransform)transform.parent;

	private Camera _worldCamera;
	private RectTransform _canvasArea;
    private IEntitySelectorViewModel _entitySelectorViewModel;
	private int SelectedEntityId {
		get {
			if (_entitySelectorViewModel.HasSelectedEntity) {
				return _entitySelectorViewModel.SelectedEntityId;
			}

			Debug.LogError("Selection Gizmo cannot be used when no entity is selected.");
			return -1;
		}
	}

    public void Configure(IEntitySelectorViewModel entitySelectorViewModel, Camera worldCamera, RectTransform canvasArea) {
        _entitySelectorViewModel = entitySelectorViewModel;
		_worldCamera = worldCamera;
		_canvasArea = canvasArea;
    }

    public abstract void OnBeginDrag(PointerEventData eventData);
	public abstract void OnDrag(PointerEventData eventData);
	protected abstract void SyncGizmoWithEntity();

    protected Vector2 _GetPointerWorldPos(Vector2 pointerScreenPos) {
        return _worldCamera.ScreenToWorldPoint(new Vector3(pointerScreenPos.x, pointerScreenPos.y, _worldCamera.nearClipPlane));
    }

	protected Vector2 _WorldToScreenPoint(Vector3 worldPoint) {
		return RectTransformUtility.WorldToScreenPoint(_worldCamera, worldPoint);
	}

	protected Vector2 _ScreenPointToLocalPointInRectangle(Vector2 screenPoint) {
		RectTransformUtility.ScreenPointToLocalPointInRectangle (
			_canvasArea,
			screenPoint,
			_worldCamera,
			out var localPoint
		);

		return localPoint;
	}

	protected void _SetSelectedEntityPosition(Vector2 worldPosition) {
		_entitySelectorViewModel.SetEntityPosition(SelectedEntityId, (worldPosition.x, worldPosition.y));
		SyncGizmoWithEntity();
	}

	protected void _SetSelectedEntityScale(Vector2 scale) {
        _entitySelectorViewModel.SetEntityScale(SelectedEntityId, (scale.x, scale.y));
		SyncGizmoWithEntity();
	}

	protected void _SetSelectedEntityRoll(float angleDegrees) {
		_entitySelectorViewModel.SetEntityRoll(SelectedEntityId, angleDegrees);
		SyncGizmoWithEntity();
	}

	protected ((float x, float y) pos, (float w, float h) scale, float rollAngleDegrees) _GetSelectedEntityTransformData() {
		_entitySelectorViewModel.EntityHasTransform(SelectedEntityId, out var t);
		return t;
	}

}
