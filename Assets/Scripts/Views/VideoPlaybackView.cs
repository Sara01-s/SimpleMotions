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
	[SerializeField] private Image _loopImage;
	[SerializeField] private Sprite _loopOnSprite;
	[SerializeField] private Sprite _loopOffSprite;
	[SerializeField] private Color _loopOnColor;
	[SerializeField] private Color _loopOffColor;

	[SerializeField] private GameObject _play;
	[SerializeField] private GameObject _pause;

	[SerializeField] private TimelineView _timelineView;

	private IVideoPlaybackViewModel _videoPlaybackViewModel;
	private IInputValidator _inputValidator;

	public void Configure(IVideoPlaybackViewModel videoPlaybackViewModel, IInputValidator inputValidator) {
		_videoPlaybackViewModel = videoPlaybackViewModel;
		_inputValidator = inputValidator;

		InitReactiveCommands();
		InitReactiveValues();
		RefreshUI();

		_currentTime.text = "00:00:00";
	}

	private void InitReactiveCommands() {
		_firstFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnFirstFrame.Execute());
		_backward.onClick.AddListener(() => _videoPlaybackViewModel.OnBackwardFrame.Execute());
		_togglePlay.onClick.AddListener(() => _videoPlaybackViewModel.OnTogglePlay.Execute());
		_forward.onClick.AddListener(() => _videoPlaybackViewModel.OnForwardFrame.Execute());
		_lastFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnLastFrame.Execute());

		_loopToggle.onValueChanged.AddListener(isLooping => {
			_videoPlaybackViewModel.IsLooping.Execute(isLooping);

			if (isLooping) {
				_loopImage.sprite = _loopOnSprite;
				_loopImage.color = _loopOnColor;
			}
			else {
				_loopImage.sprite = _loopOffSprite;
				_loopImage.color = _loopOffColor;
			}
		});

		_currentFrame.onValueChanged.AddListener(currentFrame => {
			currentFrame = _inputValidator.ValidateInput(currentFrame);
			int.TryParse(currentFrame, out var newFrame);

			if (newFrame > _videoPlaybackViewModel.TotalFrames.Value) {
				newFrame = _videoPlaybackViewModel.TotalFrames.Value;
			}

			_videoPlaybackViewModel.OnSetCurrentFrame.Execute(newFrame);
		});

		_totalFrames.onValueChanged.AddListener(totalFrames => {
			totalFrames = _inputValidator.ValidateInput(totalFrames);
			int.TryParse(totalFrames, out var newFrame);

			if (newFrame >= 10) {
				_videoPlaybackViewModel.OnSetTotalFrames.Execute(newFrame);
				_timelineView.RefreshUI();
			}
		});
	}

	private void InitReactiveValues() {
		_videoPlaybackViewModel.IsPlaying.Subscribe(isPlaying => {
			_play.SetActive(!isPlaying);
			_pause.SetActive(isPlaying);
		});

		_videoPlaybackViewModel.CurrentFrame.Subscribe(currentFrame => {
			_currentFrame.text = currentFrame.ToString();
		});

		_videoPlaybackViewModel.TotalFrames.Subscribe(totalFrames => {
			_totalFrames.text = totalFrames.ToString();
		});

		_videoPlaybackViewModel.CurrentTime.Subscribe(currentTime => _currentTime.text = FormatTime(currentTime));
		_videoPlaybackViewModel.DurationSeconds.Subscribe(durationSeconds => _duration.text = FormatTime(durationSeconds));

		_videoPlaybackViewModel.OnTargetFramerate.Subscribe(targetFramerate => {
			if (float.TryParse(_totalFrames.text, out var totalFrames)) {
				float durationSeconds = totalFrames / targetFramerate;
				_duration.text = FormatTime(durationSeconds);
			}

			_timelineView.RefreshUI();
		});
	}

	private string FormatTime(float value) {
		int totalSeconds = Mathf.FloorToInt(value);
		int minutes = totalSeconds / 60; 
		int seconds = totalSeconds % 60;
		int milliseconds = Mathf.FloorToInt((value - totalSeconds) * 100);

		return $"{minutes:00}:{seconds:00}:{milliseconds:00}";
	}

	private void RefreshUI() {
		_videoPlaybackViewModel.InitVideoData();
	}

}