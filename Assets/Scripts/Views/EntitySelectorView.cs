using SimpleMotions;
using UnityEngine;

public class EntitySelectorView : MonoBehaviour {

	[SerializeField] private RectTransform _rectSelection;

	private IEntitySelectorViewModel _entitySelectorViewModel;

	public void Configure(IEntitySelectorViewModel entitySelectorViewModel) {
		_entitySelectorViewModel = entitySelectorViewModel;
		entitySelectorViewModel.OnEntitySelected.Subscribe(DrawRectSelectionOverEntity);
	}

	private void HideRectSelection() {
		_rectSelection.gameObject.SetActive(false);
	}

	private void DrawRectSelectionOverEntity((int id, string name) entity) {
		if (_entitySelectorViewModel.EntityHasTransform(entity.id, out var _)) {
			var (min, max) = _entitySelectorViewModel.GetEntityBounds(entity.id);

			_rectSelection.anchorMin = new Vector2(min.x, min.y);
			_rectSelection.anchorMax = new Vector2(max.x, max.y);
			_rectSelection.gameObject.SetActive(true);
		}
	}
	
}