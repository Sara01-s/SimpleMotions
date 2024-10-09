namespace SimpleMotions {

	public interface IKeyframeStorage {
		
		Keyframe<Transform> GetKeyframeAt(int frame);
		KeyframesData GetKeyframesData();
		int GetTotalFrames();
		void AddKeyframe(Entity entity, int frame, Position value);

	}
}