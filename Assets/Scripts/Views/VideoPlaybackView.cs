using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

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

	public void Configure(IVideoPlaybackViewModel videoPlaybackViewModel) {
		_videoPlaybackViewModel = videoPlaybackViewModel;

		InitReactiveCommands();
		InitReactiveValues();
		RefreshUI();
	}

	private void SetCurrentTime(float currentTime) {
		_currentTime.text = $"{currentTime:00:00:00}";
	}

	private void SetCurrentFrame(int currentFrame) {
		_currentFrame.text = currentFrame.ToString();
	}

	private void SetCurrentIcon(bool isPlaying) {
		_play.SetActive(!isPlaying);
		_pause.SetActive(isPlaying);
	}

	private void SetTotalFrames(int totalFrames) {
		_totalFrames.text = totalFrames.ToString();
	}

	private void SetDuration(float durationSeconds) {
		_duration.text = $"{durationSeconds:00:00:00}";
	}

	private void InitReactiveCommands() {
		_firstFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnFirstFrameUpdated.Execute(value: null));
		_backward.onClick.AddListener(() => _videoPlaybackViewModel.OnBackwardUpdated.Execute(value: null));
		_togglePlay.onClick.AddListener(() => _videoPlaybackViewModel.OnTogglePlayUpdated.Execute(value: null));
		_forward.onClick.AddListener(() => _videoPlaybackViewModel.OnForwardUpdated.Execute(value: null));
		_lastFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnLastFrameUpdated.Execute(value: null));
	}

	private void InitReactiveValues() {
		_videoPlaybackViewModel.IsPlaying.Subscribe(SetCurrentIcon);
		_videoPlaybackViewModel.CurrentTime.Subscribe(SetCurrentTime);
		_videoPlaybackViewModel.DurationSeconds.Subscribe(SetDuration);
		_videoPlaybackViewModel.CurrentFrame.Subscribe(SetCurrentFrame);
		_videoPlaybackViewModel.TotalFrames.Subscribe(SetTotalFrames);
	}

	private void RefreshUI() {
		_videoPlaybackViewModel.RefreshData();
	}

}