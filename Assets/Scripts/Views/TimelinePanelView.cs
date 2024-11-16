using UnityEngine;
using UnityEngine.UI;
using SimpleMotions;

public class TimelinePanelView : MonoBehaviour {

	[SerializeField] private Transform _entityHierarchy;
	[SerializeField] private GameObject _entityPanelPrefab;
	[SerializeField] private GameObject _maxEntitiesWarning;
	[SerializeField] private Button _createEntity;

	public void Configure(ITimelinePanelViewModel timelinePanelViewModel, IEntitySelectorViewModel entitySelectorViewModel) {
		timelinePanelViewModel.ShowMaxEntitiesWarning.Subscribe(ShowMaxEntitiesWarning);

		_createEntity.onClick.AddListener(() => {
			if (timelinePanelViewModel.TryCreateEntity(out int createdEntityId)) {
				var entityPanel = Instantiate(_entityPanelPrefab, parent: _entityHierarchy).GetComponent<EntityPanelView>();
				entityPanel.Configure(timelinePanelViewModel, entitySelectorViewModel, createdEntityId);
			}
		});
	}

	private void ShowMaxEntitiesWarning() {
		_maxEntitiesWarning.SetActive(true);
	}
}