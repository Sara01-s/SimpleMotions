namespace SimpleMotions {

	public sealed class Keyframe<T> : System.IComparable {

		public int EntityId;
		public float Time;
		public T Value;

		public int CompareTo(object obj) {
			var keyframe = obj as Keyframe<T>;

			if (keyframe is not null) {
				return Time.CompareTo(keyframe.Time);
			}

			return 1;
		}

		public override string ToString() {
			return $"Id: {EntityId}, time: {Time}, value: {Value}";
		}

	}
}