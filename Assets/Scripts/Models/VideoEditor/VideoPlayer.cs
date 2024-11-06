using static SimpleMotions.SmMath;
using System.Threading.Tasks;
using SimpleMotions.Internal;

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
		public bool IsPlaying;
		public int TotalFrames;
		public float DurationSeconds;
	}

	public sealed class VideoPlayer : IVideoPlayer {

		private readonly VideoData _videoData;
		private readonly IVideoAnimator _videoAnimator;
		private readonly IEventService _eventService;

		private Task _playVideo;

		private readonly VideoDisplayInfo _videoDisplayInfo = new();

		public VideoPlayer(VideoData videoData, IVideoAnimator videoAnimator, IEventService eventService) {
			_videoData = videoData;
			_videoAnimator = videoAnimator;
			_eventService = eventService;

			ConfigureVideoDisplayInfo();
		}

		~VideoPlayer() {
			_playVideo.Dispose();
		}

		private void ConfigureVideoDisplayInfo() {
			_videoDisplayInfo.CurrentFrame = _videoData.CurrentFrame;
			_videoDisplayInfo.CurrentTime = _videoData.CurrentTime;
			_videoDisplayInfo.IsPlaying = _videoData.IsPlaying;
			_videoDisplayInfo.TotalFrames = _videoData.TotalFrames;
			_videoDisplayInfo.DurationSeconds = _videoData.DurationSeconds;
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

			_videoDisplayInfo.IsPlaying = _videoData.IsPlaying;
			_eventService.Dispatch(_videoDisplayInfo);
		}


		public void Pause() {
			UnityEngine.Debug.Log("Paused.");
			_videoData.IsPlaying = false;

			_videoDisplayInfo.IsPlaying = _videoData.IsPlaying;
			_eventService.Dispatch(_videoDisplayInfo);
		}

		public void Reset() {
			UnityEngine.Debug.Log("Reset");
			_videoData.CurrentFrame = TimelineData.FIRST_FRAME;
			_videoData.CurrentTime = 0.0f;
		}

		public void SetCurrentFrame(int frame) {
			// ---- TOTALMENTE SUS ---- POR ALGUNA RAZÓN QUE DESCONOZCO, 
			_videoData.CurrentFrame = frame;
			// ---- TOTALMENTE SUS ---- ESTA LINEA HACE QUE SE EJECUTE DOS VECES ESTE METODO (ME DEMORE MUCHO EN CACHARLO XD)

			// TODO - Ver caso en el que está reproduciéndose el video y se llama esta función.
			_videoAnimator.GenerateVideoCache();
			_videoAnimator.InterpolateAllEntities(frame);

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
			_videoData.CurrentFrame = min(_videoData.CurrentFrame + 1, _videoData.TotalFrames);

			_videoDisplayInfo.CurrentFrame = _videoData.CurrentFrame;
			_eventService.Dispatch(_videoDisplayInfo);
		}

		public void DecreaseFrame() {
			_videoData.CurrentFrame = max(_videoData.CurrentFrame - 1, TimelineData.FIRST_FRAME);

			_videoDisplayInfo.CurrentFrame = _videoData.CurrentFrame;
			_eventService.Dispatch(_videoDisplayInfo);
		}

        public int GetFirstFrame() {
			return TimelineData.FIRST_FRAME;
        }

        public int GetLastFrame() {
			return _videoData.TotalFrames;
        }

    }
}