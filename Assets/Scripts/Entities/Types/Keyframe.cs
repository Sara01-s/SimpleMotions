namespace SimpleMotions {

	public interface IKeyframe<out T> where T : Component {
		int EntityId { get; set; }
		int Frame { get; set; }
		T Value { get; }
	}

	[System.Serializable]
	public class Keyframe<T> : IKeyframe<T>, System.IComparable where T : Component, new() {

		public static Keyframe<T> Invalid = new(Entity.INVALID_ID, TimelineData.INVALID_FRAME, new());
		
		public int EntityId { get; set; }
		public int Frame { get; set; }
		public T Value { get; set; }

		public Keyframe(int entityId) {
			EntityId = entityId;
			Frame = TimelineData.FIRST_KEYFRAME;
			Value = new T();
		}

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