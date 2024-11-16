using SimpleMotions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EntityPanelView : MonoBehaviour, IPointerClickHandler {

	[SerializeField] private Image _selectionHighlight;
	[SerializeField] private TMP_InputField _entityName;
    [SerializeField] private Toggle _toggleActive;
    [SerializeField] private Button _deleteEntity;

	private IEntitySelectorViewModel _entitySelectorViewModel;
	private int _ownerEntityId;

    public void Configure(ITimelinePanelViewModel timelinePanelViewModel, IEntitySelectorViewModel entitySelectorViewModel, int ownerEntityId) {
        _deleteEntity.onClick.AddListener(() => timelinePanelViewModel.DeleteEntity.Execute(ownerEntityId));
		_toggleActive.onValueChanged.AddListener(active => timelinePanelViewModel.ToggleEntityActive(ownerEntityId, active));
		_entityName.onValueChanged.AddListener(newName => timelinePanelViewModel.ChangeEntityName(ownerEntityId, newName));
		_entityName.text = timelinePanelViewModel.GetEntityName(ownerEntityId);

		timelinePanelViewModel.OnEntityNameChanged.Subscribe((id, name) => {
			if (ownerEntityId == id) {
				_entityName.text = name;
			}
		});

		entitySelectorViewModel.OnEntityDeselected.Subscribe(() => _selectionHighlight.enabled = false);
		entitySelectorViewModel.OnEntitySelected.Subscribe(entity => _selectionHighlight.enabled = ownerEntityId == entity.entityId);
		
		_entitySelectorViewModel = entitySelectorViewModel;
		_ownerEntityId = ownerEntityId;
	}

	public void OnPointerClick(PointerEventData _) {
		_entitySelectorViewModel.SelectEntity.Execute(_ownerEntityId);
	}
}
