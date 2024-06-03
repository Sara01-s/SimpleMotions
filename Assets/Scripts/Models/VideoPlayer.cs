using System.Threading.Tasks;

namespace SimpleMotions {

	internal sealed class VideoPlayer {

		private readonly IComponentDatabase _componentDatabase;

		internal VideoPlayer(IComponentDatabase componentDatabase) {
			_componentDatabase = componentDatabase;
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