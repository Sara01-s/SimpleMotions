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

	[SerializeField] private Toggle _toggleLoop;
	[SerializeField] private Image _loopImage;

	[SerializeField] private Image _togglePlayStopImage;
	[SerializeField] private Sprite _play;
	[SerializeField] private Sprite _stop;

	[SerializeField] private TimelineView _timelineView;
	[SerializeField] private EditorPainter _editorPainter;

	private IVideoPlaybackViewModel _videoPlaybackViewModel;
	private IInputValidator _inputValidator;

	private bool _isLoopOn;

	private void OnDisable() {
		_editorPainter.OnAccentColorUpdate -= UpdateLoopColor;
	}

	public void Configure(IVideoPlaybackViewModel videoPlaybackViewModel, IInputValidator inputValidator) {
		_videoPlaybackViewModel = videoPlaybackViewModel;
		_inputValidator = inputValidator;

		_editorPainter.OnAccentColorUpdate += UpdateLoopColor;

		InitReactiveCommands();
		InitReactiveValues();
		RefreshUI();

		_currentTime.text = "00:00:00";
		_loopImage.color = _editorPainter.Theme.AccentColor;
	}

	private void InitReactiveCommands() {
		_firstFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnFirstFrame.Execute());
		_backward.onClick.AddListener(() => _videoPlaybackViewModel.OnBackwardFrame.Execute());
		_togglePlay.onClick.AddListener(() => _videoPlaybackViewModel.OnTogglePlay.Execute());
		_forward.onClick.AddListener(() => _videoPlaybackViewModel.OnForwardFrame.Execute());
		_lastFrame.onClick.AddListener(() => _videoPlaybackViewModel.OnLastFrame.Execute());

		_toggleLoop.onValueChanged.AddListener(isLooping => {
			_videoPlaybackViewModel.OnSetLoop.Execute(isLooping);
		});

		_currentFrame.onValueChanged.AddListener(currentFrame => {
			currentFrame = _inputValidator.ValidateComponentInput(currentFrame);
			int.TryParse(currentFrame, out var newFrame);

			if (newFrame > _videoPlaybackViewModel.TotalFrames.Value) {
				newFrame = _videoPlaybackViewModel.TotalFrames.Value;
			}

			_videoPlaybackViewModel.OnSetCurrentFrame.Execute(newFrame);
		});

		_totalFrames.onValueChanged.AddListener(totalFrames => {
			totalFrames = _inputValidator.ValidateComponentInput(totalFrames);
			int.TryParse(totalFrames, out var newFrame);

			if (newFrame >= 10) {
				_videoPlaybackViewModel.OnSetTotalFrames.Execute(newFrame);
				_timelineView.RefreshUI();
			}
		});
	}

	private void InitReactiveValues() {
		_videoPlaybackViewModel.IsPlaying.Subscribe(isPlaying => {
			if (isPlaying) {
				_togglePlayStopImage.sprite = _stop;
			}
			else {
				_togglePlayStopImage.sprite = _play;
			}
		});

		_videoPlaybackViewModel.IsLooping.Subscribe(isLooping => {
			_toggleLoop.isOn = isLooping;

			if (isLooping) {
				_isLoopOn = true;
				_loopImage.color = _editorPainter.CurrentAccentColor;
			}
			else {
				_isLoopOn = false;
				_loopImage.color = Color.white;
			}
		});

		_videoPlaybackViewModel.CurrentFrame.Subscribe(currentFrame => {
			_currentFrame.text = currentFrame.ToString();
		});

		_videoPlaybackViewModel.TotalFrames.Subscribe(totalFrames => {
			_totalFrames.text = totalFrames.ToString();
		});

		_videoPlaybackViewModel.CurrentTime.Subscribe(currentTime => _currentTime.text = FormatTime(currentTime));
		_videoPlaybackViewModel.DurationSeconds.Subscribe(durationSeconds => _duration.text = FormatTime(durationSeconds));

		_videoPlaybackViewModel.TargetFramerate.Subscribe(targetFramerate => {
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

	private void UpdateLoopColor(Color color) {
		if (_isLoopOn) {
			_loopImage.color = color;
		}
	}

	private void RefreshUI() {
		_videoPlaybackViewModel.InitVideoData();
	}

}