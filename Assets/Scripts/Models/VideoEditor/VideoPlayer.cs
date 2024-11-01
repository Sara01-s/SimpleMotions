using System.Threading.Tasks;

namespace SimpleMotions {

	public sealed class VideoPlayer : IVideoPlayer {

		private readonly VideoData _videoData;
		private readonly IVideoAnimator _videoAnimator;

		private Task _playVideo;

		public VideoPlayer(VideoData videoData, IVideoAnimator videoAnimator) {
			_videoData = videoData;
			_videoAnimator = videoAnimator;
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

		public void SetCurrentTime(float seconds) {
			_videoData.CurrentTime = seconds;
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
				_videoData.CurrentFrame++;

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

	}
}