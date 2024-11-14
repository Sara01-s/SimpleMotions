using SimpleMotions;
using UnityEngine;
using UnityEngine.UI;

public class EntityPanelView : MonoBehaviour {

    [SerializeField] private Button _toggleActive;
    [SerializeField] private Button _deleteEntity;

    public void Configure(ITimelinePanelViewModel timelinePanelViewModel, int ownerEntityId) {
        _deleteEntity.onClick.AddListener(() => timelinePanelViewModel.DeleteEntity.Execute(ownerEntityId));
    }
}
