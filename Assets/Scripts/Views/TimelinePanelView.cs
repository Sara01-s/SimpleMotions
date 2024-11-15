using UnityEngine;
using UnityEngine.UI;
using SimpleMotions;

public class TimelinePanelView : MonoBehaviour {

	[SerializeField] private GameObject _maxEntitiesWarning;
	[SerializeField] private Button _createEntity;

	private ITimelinePanelViewModel _timelinePanelViewModel;
	private IEntitySelector _entitySelector;

	public void Configure(ITimelinePanelViewModel timelinePanelViewModel, IEntitySelector entitySelector) {
		_createEntity.onClick.AddListener(timelinePanelViewModel.TryCreateEntity);
		timelinePanelViewModel.ShowMaxEntitiesWarning.Subscribe(ShowMaxEntitiesWarning);

		_timelinePanelViewModel = timelinePanelViewModel;
		_entitySelector = entitySelector;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			DeleteSelectedEntity();
		}
	}

	private void DeleteSelectedEntity() {
		print("hola");
		_timelinePanelViewModel.DeleteEntity.Execute(_entitySelector.SelectedEntity.Id);
	}

	private void ShowMaxEntitiesWarning() {
		_maxEntitiesWarning.SetActive(true);
	}
}