using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SimpleMotions {
    
    public sealed class VideoPlaybackView : MonoBehaviour {

        [SerializeField] private Slider _time;
        [SerializeField] private Button _togglePlay;
        [SerializeField] private TextMeshProUGUI _currentTime;
        [SerializeField] private TextMeshProUGUI _duration;

        public void Configure(IVideoPlaybackViewModel videoPlaybackViewModel) {
			_togglePlay.onClick.AddListener(() => videoPlaybackViewModel.TogglePlay.Execute(value: null));
        }

    }
}