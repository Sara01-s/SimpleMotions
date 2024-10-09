namespace SimpleMotions {

	public sealed class Keyframe<T> : System.IComparable {

		public static Keyframe<T> Empty = new(-1, -1, default); 

		public int EntityId;
		public int Frame;
		public T Value;

		public Keyframe(int entityId, int frame, T value) {
			EntityId = entityId;
			Frame = frame;
			Value = value;
		}

		public int CompareTo(object obj) {
			var keyframe = obj as Keyframe<T>;

			if (keyframe is not null) {
				return Frame.CompareTo(keyframe.Frame);
			}

			return 1;
		}

		public override string ToString() {
			return $"Keyframe (entity id: {EntityId}, frame: {Frame} value: {Value}";
		}

	}
}