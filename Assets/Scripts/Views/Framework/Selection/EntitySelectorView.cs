using SimpleMotions;
using UnityEngine;

public class EntitySelectorView : MonoBehaviour {

	[SerializeField] private Canvas _editorCanvas;
	[SerializeField] private Transform _canvasOrigin;
	[SerializeField] private RectTransform _canvasArea;
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
			var entityWorldPos = _canvasOrigin.TransformPoint(new Vector2(t.pos.x, t.pos.y));
			var entityScreenPos = _editorCanvas.worldCamera.WorldToScreenPoint(entityWorldPos);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasArea, entityScreenPos, _editorCanvas.worldCamera, out var entityRectPos);
			
			float widthInRect = _selectionGizmo.rect.width * t.scale.w;
			float heightInRect = _selectionGizmo.rect.height * t.scale.h;
			
			_selectionGizmo.anchoredPosition = entityRectPos;
			_selectionGizmo.sizeDelta = new Vector2(widthInRect, heightInRect);
			_selectionGizmo.localRotation = Quaternion.AngleAxis(t.rollAngleDegrees, Vector3.forward);
			_selectionGizmo.gameObject.SetActive(true);
		}
	}
	
}