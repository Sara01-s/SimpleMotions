using UnityEngine;
using UnityEngine.EventSystems;
using SimpleMotions;

public class SelectionGizmoBody : MonoBehaviour, IDragHandler, IBeginDragHandler {
    
	[SerializeField] private Canvas _mainCanvas;

	private RectTransform _rect;
    private IEntityViewModel _entityViewModel;
    private IEntitySelector _entitySelector;

	private Vector3 _pointerOffset;
	private Camera _mainCamera;

    public void Configure(IEntityViewModel entityViewModel, IEntitySelector entitySelector) {
		_rect = GetComponent<RectTransform>();
        _entityViewModel = entityViewModel;
        _entitySelector = entitySelector;
		_mainCamera = Camera.main;
    }

	public void OnBeginDrag(PointerEventData eventData) {
		_pointerOffset = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
	}

	public void OnDrag(PointerEventData eventData) {
		int selectedEntityId = _entitySelector.SelectedEntity.Id;

		if (_entityViewModel.EntityHasTransform(selectedEntityId)) {
			print("moviendo se supone: " + eventData.delta);

			var pos = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - _pointerOffset;

			print(pos);

			var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - _pointerOffset;
			_entityViewModel.IncrementEntityPosition(selectedEntityId, (pos.x, pos.y));
		}
	}


}
