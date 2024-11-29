using SimpleMotions;
using UnityEngine;
using TMPro;

public class InspectorView : MonoBehaviour {
    
	[SerializeField] private TMP_InputField _selectedEntityName;
	[SerializeField] private GameObject _transformComponent;
	[SerializeField] private GameObject _shapeComponent;
	[SerializeField] private GameObject _textComponent;

	private IInspectorViewModel _inspectorViewModel;

    public void Configure(IInspectorViewModel inspectorViewModel) {
		_selectedEntityName.onSubmit.AddListener(name => inspectorViewModel.SelectedEntityName = name);
		_selectedEntityName.onDeselect.AddListener(name => inspectorViewModel.SelectedEntityName = name);

		inspectorViewModel.OnEntitySelected.Subscribe(UpdateInspector);
		inspectorViewModel.OnEntityDeselected.Subscribe(ClearInspector);
		inspectorViewModel.OnClearInspector.Subscribe(ClearInspector);

        _inspectorViewModel = inspectorViewModel;
		InitializeUI();
    }

	private void InitializeUI() {
		_transformComponent.SetActive(false);
		_shapeComponent.SetActive(false);
		_textComponent.SetActive(false);
	}

	private void ClearInspector() {
		InitializeUI();
		_selectedEntityName.text = string.Empty;
	}

    private void UpdateInspector(EntityDTO entityDTO) {
        _selectedEntityName.text = entityDTO.Name;

		CheckTransformComponent(entityDTO.Id);
		CheckShapeComponent(entityDTO.Id);
		CheckTextComponent(entityDTO.Id);
    }

	private void CheckTransformComponent(int entityId) {
		bool selectedEntityHasTransform = _inspectorViewModel.TryGetEntityTransform(entityId, out var transformData);

		_transformComponent.SetActive(selectedEntityHasTransform);

		if (selectedEntityHasTransform) {
			if (_transformComponent.TryGetComponent<TransformComponentView>(out var transformComponentView)) {
				transformComponentView.RefreshData(transformData);
			}
		}
	}

	private void CheckShapeComponent(int entityId) {
		bool selectedEntityHasShape = _inspectorViewModel.TryGetEntityShape(entityId, out var shapeData);

		_shapeComponent.SetActive(selectedEntityHasShape);

		if (selectedEntityHasShape) {
			if (_shapeComponent.TryGetComponent<ShapeComponentView>(out var shapeComponentView)) {
				shapeComponentView.RefreshData(shapeData);
			}
		}
	}

	private void CheckTextComponent(int entityId) {
		bool selectedEntityHasText = _inspectorViewModel.TryGetEntityText(entityId, out string text);

		_textComponent.SetActive(selectedEntityHasText);

		if (selectedEntityHasText) {
			if (_textComponent.TryGetComponent<TextComponentView>(out var textComponentView)) {
				textComponentView.RefreshData(text);
			}
		}
	}
	
}