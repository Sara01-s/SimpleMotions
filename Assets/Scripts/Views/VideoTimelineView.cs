using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SimpleMotions {
    
    internal sealed class VideoTimelineView : MonoBehaviour {

        [SerializeField] private Slider _time;
        [SerializeField] private Button _togglePlay;
        [SerializeField] private TextMeshProUGUI _currentTime;
        [SerializeField] private TextMeshProUGUI _duration;
		[SerializeField] private Button _createTestEntity;

        public void Configure(IVideoTimelineViewModel videoTimelineViewModel) {
            videoTimelineViewModel.CurrentTime.Subscribe(UpdateCurrentTime);
            videoTimelineViewModel.Duration.Subscribe(UpdateDuration);

            _togglePlay.onClick.AddListener(() => videoTimelineViewModel.TogglePlay.Execute(null));
			_createTestEntity.onClick.AddListener(() => videoTimelineViewModel.CreateTestEntity.Execute(null));
        }

        private void UpdateCurrentTime(float value) {
            _currentTime.text = value.ToString();
        }

        private void UpdateDuration(float value) {
            _currentTime.text = value.ToString();
        }

    }
}