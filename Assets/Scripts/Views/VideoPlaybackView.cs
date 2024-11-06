using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace SimpleMotions {
    
    public sealed class VideoPlaybackView : MonoBehaviour {

        [SerializeField] private Button _firstFrame;
        [SerializeField] private Button _backward;
        [SerializeField] private Button _togglePlay;
        [SerializeField] private Button _forward;
        [SerializeField] private Button _lastFrame;

        [SerializeField] private TextMeshProUGUI _currentTime;
        [SerializeField] private TextMeshProUGUI _currentFrame;
        [SerializeField] private TextMeshProUGUI _totalFrames;
        [SerializeField] private TextMeshProUGUI _duration;

        [SerializeField] private GameObject _play;
        [SerializeField] private GameObject _pause;

        private IVideoPlaybackViewModel _videoPlaybackViewModel;
        private bool _isPlaying;

        public void Configure(IVideoPlaybackViewModel videoPlaybackViewModel) {
            _videoPlaybackViewModel = videoPlaybackViewModel;

            _firstFrame.onClick.AddListener(() => _videoPlaybackViewModel.FirstFrame.Execute(value: null));
            _backward.onClick.AddListener(() => _videoPlaybackViewModel.Backward.Execute(value: null));

            _togglePlay.onClick.AddListener(() => {
                _videoPlaybackViewModel.TogglePlay.Execute(value: null);
                SetIcon();
            });
            
            _forward.onClick.AddListener(() => _videoPlaybackViewModel.Forward.Execute(value: null));
            _lastFrame.onClick.AddListener(() => _videoPlaybackViewModel.LastFrame.Execute(value: null));

            _videoPlaybackViewModel.CurrentTime.Subscribe(SetCurrentTime);
            _videoPlaybackViewModel.CurrentFrame.Subscribe(SetCurrentFrame);
            _videoPlaybackViewModel.TotalFrames.Subscribe(SetTotalFrames);
            _videoPlaybackViewModel.Duration.Subscribe(SetDuration);
        }

        private void SetIcon() {
            _isPlaying = !_isPlaying;

            _play.SetActive(!_isPlaying);
            _pause.SetActive(_isPlaying);
        }

        private void SetCurrentTime(float currentTime) {
			_currentTime.text = $"{currentTime:00:00:00}";
        }

        private void SetCurrentFrame(int currentFrame) {
            _currentFrame.text = currentFrame.ToString();
        }

        private void SetTotalFrames(int totalFrames) {
            _totalFrames.text = totalFrames.ToString();
        }

        private void SetDuration(float durationSeconds) {
            _duration.text = $"{durationSeconds:00:00:00}";
        }

    }
}