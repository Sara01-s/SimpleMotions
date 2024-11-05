using System.Threading.Tasks;

namespace SimpleMotions {

	public interface IVideoPlayer {
		
		void TogglePlay();
		VideoData GetVideoData();
		void Play();
		void Pause();
		void Reset();
		void SetCurrentFrame(int frame);
		void IncreaseFrame();
		void DecreaseFrame();
		int GetFirstFrame();
		int GetLastFrame();

	}

	public class VideoDisplayInfo {
		public int CurrentFrame;
		public float CurrentTime;
		public int TotalFrames;
		public float Duration;
	}

	public sealed class VideoPlayer : IVideoPlayer {

		private readonly VideoData _videoData;
		private readonly TimelineData _timelineData;
		private readonly IVideoAnimator _videoAnimator;
		private readonly IEventService _eventService;

		private Task _playVideo;

		private VideoDisplayInfo _videoDisplayInfo = new();

		public VideoPlayer(VideoData videoData, TimelineData timelineData, IVideoAnimator videoAnimator, IEventService eventService) {
			_videoData = videoData;
			_timelineData = timelineData;
			_videoAnimator = videoAnimator;
			_eventService = eventService;

			// SUS - Ver quien y donde hay que llamar esto.
			_videoData.Duration = _videoData.TotalFrames / _videoData.TargetFrameRate;
			_videoDisplayInfo.TotalFrames = _videoData.TotalFrames;
			_videoDisplayInfo.Duration = _videoData.Duration;
		}

		~VideoPlayer() {
			_playVideo.Dispose();
		}

		public void TogglePlay() {
            if (!_videoData.IsPlaying) {
                Play();
            }
            else {
                Pause();
            }
		}

		public void Play() {
			_videoData.IsPlaying = true;
			
			if (_playVideo == null || _playVideo.IsCompleted) {
				_playVideo = PlayVideo();
			}
		}


		public void Pause() {
			UnityEngine.Debug.Log("Paused.");
			_videoData.IsPlaying = false;
		}

		public void Reset() {
			_videoData.CurrentFrame = _timelineData.FirstKeyframe;
			_videoData.CurrentTime = 0.0f;
		}

		public void SetCurrentFrame(int frame) {
			// TODO - Ver caso en el que está reproduciéndose el video y se llama esta función.
			_videoAnimator.GenerateVideoCache();
			_videoAnimator.InterpolateAllEntities(frame);


			// ---- TOTALMENTE SUS ----
			_videoData.CurrentFrame = frame;
			// ---- TOTALMENTE SUS ----

			_videoDisplayInfo.CurrentFrame = _videoData.CurrentFrame;
			_eventService.Dispatch(_videoDisplayInfo);
		}

		public VideoData GetVideoData() {
			return _videoData;
		}

		private bool IsAtTheEnd() {
			return !_videoData.IsLooping && _videoData.CurrentFrame == _videoData.TotalFrames;
		}

		private async Task PlayVideo() {
			if (IsAtTheEnd()) {
				Reset();
			}

			UnityEngine.Debug.Log("Playing.");

			_videoAnimator.GenerateVideoCache();

			while (_videoData.IsPlaying) {
				_videoAnimator.InterpolateAllEntities(_videoData.CurrentFrame);
				_videoData.CurrentTime += 1.0f / _videoData.TargetFrameRate;

				_videoDisplayInfo.CurrentTime = _videoData.CurrentTime;
				_eventService.Dispatch(_videoDisplayInfo);

				IncreaseFrame();

				//UnityEngine.Debug.Log($"f: {_videoData.CurrentFrame} | t: {_videoData.CurrentTime:0.00}");

				if (_videoData.CurrentFrame >= _videoData.TotalFrames) {
					if (!_videoData.IsLooping) {
						Pause();
						break;
					}

					Reset();
				}

				await Task.Yield();
			}
		}

		public void IncreaseFrame() {
			_videoData.CurrentFrame++;

			_videoDisplayInfo.CurrentFrame = _videoData.CurrentFrame;
			_eventService.Dispatch(_videoDisplayInfo);
		}

		public void DecreaseFrame() {
			_videoData.CurrentFrame--;

			_videoDisplayInfo.CurrentFrame = _videoData.CurrentFrame;
			_eventService.Dispatch(_videoDisplayInfo);
		}

        public int GetFirstFrame() {
			return _timelineData.FirstKeyframe;
        }

        public int GetLastFrame() {
			return _videoData.TotalFrames;
        }

    }
}