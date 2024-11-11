using UnityEngine;
using UnityEngine.UI;
using SimpleMotions;

public class TimelinePanelView : MonoBehaviour {

	[SerializeField] private Button _createEntity;

	public void Configure(ITimelinePanelViewModel timelinePanelViewModel) {
		_createEntity.onClick.AddListener(timelinePanelViewModel.CreateEntity);
	}
}