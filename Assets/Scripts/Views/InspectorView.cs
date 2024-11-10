using SimpleMotions;
using UnityEngine;
using TMPro;

public class InspectorView : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI _selectedEntityName;
	[SerializeField] private GameObject _transformComponent;
	[SerializeField] private GameObject _shapeComponent;
	[SerializeField] private GameObject _textComponent;

	private IInspectorViewModel _inspectorViewModel;
	private EditorPainter _editorPainter;

    public void Configure(IInspectorViewModel inspectorViewModel, EditorPainter editorPainter) {
        _inspectorViewModel = inspectorViewModel;
		_editorPainter = editorPainter;

		inspectorViewModel.OnEntitySelected.Subscribe(UpdateInspector);
    }

    private void UpdateInspector((int id, string name) entity) {
        _selectedEntityName.text = entity.name;

		CheckTransformComponent(entity.id);
		CheckShapeComponent(entity.id);
		CheckTextComponent(entity.id);
    }

	private void CheckTransformComponent(int entityId) {
		bool selectedEntityHasTransform = _inspectorViewModel.EntityHasTransform(entityId, out var transformData);

		_transformComponent.SetActive(selectedEntityHasTransform);

		if (selectedEntityHasTransform) {
			if (_transformComponent.TryGetComponent<TransformComponentUI>(out var transformComponentUI)) {
				transformComponentUI.RefreshData(transformData);
			}
		}
	}

	private void CheckShapeComponent(int entityId) {
		bool selectedEntityHasShape = _inspectorViewModel.EntityHasShape(entityId, out var shapeData);

		_shapeComponent.SetActive(selectedEntityHasShape);

		if (selectedEntityHasShape) {
			if (_shapeComponent.TryGetComponent<ShapeComponentUI>(out var shapeComponentUI)) {
				shapeComponentUI.RefreshData(shapeData, _editorPainter);
			}
		}
	}

	private void CheckTextComponent(int entityId) {
		bool selectedEntityHasText = _inspectorViewModel.EntityHasText(entityId, out string text);

		_textComponent.SetActive(selectedEntityHasText);

		if (selectedEntityHasText) {
			if (_textComponent.TryGetComponent<TextComponentUI>(out var textComponentUI)) {
				textComponentUI.RefreshData(text);
			}
		}
	}
	
}