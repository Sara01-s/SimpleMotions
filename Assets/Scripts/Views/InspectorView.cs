using UnityEngine;
using TMPro;
using SimpleMotions;

public class InspectorView : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI _selectedEntityName;
	[SerializeField] private GameObject _transformComponent;
	[SerializeField] private GameObject _shapeComponent;
	[SerializeField] private GameObject _textComponent;

	private IInspectorViewModel _inspectorViewModel;

    public void Configure(IInspectorViewModel inspectorViewModel) {
        _inspectorViewModel = inspectorViewModel;
		inspectorViewModel.OnEntitySelected.Subscribe(UpdateInspector);
    }

    private void UpdateInspector(EntityDisplayInfo info) {
        _selectedEntityName.text = info.EntityName;

		CheckTransformComponent(info.EntityId);
		CheckShapeComponent(info.EntityId);
		CheckTextComponent(info.EntityId);
    }

	private void CheckTransformComponent(int entityId) {
		bool selectedEntityHasTransform = _inspectorViewModel.EntityHasTransform(entityId, out var transformData);

		_transformComponent.SetActive(selectedEntityHasTransform);

		if (selectedEntityHasTransform) {
			// TODO -  utilizar transformData para pintar los datos del transform en el inspector
		}
	}

	private void CheckShapeComponent(int entityId) {
		bool selectedEntityHasShape = _inspectorViewModel.EntityHasShape(entityId, out var shapeData);

		_shapeComponent.SetActive(selectedEntityHasShape);

		if (selectedEntityHasShape) {
			// TODO -  utilizar shapeData para pintar los datos del shape en el inspector
		}
	}

	private void CheckTextComponent(int entityId) {
		bool selectedEntityHasText = _inspectorViewModel.EntityHasText(entityId, out string text);

		_textComponent.SetActive(selectedEntityHasText);

		if (selectedEntityHasText) {
			// TODO -  utilizar text para pintar los datos del texto en el inspector
		}
	}

}