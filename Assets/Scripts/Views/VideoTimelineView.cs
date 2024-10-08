using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SimpleMotions {
    
    public sealed class VideoTimelineView : MonoBehaviour {

        [SerializeField] private Slider _time;
        [SerializeField] private Button _togglePlay;
        [SerializeField] private TextMeshProUGUI _currentTime;
        [SerializeField] private TextMeshProUGUI _duration;
		[SerializeField] private Button _createTestEntity;

        public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
			_createTestEntity.onClick.AddListener(() => videoTimelineViewModel.CreateTestEntity.Execute(null));
        }

    }
}