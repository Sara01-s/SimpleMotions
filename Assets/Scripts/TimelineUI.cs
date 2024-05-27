using UnityEngine.UI;
using UnityEngine;

public class TimelineUI : MonoBehaviour {

	[SerializeField] private VideoData _videoData;
	[SerializeField] private Button _playButton;
	[SerializeField] private Slider _timeSlider;

	private float _totalPausedTime;
	private float _lastPauseTime;

	private void Awake() {
		_playButton.onClick.AddListener(TogglePlay);

		_timeSlider.onValueChanged.AddListener(GoTotime);
		_timeSlider.minValue = 0.0f;
		_timeSlider.maxValue = _videoData.DurationSec;
	}

	private void Update() {
		if (_videoData.IsPlaying) {
			_timeSlider.value = (Time.time - _totalPausedTime) % _videoData.DurationSec;
		}
	}

	private void GoTotime(float sliderValue) {
		_videoData.CurrentTime = sliderValue;
		_videoData.CurrentTimeNormalized = sliderValue / _videoData.DurationSec;

		_videoData.OnCurrentTimeChanged?.Invoke(_videoData.CurrentTime);
		_videoData.OnCurrentTimeNormalizedChanged?.Invoke(_videoData.CurrentTimeNormalized);
	}

	private void TogglePlay() {
		if (_videoData.IsPlaying) {
			_lastPauseTime = Time.time;
			_videoData.IsPlaying = false;
		}
		else {
			_totalPausedTime += Time.time - _lastPauseTime;
			_videoData.IsPlaying = true;
		}

		_timeSlider.interactable = !_videoData.IsPlaying;
	}
}
