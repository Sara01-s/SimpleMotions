using System.Threading.Tasks;

namespace SimpleMotions {

	internal sealed class VideoPlayer {

		private readonly IComponentStorage _componentStorage;

		internal VideoPlayer(IComponentStorage componentStorage) {
			_componentStorage = componentStorage;
		}

		internal async void PlayVideo(float currentTime) {
			await Task.Yield();
		}

		internal void AddKeyframe(Keyframe keyframe) {

		}

		internal void RemoveKeyframe(Keyframe keyframe) {

		}

		internal void InterpolateKeyframes(Keyframe first, Keyframe second, float t) {

		}

	}
}