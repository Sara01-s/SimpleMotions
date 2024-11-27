using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleMotions;

public abstract class SelectionGizmo : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {
    
	public ReactiveCommand<EntityDTO> OnEntityChanged { get; } = new();

	[SerializeField] protected Image _GizmoImage;

	protected RectTransform _SelectionGizmoRect => (RectTransform)transform.parent;

	private Camera _worldCamera;
	private RectTransform _canvasArea;
    private IEntitySelectorViewModel _entitySelectorViewModel;
	private Color _startImageColor;
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

		if (_GizmoImage == null) {
			_GizmoImage = GetComponent<Image>();
		}
    }

    public abstract void OnBeginDrag(PointerEventData eventData);
	public abstract void OnDrag(PointerEventData eventData);

	private Color _normalColor = Color.magenta;
	private Color _selectedColor = Color.cyan;

	public void OnPointerEnter(PointerEventData eventData) {
		_GizmoImage.color = _selectedColor;
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (!eventData.dragging)  {
			_GizmoImage.color = _normalColor;
		}
	}

	public void OnPointerDown(PointerEventData eventData) {
		_GizmoImage.color = _selectedColor;
	}
	
	public void OnPointerUp(PointerEventData eventData) {
		if (!eventData.dragging)  {
			_GizmoImage.color = _normalColor;
		}
	}

    protected Vector2 _GetPointerWorldPos(Vector2 pointerScreenPos) {
        return _worldCamera.ScreenToWorldPoint(new Vector3(pointerScreenPos.x, pointerScreenPos.y, _worldCamera.nearClipPlane));
    }

	protected Vector2 _WorldToScreenPoint(Vector3 worldPoint) {
		return RectTransformUtility.WorldToScreenPoint(_worldCamera, worldPoint);
	}

	protected Vector2 _ScreenPointToRect(Vector2 screenPoint) {
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
		OnEntityChanged.Execute(new EntityDTO(SelectedEntityId));
	}

	protected void _SetSelectedEntityScale(Vector2 scale) {
		_entitySelectorViewModel.SetEntityScale(SelectedEntityId, (scale.x, scale.y));
		OnEntityChanged.Execute(new EntityDTO(SelectedEntityId));
	}

	protected void _SetSelectedEntityRoll(float angleDegrees) {
		_entitySelectorViewModel.SetEntityRoll(SelectedEntityId, angleDegrees);
		OnEntityChanged.Execute(new EntityDTO(SelectedEntityId));
	}

	protected TransformDTO _GetSelectedEntityTransformData() {
		_entitySelectorViewModel.TryGetEntityTransform(SelectedEntityId, out var transformDTO);
		return transformDTO;
	}

}
