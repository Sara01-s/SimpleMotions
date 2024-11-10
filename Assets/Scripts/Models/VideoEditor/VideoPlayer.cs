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

	public interface IVideoPlayerData {
		ReactiveValue<bool> IsPlaying { get; }
		ReactiveValue<int> CurrentFrame { get; }
		ReactiveValue<int> TotalFrames { get; }
		ReactiveValue<float> CurrentTime { get; }
		ReactiveValue<float> DurationSeconds { get; }
	}

	public sealed class VideoPlayer : IVideoPlayer, IVideoPlayerData {

		private readonly VideoData _videoData;
		private readonly IVideoAnimator _videoAnimator;

		private Task _playVideo;

		public ReactiveValue<bool> IsPlaying { get; } = new();
        public ReactiveValue<int> CurrentFrame { get; } = new();
        public ReactiveValue<int> TotalFrames { get; } = new();
        public ReactiveValue<float> CurrentTime { get; } = new();
        public ReactiveValue<float> DurationSeconds { get; } = new();

        public VideoPlayer(VideoData videoData, IVideoAnimator videoAnimator) {
			_videoData = videoData;
			_videoAnimator = videoAnimator;
		}

		~VideoPlayer() {
			_playVideo.Dispose();
		}

		// TODO - Encontrar lugar para hacer esto.
		private void ENCONTRAR_LUGAR_PARA_HACER_ESTO() {
			IsPlaying.Value = _videoData.IsPlaying;
			CurrentFrame.Value = _videoData.CurrentFrame;
			TotalFrames.Value = _videoData.TotalFrames;
			CurrentTime.Value = _videoData.CurrentTime;
			DurationSeconds.Value = _videoData.DurationSeconds;
		}

		public void TogglePlay() {
            if (!_videoData.IsPlaying) {
                Play();
            }
            else {
                Pause();
            }

			// TEMPORAL
			ENCONTRAR_LUGAR_PARA_HACER_ESTO();
		}

		public void Play() {
			_videoData.IsPlaying = true;
			
			if (_playVideo == null || _playVideo.IsCompleted) {
				_playVideo = PlayVideo();
			}

			IsPlaying.Value = _videoData.IsPlaying;
		}


		public void Pause() {
			UnityEngine.Debug.Log("Paused.");
			_videoData.IsPlaying = false;

			IsPlaying.Value = _videoData.IsPlaying;
		}

		public void Reset() {
			UnityEngine.Debug.Log("Reset");
			_videoData.CurrentFrame = TimelineData.FIRST_FRAME;
			_videoData.CurrentTime = 0.0f;

			CurrentFrame.Value = _videoData.CurrentFrame;
			CurrentTime.Value = _videoData.CurrentTime;
		}

		public void SetCurrentFrame(int frame) {
			// ---- TOTALMENTE SUS ---- POR ALGUNA RAZÓN QUE DESCONOZCO, 
			_videoData.CurrentFrame = frame;
			CurrentFrame.Value = _videoData.CurrentFrame;
			// ---- TOTALMENTE SUS ---- ESTA LINEA HACE QUE SE EJECUTE DOS VECES ESTE METODO (ME DEMORE MUCHO EN CACHARLO XD)

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

				CurrentTime.Value = _videoData.CurrentTime;

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

			CurrentFrame.Value = _videoData.CurrentFrame;
		}

		public void DecreaseFrame() {
			_videoData.CurrentFrame = max(_videoData.CurrentFrame - 1, TimelineData.FIRST_FRAME);

			CurrentFrame.Value = _videoData.CurrentFrame;
		}

        public int GetFirstFrame() {
			return TimelineData.FIRST_FRAME;
        }

        public int GetLastFrame() {
			return _videoData.TotalFrames;
        }

    }
}