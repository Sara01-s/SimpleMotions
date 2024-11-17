using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoCorner : MonoBehaviour, IDragHandler {
    
	[SerializeField] private Transform _canvasOrigin;
	[SerializeField] private Canvas _editorCanvas;

	private RectTransform _rectTransform;
    private IComponentViewModel _entityViewModel;
    private IEntitySelector _entitySelector;

    public void Configure(IComponentViewModel entityViewModel, IEntitySelector entitySelector) {
		_rectTransform = GetComponent<RectTransform>();
        _entityViewModel = entityViewModel;
        _entitySelector = entitySelector;
    }

	public void OnDrag(PointerEventData eventData) {
		int selectedEntityId = _entitySelector.SelectedEntity.Id;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var t)) {
			var deltaMovement = eventData.delta / _editorCanvas.scaleFactor;

			// Drag gizmo
			_rectTransform.anchoredPosition += deltaMovement;

			var camera = _editorCanvas.worldCamera;

			var pointerWorldPosition = camera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, camera.nearClipPlane));
			var pointerInCanvasSpace = _canvasOrigin.transform.InverseTransformPoint(pointerWorldPosition);

			_entityViewModel.SetEntityPosition(_entitySelector.SelectedEntity.Id, (pointerInCanvasSpace.x, pointerInCanvasSpace.y));
		}
	}

}
