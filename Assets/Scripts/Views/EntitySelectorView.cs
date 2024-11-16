using SimpleMotions;
using UnityEngine;

public class EntitySelectorView : MonoBehaviour {

	[SerializeField] private EditorPainter _editorPainter;
	[SerializeField] private RectTransform _selectionGizmo;

	private IEntitySelectorViewModel _entitySelectorViewModel;

	public void Configure(IEntitySelectorViewModel entitySelectorViewModel) {
		entitySelectorViewModel.OnEntitySelected.Subscribe(DrawSelectionGizmoOverEntity);
		entitySelectorViewModel.OnEntityDeselected.Subscribe(HideSelectionGizmo);
		_entitySelectorViewModel = entitySelectorViewModel;

		_selectionGizmo.GetComponent<UnityEngine.UI.Image>().color = _editorPainter.Theme.AccentColor;
	}

	private void HideSelectionGizmo() {
		_selectionGizmo.gameObject.SetActive(false);
	}

	private void DrawSelectionGizmoOverEntity((int id, string _) entity) {
		if (_entitySelectorViewModel.EntityHasTransform(entity.id, out var t)) {
			_selectionGizmo.anchoredPosition = new Vector2(t.pos.x, t.pos.y);
			_selectionGizmo.localScale = new Vector2(t.scale.w, t.scale.h);
			_selectionGizmo.rotation = Quaternion.AngleAxis(t.rollAngleDegrees, Vector3.forward);
			_selectionGizmo.gameObject.SetActive(true);
		}
	}
	
}