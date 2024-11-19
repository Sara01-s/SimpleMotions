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
		var camera = _editorCanvas.worldCamera;
		var pointerWorldPosition = camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, camera.nearClipPlane));

		int selectedEntityId = _entitySelector.SelectedEntity.Id;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var transform)) {
			var objectWorldPosition = new Vector2(transform.pos.x, transform.pos.y);

			_dragOffset = objectWorldPosition - (Vector2)pointerWorldPosition;
		}
	}

	public void OnDrag(PointerEventData eventData) {
		DragGizmo(eventData.delta);
		DragEntity(eventData.position);
	}

	private void DragEntity(Vector2 pointerPosition) {
		var camera = _editorCanvas.worldCamera;
		var pointerWorldPosition = camera.ScreenToWorldPoint(new Vector3(pointerPosition.x, pointerPosition.y, camera.nearClipPlane));

		var newWorldPosition = (Vector2)pointerWorldPosition + _dragOffset;

		int selectedEntityId = _entitySelector.SelectedEntity.Id;

		_entityViewModel.SetEntityPosition(selectedEntityId, (newWorldPosition.x, newWorldPosition.y));
		_rectTransform.localPosition = camera.WorldToScreenPoint(newWorldPosition);
	}

	private void DragGizmo(Vector2 pointerDelta) {
		//_rectTransform.anchoredPosition += pointerDelta / _editorCanvas.scaleFactor;
	}

}
