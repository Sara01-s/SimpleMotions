namespace SimpleMotions {

	public sealed class Keyframe<T> : System.IComparable {

		public int EntityId;
		public float Time;
		public T Value;

		public Keyframe(int entityId, float time, T value) {
			EntityId = entityId;
			Time = time;
			Value = value;
		}

		public int CompareTo(object obj) {
			var keyframe = obj as Keyframe<T>;

			if (keyframe is not null) {
				return Time.CompareTo(keyframe.Time);
			}

			return 1;
		}

		public override string ToString() {
			return $"Keyframe for entity with id: {EntityId}, at time: {Time} and value: {Value}";
		}

	}
}