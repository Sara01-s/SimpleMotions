using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoBody : MonoBehaviour, IDragHandler {
    
	[SerializeField] private Canvas _mainCanvas;

	private RectTransform _rect;
    private IEntityViewModel _entityViewModel;
    private IEntitySelector _entitySelector;

    public void Configure(IEntityViewModel entityViewModel, IEntitySelector entitySelector) {
		_rect = GetComponent<RectTransform>();
        _entityViewModel = entityViewModel;
        _entitySelector = entitySelector;
    }

    public void OnDrag(PointerEventData eventData) {
		int selectedEntityId = _entitySelector.SelectedEntity.Id;
	
		if (_entityViewModel.EntityHasTransform(selectedEntityId)) {
			print("moviendo se supone: " + eventData.delta);
			var delta = (eventData.delta.x, eventData.delta.y);

			_entityViewModel.IncrementEntityPosition(selectedEntityId, delta);

			RectTransformUtility.ScreenPointToLocalPointInRectangle (
				_mainCanvas.transform as RectTransform,
				Input.mousePosition,
				_mainCanvas.worldCamera,
				out var movePosition
			);

			_rect.transform.position = _mainCanvas.transform.TransformPoint(movePosition);
		}
    }

}
