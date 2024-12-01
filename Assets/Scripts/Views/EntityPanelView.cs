using SimpleMotions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EntityPanelView : MonoBehaviour, IPointerClickHandler, System.IDisposable {

	[SerializeField] private Image _selectionHighlight;
	[SerializeField] private TMP_InputField _entityName;
    [SerializeField] private Toggle _toggleActive;
    [SerializeField] private Button _deleteEntity;
	[SerializeField] private Button _moveLayerUp;
	[SerializeField] private Button _moveLayerDown;
	
	private IEntitySelectorViewModel _entitySelectorViewModel;
	private GridLayoutGroup _containerGrid;
	private int _ownerEntityId;

    public void Configure(IVideoCanvasViewModel videoCanvasViewModel, ITimelinePanelViewModel timelinePanelViewModel, IEntitySelectorViewModel entitySelectorViewModel, int ownerEntityId) {
		const string defaultEntityName = "New Entity";
		_entityName.text = defaultEntityName;
		_containerGrid = GetComponentInParent<GridLayoutGroup>();

		_toggleActive.onValueChanged.AddListener(active => timelinePanelViewModel.ToggleEntityActive(ownerEntityId, active));
		_entityName.onSubmit.AddListener(newName => timelinePanelViewModel.ChangeEntityName(ownerEntityId, newName));
		_entityName.onSubmit.Invoke(defaultEntityName);
		_entityName.onDeselect.AddListener(newName => timelinePanelViewModel.ChangeEntityName(ownerEntityId, newName));

        _deleteEntity.onClick.AddListener(() => {
			timelinePanelViewModel.DeleteEntity.Execute(ownerEntityId);
			Dispose();
			Destroy(gameObject);
		});

		timelinePanelViewModel.OnEntityNameChanged.Subscribe(entityDTO => {
			if (ownerEntityId == entityDTO.Id) {
				_entityName.text = entityDTO.Name;
			}
		});

		_moveLayerUp.onClick.AddListener(() => {
			videoCanvasViewModel.IncreaseEntityLayer.Execute(ownerEntityId);

			// Move up
			//int panelIndex = transform.GetSiblingIndex();
			//if (panelIndex > 0) {
			//	transform.SetSiblingIndex(panelIndex - 1);
			//}
			//else {
			//
			//}
		});

		_moveLayerDown.onClick.AddListener(() => {
			videoCanvasViewModel.DecreaseEntityLayer.Execute(ownerEntityId);
		});

		entitySelectorViewModel.OnEntitySelected.Subscribe(EnableHightlight);
		entitySelectorViewModel.OnEntityDeselected.Subscribe(DisableHighlight);
		
		_entitySelectorViewModel = entitySelectorViewModel;
		_ownerEntityId = ownerEntityId;

		SelectOwnerEntity();
	}

	private void DisableHighlight() {
		_selectionHighlight.enabled = false;
	}

	private void EnableHightlight(EntityDTO entityDTO) {
		_selectionHighlight.enabled = _ownerEntityId == entityDTO.Id;
	}

	public void OnPointerClick(PointerEventData _) {
		SelectOwnerEntity();
	}

	private void SelectOwnerEntity() {
		_entitySelectorViewModel.SelectEntity.Execute(_ownerEntityId);
	}

	public void Dispose() {
		_entitySelectorViewModel.OnEntitySelected.Unsubscribe(EnableHightlight);
		_entitySelectorViewModel.OnEntityDeselected.Unsubscribe(DisableHighlight);
		_deleteEntity.onClick.RemoveAllListeners();
		_entityName.onValueChanged.RemoveAllListeners();
		_toggleActive.onValueChanged.RemoveAllListeners();
	}
}
