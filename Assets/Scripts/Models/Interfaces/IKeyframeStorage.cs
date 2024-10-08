namespace SimpleMotions {

	public interface IKeyframeStorage {
		
		Keyframe<Transform> GetKeyframeAt(int frame);
		KeyframesData GetKeyframesData();
		void AddKeyframe(Entity entity, int frame, Position value);

	}
}