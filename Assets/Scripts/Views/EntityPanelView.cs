using SimpleMotions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EntityPanelView : MonoBehaviour, IPointerClickHandler, System.IDisposable {

	public int OwnerEntityId { get; private set; }

	[SerializeField] private Image _selectionHighlight;
	[SerializeField] private TMP_InputField _entityName;
    [SerializeField] private Toggle _toggleActive;
    [SerializeField] private Button _deleteEntity;
	[SerializeField] private Button _moveLayerUp;
	[SerializeField] private Button _moveLayerDown;
	
	private IEntitySelectorViewModel _entitySelectorViewModel;

    public void Configure(IVideoCanvasViewModel videoCanvasViewModel, ITimelinePanelViewModel timelinePanelViewModel, IEntitySelectorViewModel entitySelectorViewModel, int ownerEntityId) {
		const string defaultEntityName = "New Entity";

		_entityName.text = defaultEntityName;
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
			// Move entity panel up
			int panelIndex = transform.GetSiblingIndex();
			int upperPanelIndex = panelIndex - 1;

			if (upperPanelIndex < 0) {
				return;
			}

			var upperPanel = transform.parent.GetChild(upperPanelIndex);

			transform.SetSiblingIndex(upperPanelIndex);
			upperPanel.SetSiblingIndex(panelIndex);

			UpdateSortingIndex(upperPanel);
		});

		_moveLayerDown.onClick.AddListener(() => {
			// Move entity panel down
			int panelIndex = transform.GetSiblingIndex();
			int belowPanelIndex = panelIndex + 1;

			if (belowPanelIndex >= transform.parent.childCount) {
				return;
			}

			var belowPanel = transform.parent.GetChild(belowPanelIndex);

			transform.SetSiblingIndex(belowPanelIndex);
			belowPanel.SetSiblingIndex(panelIndex);

			UpdateSortingIndex(belowPanel);
		});

		timelinePanelViewModel.OnEntityCreated.Subscribe(() => {
		});
		 
		transform.SetAsFirstSibling();
		UpdateSortingIndex(otherSibling: null);

		entitySelectorViewModel.OnEntitySelected.Subscribe(EnableHightlight);
		entitySelectorViewModel.OnEntityDeselected.Subscribe(DisableHighlight);

		_entitySelectorViewModel = entitySelectorViewModel;
		OwnerEntityId = ownerEntityId;

		SelectOwnerEntity();

		void UpdateSortingIndex(Transform otherSibling) {
			int totalSiblings = transform.parent.childCount;

			int currentSortingIndex = totalSiblings - 1 - transform.GetSiblingIndex();

			videoCanvasViewModel.SetEntitySortingIndex.Execute(ownerEntityId, currentSortingIndex);
			transform.name = $"Entity Panel #{currentSortingIndex}";

			if (otherSibling != null) {
				int otherSortingIndex = totalSiblings - 1 - otherSibling.GetSiblingIndex();
				videoCanvasViewModel.SetEntitySortingIndex.Execute(
					otherSibling.GetComponent<EntityPanelView>().OwnerEntityId, 
					otherSortingIndex
				);
				otherSibling.name = $"Entity Panel #{otherSortingIndex}";
			}
		}
	}

	private void DisableHighlight() {
		_selectionHighlight.enabled = false;
	}

	private void EnableHightlight(EntityDTO entityDTO) {
		_selectionHighlight.enabled = OwnerEntityId == entityDTO.Id;
	}

	public void OnPointerClick(PointerEventData _) {
		SelectOwnerEntity();
	}

	private void SelectOwnerEntity() {
		_entitySelectorViewModel.SelectEntity.Execute(OwnerEntityId);
	}

	public void Dispose() {
		_entitySelectorViewModel.OnEntitySelected.Unsubscribe(EnableHightlight);
		_entitySelectorViewModel.OnEntityDeselected.Unsubscribe(DisableHighlight);
		_deleteEntity.onClick.RemoveAllListeners();
		_entityName.onValueChanged.RemoveAllListeners();
		_toggleActive.onValueChanged.RemoveAllListeners();
	}
}
