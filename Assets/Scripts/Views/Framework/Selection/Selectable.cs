using UnityEngine;
using SimpleMotions;

[RequireComponent(typeof(BoxCollider2D))]
public class Selectable : MonoBehaviour {

	public int OwnerId { get; private set; } = -1;

	private IEntitySelectorViewModel _entitySelectorViewModel;

	public void Configure(int ownerId, IEntitySelectorViewModel entitySelectorViewModel) {
		OwnerId = ownerId;
		_entitySelectorViewModel = entitySelectorViewModel;
	}

	private void OnMouseDown() {
		if (OwnerId == -1) {
			Debug.LogError("Assign a valid owner id to selectable");
		}

		print("Entiidad clickeada, seleccionando: " + OwnerId);
		_entitySelectorViewModel.SelectEntity.Execute(OwnerId);
	}
	
}