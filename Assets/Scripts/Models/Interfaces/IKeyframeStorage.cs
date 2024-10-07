namespace SimpleMotions {

	public interface IKeyframeStorage {

		KeyframesData GetKeyframesData();
		void AddKeyframe(Entity entity, int frame, Position value);

	}
}