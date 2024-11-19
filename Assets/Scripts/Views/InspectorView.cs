using SimpleMotions;
using UnityEngine;
using TMPro;

public class InspectorView : MonoBehaviour {
    
	[SerializeField] private TMP_InputField _selectedEntityName;
	[SerializeField] private GameObject _transformComponent;
	[SerializeField] private GameObject _shapeComponent;
	[SerializeField] private GameObject _textComponent;

	private IInspectorViewModel _inspectorViewModel;
	private EditorPainter _editorPainter;

    public void Configure(IInspectorViewModel inspectorViewModel, EditorPainter editorPainter) {
        _inspectorViewModel = inspectorViewModel;
		_editorPainter = editorPainter;

		_selectedEntityName.onValueChanged.AddListener(name => inspectorViewModel.SelectedEntityName = name);
		inspectorViewModel.OnEntitySelected.Subscribe(UpdateInspector);
		inspectorViewModel.OnClearInspector.Subscribe(ClearInspector);

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
			if (_transformComponent.TryGetComponent<TransformComponentView>(out var transformComponentView)) {
				print("me voy a matar"); // TODO - Julián arreglará esto.
				//transformComponentView.SetData(transformData); // FIXME - PQ CHUCHA SE CONVIERTEN EN ENTEROS A VECES.
			}
		}
	}

	private void CheckShapeComponent(int entityId) {
		bool selectedEntityHasShape = _inspectorViewModel.EntityHasShape(entityId, out var shapeData);

		_shapeComponent.SetActive(selectedEntityHasShape);

		if (selectedEntityHasShape) {
			if (_shapeComponent.TryGetComponent<ShapeComponentView>(out var shapeComponentView)) {
				shapeComponentView.RefreshData(shapeData, _editorPainter);
			}
		}
	}

	private void CheckTextComponent(int entityId) {
		bool selectedEntityHasText = _inspectorViewModel.EntityHasText(entityId, out string text);

		_textComponent.SetActive(selectedEntityHasText);

		if (selectedEntityHasText) {
			if (_textComponent.TryGetComponent<TextComponentView>(out var textComponentView)) {
				textComponentView.RefreshData(text);
			}
		}
	}
	
}