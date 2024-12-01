using UnityEngine;
using SimpleMotions;

[RequireComponent(typeof(BoxCollider2D))]
public class EntitySelector : MonoBehaviour {

	public int OwnerId { get; private set; } = -1;

	private IEntitySelectorViewModel _entitySelectorViewModel;
	private RectTransform _selectionGizmo;
	private float _lastClickTime = 0.0f;
	private float _doubleClickThreshold = 0.3f;  // In seconds.

	public void Configure(int ownerId, IEntitySelectorViewModel entitySelectorViewModel) {
		OwnerId = ownerId;
		_entitySelectorViewModel = entitySelectorViewModel;
		_selectionGizmo = GameObject.Find("Image - Selection Rotate").GetComponent<RectTransform>(); // FIXME - remove this filthy hack.
	}

	private void OnMouseDown() {
		if (!IsDoubleClick()) {
			if (_entitySelectorViewModel.HasSelectedEntity && _entitySelectorViewModel.SelectedEntityId != OwnerId) {
				if (RectTransformUtility.RectangleContainsScreenPoint(_selectionGizmo, Input.mousePosition, Camera.main)) {
					_lastClickTime = Time.time;
					return;
				}
			}
		}

		Select();
		_lastClickTime = Time.time;
	}

	private bool IsDoubleClick() {
		return Time.time - _lastClickTime <= _doubleClickThreshold;
	}


	public void Select() {
		if (OwnerId == -1) {
			Debug.LogError("Assign a valid owner id to selectable");
		}

		_entitySelectorViewModel.SelectEntity.Execute(OwnerId);
	}
	
}