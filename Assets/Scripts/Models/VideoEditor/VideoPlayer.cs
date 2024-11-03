using System.Threading.Tasks;

namespace SimpleMotions {

	public interface IVideoPlayer {
		
		void TogglePlay();
		VideoData GetVideoData();
		void Play();
		void Pause();
		void Reset();
		void SetCurrentFrame(int frame);

	}

	public struct VideoDisplayInfo {
		public int CurrentFrame;
	}

	public sealed class VideoPlayer : IVideoPlayer {

		private readonly VideoData _videoData;
		private readonly IVideoAnimator _videoAnimator;
		private readonly IEventService _eventService;

		private Task _playVideo;

		public VideoPlayer(VideoData videoData, IVideoAnimator videoAnimator, IEventService eventService) {
			_videoData = videoData;
			_videoAnimator = videoAnimator;
			_eventService = eventService;
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
			_videoData.CurrentFrame = TimelineData.FIRST_KEYFRAME;
			_videoData.CurrentTime = 0.0f;
		}

		public void SetCurrentFrame(int frame) {
			// TODO - Ver caso en el que está reproduciéndose el video y se llama esta función.
			_videoAnimator.GenerateVideoCache();
			_videoAnimator.InterpolateAllEntities(frame);
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

		private void IncreaseFrame() {
			_videoData.CurrentFrame++;

			var videoDisplayInfo = new VideoDisplayInfo() {
				CurrentFrame = _videoData.CurrentFrame
			};

			_eventService.Dispatch(videoDisplayInfo);
		}

	}
}