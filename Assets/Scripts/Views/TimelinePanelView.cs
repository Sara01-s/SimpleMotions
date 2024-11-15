using UnityEngine;
using UnityEngine.UI;
using SimpleMotions;

public class TimelinePanelView : MonoBehaviour {

	[SerializeField] private GameObject _maxEntitiesWarning;
	[SerializeField] private Button _createEntity;

	public void Configure(ITimelinePanelViewModel timelinePanelViewModel) {
		_createEntity.onClick.AddListener(timelinePanelViewModel.TryCreateEntity);
		timelinePanelViewModel.ShowMaxEntitiesWarning.Subscribe(ShowMaxEntitiesWarning);
	}

	private void ShowMaxEntitiesWarning() {
		_maxEntitiesWarning.SetActive(true);
	}
}