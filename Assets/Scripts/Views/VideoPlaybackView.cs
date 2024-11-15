using UnityEngine.UI;
using SimpleMotions;
using UnityEngine;
using TMPro;

public sealed class VideoPlaybackView : MonoBehaviour {

	[SerializeField] private TextMeshProUGUI _currentTime;
	[SerializeField] private TextMeshProUGUI _duration;

	[SerializeField] private Button _firstFrame;
	[SerializeField] private Button _backward;
	[SerializeField] private Button _togglePlay;
	[SerializeField] private Button _forward;
	[SerializeField] private Button _lastFrame;

	[SerializeField] private TMP_InputField _currentFrame;
	[SerializeField] private TMP_InputField _totalFrames;

	[SerializeField] private Toggle _loopToggle;

	[SerializeField] private GameObject _play;
	[SerializeField] private GameObject _pause;

	private IVideoPlaybackViewModel _videoPlaybackViewModel;
	private IInputValidator _inputValidator;

	private string _previousCurrentFrame;
	private string _previousTotalFrames;

	public void Configure(IVideoPlaybackViewModel videoPlaybackViewModel, IInputValidator inputValidator) {
		_videoPlaybackViewModel = videoPlaybackViewModel;
		_inputValidator = inputValidator;

		InitReactiveCommands();
		InitReactiveValues();
		RefreshUI();
	}

	private void InitReactiveCommands() {
		_firstFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnFirstFrame.Execute());
		_backward.onClick.AddListener(() => _videoPlaybackViewModel.OnBackwardFrame.Execute());
		_togglePlay.onClick.AddListener(() => _videoPlaybackViewModel.OnTogglePlay.Execute());
		_forward.onClick.AddListener(() => _videoPlaybackViewModel.OnForwardFrame.Execute());
		_lastFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnLastFrame.Execute());

		_loopToggle.onValueChanged.AddListener(isLooping => _videoPlaybackViewModel.IsLooping.Execute(isLooping));

		_currentFrame.onValueChanged.AddListener(currentFrame => {
			currentFrame = _inputValidator.ValidateInput(currentFrame, _previousCurrentFrame);
			int.TryParse(currentFrame, out var newFrame);

			if (newFrame > _videoPlaybackViewModel.TotalFrames.Value) {
				newFrame = _videoPlaybackViewModel.TotalFrames.Value;
			}

			_videoPlaybackViewModel.OnSetCurrentFrame.Execute(newFrame);
		});

		_totalFrames.onValueChanged.AddListener(totalFrames => {
			totalFrames = _inputValidator.ValidateInput(totalFrames, _previousTotalFrames);
			int.TryParse(totalFrames, out var newFrame);

			_videoPlaybackViewModel.OnSetTotalFrames.Execute(newFrame);
		});
	}

	private void InitReactiveValues() {
		_videoPlaybackViewModel.IsPlaying.Subscribe(isPlaying => {
			_play.SetActive(!isPlaying);
			_pause.SetActive(isPlaying);
		});

		_videoPlaybackViewModel.CurrentFrame.Subscribe(currentFrame => {
			_currentFrame.text = currentFrame.ToString();
			_previousCurrentFrame = currentFrame.ToString();
		});

		_videoPlaybackViewModel.TotalFrames.Subscribe(totalFrames => {
			_totalFrames.text = totalFrames.ToString();
			_previousTotalFrames = totalFrames.ToString();
		});

		_videoPlaybackViewModel.CurrentTime.Subscribe(currentTime => _currentTime.text = $"{currentTime:00:00:00}");
		_videoPlaybackViewModel.DurationSeconds.Subscribe(durationSeconds => _duration.text = $"{durationSeconds:00:00:00}");
	}

	private void RefreshUI() {
		_videoPlaybackViewModel.RefreshData();
	}

}