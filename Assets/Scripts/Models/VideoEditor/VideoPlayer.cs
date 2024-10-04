using System.Threading.Tasks;

namespace SimpleMotions {

	public sealed class VideoPlayer {

		private readonly IComponentStorage _componentStorage;

		public VideoPlayer(IComponentStorage componentStorage) {
			_componentStorage = componentStorage;
		}

		public async void PlayVideo(float currentTime) {
			await Task.Yield();
		}

		public void AddKeyframe(Keyframe keyframe) {

		}

		public void RemoveKeyframe(Keyframe keyframe) {

		}

		public void InterpolateKeyframes(Keyframe first, Keyframe second, float t) {

		}

	}
}