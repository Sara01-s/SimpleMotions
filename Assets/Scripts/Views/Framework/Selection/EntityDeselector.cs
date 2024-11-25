using SimpleMotions;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityDeselector : MonoBehaviour, IPointerDownHandler {

	[SerializeField] private RectTransform _selectionGizmoArea;

	private IEntitySelectorViewModel _entitySelectorViewModel;

	public void Configure(IEntitySelectorViewModel entitySelectorViewModel) {
		_entitySelectorViewModel = entitySelectorViewModel;
	}

	public void OnPointerDown(PointerEventData eventData) {
		if (!RectTransformUtility.RectangleContainsScreenPoint(_selectionGizmoArea, eventData.position, Camera.main)) {
			Deselect();
		}
	}

	public void Deselect() {
		if (_entitySelectorViewModel.HasSelectedEntity) {
			_entitySelectorViewModel.DeselectEntity.Execute();
		}
	}


}