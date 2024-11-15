using SimpleMotions;
using UnityEngine;
using UnityEngine.UI;

public class EntityPanelView : MonoBehaviour {

    [SerializeField] private Toggle _toggleActive;
    [SerializeField] private Button _deleteEntity;

    public void Configure(ITimelinePanelViewModel timelinePanelViewModel, int ownerEntityId) {
        _deleteEntity.onClick.AddListener(() => timelinePanelViewModel.DeleteEntity.Execute(ownerEntityId));
		_toggleActive.onValueChanged.AddListener(active => timelinePanelViewModel.EntityToggleActive(ownerEntityId, active));
	}
	
}
