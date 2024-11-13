using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoBody : MonoBehaviour, IDragHandler {
    
	[SerializeField] RectTransform _canvasArea;
	[SerializeField] private Camera _canvasCamera;
	[SerializeField] private Canvas _editorCanvas;

	private RectTransform _rectTransform;
    private IEntityViewModel _entityViewModel;
    private IEntitySelector _entitySelector;

    public void Configure(IEntityViewModel entityViewModel, IEntitySelector entitySelector) {
		_rectTransform = GetComponent<RectTransform>();
        _entityViewModel = entityViewModel;
        _entitySelector = entitySelector;
    }

	public void OnDrag(PointerEventData eventData) {
		int selectedEntityId = _entitySelector.SelectedEntity.Id;

		if (_entityViewModel.EntityHasTransform(selectedEntityId, out var t)) {
			// Drag gizmo
			_rectTransform.anchoredPosition += eventData.delta / _editorCanvas.scaleFactor;

			// Drag entity
			var entityScreenPos = _editorCanvas.worldCamera.WorldToScreenPoint(new Vector2(t.pos.x, t.pos.y));

			RectTransformUtility.ScreenPointToLocalPointInRectangle (
				_canvasArea,
				Input.mousePosition,
				_editorCanvas.worldCamera,
				out var localPoint
			);

			print(localPoint);
			return;
		}
	}

}
