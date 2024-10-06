namespace SimpleMotions {

	public interface IKeyframeStorage {

		KeyframesData GetKeyframesData();
		void AddKeyframe(Entity entity, float time, Position value);

	}
}