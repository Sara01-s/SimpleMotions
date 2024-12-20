using static SimpleMotions.SmMath;
using System.Threading.Tasks;
using SimpleMotions.Internal;

namespace SimpleMotions {

	public interface IVideoPlayer {
		
		int FirstFrame { get; }
		int LastFrame { get; }

		void Play();
		void Pause();
		void TogglePlay();
		void Reset();
		void SetCurrentFrame(int frame);
		void IncreaseFrame();
		void DecreaseFrame();
		VideoData GetVideoData();

	}

	public interface IVideoPlayerData {
		int FirstFrame { get; }
		int LastFrame { get; }
		
		ReactiveValue<bool> IsPlaying { get; }
		ReactiveValue<float> CurrentTime { get; }
		ReactiveValue<float> DurationSeconds { get; }
		ReactiveValue<int> CurrentFrame { get; }
		ReactiveValue<int> TotalFrames { get; }
		ReactiveValue<bool> IsLooping { get ; }
		ReactiveValue<int> TargetFrameRate { get; }

		void InitReactiveData();
	}

	public sealed class VideoPlayer : IVideoPlayer, IVideoPlayerData {

		public int FirstFrame => TimelineData.FIRST_FRAME;
		public int LastFrame => TotalFrames.Value;

		public ReactiveValue<bool> IsPlaying { get; } = new();
		public ReactiveValue<bool> IsLooping { get; } = new();
        public ReactiveValue<int> TotalFrames { get; } = new();
        public ReactiveValue<int> CurrentFrame { get; } = new();
        public ReactiveValue<float> CurrentTime { get; } = new();
		public ReactiveValue<int> TargetFrameRate { get; } = new();
        public ReactiveValue<float> DurationSeconds { get; } = new();

		private readonly IVideoAnimator _videoAnimator;
		private readonly VideoData _videoData;

		private Task _playVideo;

        public VideoPlayer(IVideoAnimator videoAnimator, VideoData videoData) {
			IsPlaying.Subscribe(isPlaying => _videoData.IsPlaying = isPlaying);
			IsLooping.Subscribe(IsLooping => _videoData.IsLooping = IsLooping);
			CurrentTime.Subscribe(currentTime => _videoData.CurrentTime = currentTime);
			CurrentFrame.Subscribe(currentFrame => _videoData.CurrentFrame = currentFrame);
			TargetFrameRate.Subscribe(framerate => _videoData.TargetFrameRate = framerate);

			TotalFrames.Subscribe(totalFrames =>  {
				_videoData.TotalFrames = totalFrames;
				SetDurationSeconds(totalFrames);
			});

			_videoAnimator = videoAnimator;
			_videoData = videoData;
		}

		public void InitReactiveData() {
			IsPlaying.Value = _videoData.IsPlaying;
			CurrentTime.Value = _videoData.CurrentTime;
			DurationSeconds.Value = _videoData.DurationSeconds;
			CurrentFrame.Value = _videoData.CurrentFrame;
			TotalFrames.Value = _videoData.TotalFrames;
			IsLooping.Value = _videoData.IsLooping;

			TargetFrameRate.Value = _videoData.TargetFrameRate;
		}

		~VideoPlayer() {
			_playVideo.Dispose();
		}

		public void TogglePlay() {
            if (!IsPlaying.Value) {
                Play();
            }
            else {
                Pause();
            }
		}

		public void Play() {
			IsPlaying.Value = true;
			
			if (_playVideo == null || _playVideo.IsCompleted) {
				_playVideo = PlayVideo();
			}
		}

		public void Pause() {
			IsPlaying.Value = false;
		}

		public void Reset() {
			CurrentFrame.Value = TimelineData.FIRST_FRAME;
			CurrentTime.Value = 0.0f;
		}

		public void SetCurrentFrame(int frame) {
			CurrentFrame.Value = frame;
			SetCurrentTime(frame);

			_videoAnimator.GenerateVideoCache();
			_videoAnimator.InterpolateAllEntities(frame);
		}

		public void SetCurrentTime(int frame) {
			CurrentTime.Value = (float)frame / TargetFrameRate.Value;
		}

		private void SetDurationSeconds(int seconds) {
			if (TotalFrames.Value > 0 && TargetFrameRate.Value > 0) {
				DurationSeconds.Value = (float)TotalFrames.Value / TargetFrameRate.Value;
			}
		}

		public VideoData GetVideoData() {
			return new VideoData {
				IsPlaying = IsPlaying.Value,
				CurrentFrame = CurrentFrame.Value,
				CurrentTime = CurrentTime.Value,
				IsLooping = IsLooping.Value,
				TotalFrames = TotalFrames.Value,
			};
		}

		private bool IsAtTheEnd() {
			return !IsLooping.Value && CurrentFrame.Value == TotalFrames.Value;
		}

		private async Task PlayVideo() {
			if (IsAtTheEnd()) {
				Reset();
			}

			_videoAnimator.GenerateVideoCache();

			while (IsPlaying.Value) {
				_videoAnimator.InterpolateAllEntities(CurrentFrame.Value);
				CurrentTime.Value += 1.0f / TargetFrameRate.Value;

				IncreaseFrame();

				if (CurrentFrame.Value >= TotalFrames.Value) {
					if (!IsLooping.Value) {
						Pause();
						break;
					}

					Reset();
				}

				await Task.Yield();
			}
		}

		public void IncreaseFrame() {
			CurrentFrame.Value = min(CurrentFrame.Value + 1, TotalFrames.Value);
		}

		public void DecreaseFrame() {
			CurrentFrame.Value = max(CurrentFrame.Value - 1, TimelineData.FIRST_FRAME);
		}

    }
}