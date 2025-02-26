
namespace SimpleMotions.Internal {

    public interface IKeyframe<out T> where T : Component, new() {
		Ease Ease { get; set; }
		int EntityId { get; }
        int Frame { get; set ; }
        T Value { get; }
    }

	[System.Serializable]
	public class Keyframe<T> : IKeyframe<T>, System.IComparable where T : Component, new() {

		public static Keyframe<T> Invalid = new(Entity.Invalid.Id, TimelineData.INVALID_FRAME, new());

		public Ease Ease { get; set; } = Ease.Linear;
		public int EntityId { get; }
		public int Frame { get; set; }
		public T Value { get; }

		public Keyframe(IKeyframe<T> keyframe) {
			EntityId = keyframe.EntityId;
			Frame = keyframe.Frame;
			Value = keyframe.Value;
		}

		public Keyframe(int entityId) {
			EntityId = entityId;
			Frame = TimelineData.FIRST_FRAME;
			Value = new T();
		}

		public Keyframe(int entityId, int frame, T value) {
			EntityId = entityId;
			Frame = frame;
			Value = value;
		}

		public Keyframe(int entityId, int frame, T value, Ease ease) {
			Ease = ease;
			EntityId = entityId;
			Frame = frame;
			Value = value;
		}

		public int CompareTo(object obj) {
			var keyframe = obj as Keyframe<T> ?? throw new System.ArgumentException($"Only compare keyframes with another keyframes. " + nameof(obj));

			if (keyframe is not null) {
				return Frame.CompareTo(keyframe.Frame);
			}

			return 1;
		}

		public override string ToString() {
			return $"Keyframe (entity id: {EntityId}, frame: {Frame} value: {Value}, ease: {Ease}";
		}
	}

}