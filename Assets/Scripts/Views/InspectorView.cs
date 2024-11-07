using UnityEngine;
using TMPro;
using SimpleMotions;

public class InspectorView : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI _selectedEntityName;

    public void Configure(IInspectorViewModel inspectorViewModel) {
        inspectorViewModel.OnSelectedEntityNameUpdated.Subscribe(UpdateSelectedEntityName);
    }

    private void UpdateSelectedEntityName(string name) {
        _selectedEntityName.text = name;
    }

}