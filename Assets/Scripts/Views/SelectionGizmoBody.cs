using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoBody : MonoBehaviour, IDragHandler, IBeginDragHandler {
    
	[SerializeField] private Transform _canvasOrigin;
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

	public void OnBeginDrag(PointerEventData eventData) {
		_dragOffset = eventData.position - _rectTransform.rect.center;
	}

	public void OnDrag(PointerEventData eventData) {
		int selectedEntityId = _entitySelector.SelectedEntity.Id;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var t)) {
			var deltaMovement = eventData.delta / _editorCanvas.scaleFactor;

			// Drag gizmo
			_rectTransform.anchoredPosition += deltaMovement;

			var camera = _editorCanvas.worldCamera;

			
			var pointerPosWithOffset = eventData.position + _dragOffset;
			var pointerWorldPosition = camera.ScreenToWorldPoint(new Vector3(pointerPosWithOffset.x, pointerPosWithOffset.y, camera.nearClipPlane));
			var pointerInCanvasSpace = _canvasOrigin.transform.InverseTransformPoint(pointerWorldPosition);

			_entityViewModel.SetEntityPosition(_entitySelector.SelectedEntity.Id, (pointerInCanvasSpace.x, pointerInCanvasSpace.y));
		}
	}

}
