using SimpleMotions;
using UnityEngine;

public class EntitySelectorView : MonoBehaviour {

	[SerializeField] private Canvas _editorCanvas;
	[SerializeField] private RectTransform _selectionGizmo;
	[SerializeField] private SelectionGizmo[] _selectionGizmoParts;

	private IEntitySelectorViewModel _entitySelectorViewModel;

	public void Configure(IEntitySelectorViewModel entitySelectorViewModel) {
		entitySelectorViewModel.OnEntitySelected.Subscribe(DrawSelectionGizmoOverEntity);
		entitySelectorViewModel.OnEntityDeselected.Subscribe(HideSelectionGizmo);
		_entitySelectorViewModel = entitySelectorViewModel;

		foreach (var selectionGizmoPart in _selectionGizmoParts) {
			selectionGizmoPart.Configure(entitySelectorViewModel, _editorCanvas.worldCamera, (RectTransform)_editorCanvas.transform);
		}
	}

	private void HideSelectionGizmo() {
		_selectionGizmo.gameObject.SetActive(false);
	}

	private void DrawSelectionGizmoOverEntity((int id, string _) entity) {
		if (_entitySelectorViewModel.EntityHasTransform(entity.id, out var t)) {
			var entityScreenPos = _editorCanvas.worldCamera.WorldToScreenPoint(new Vector2(t.pos.x, t.pos.y));
			_selectionGizmo.anchoredPosition = entityScreenPos;
			_selectionGizmo.localScale = new Vector2(t.scale.w, t.scale.h);
			_selectionGizmo.localRotation = Quaternion.AngleAxis(t.rollAngleDegrees, Vector3.forward);
			_selectionGizmo.gameObject.SetActive(true);
		}
	}
	
}