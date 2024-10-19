namespace SimpleMotions {

	public interface IKeyframeStorage {
		
		Keyframe<T> GetKeyframeAt<T>(int frame) where T : Component;
		KeyframesData GetKeyframesData();
		bool EntityHasKeyframes<T>(int entityId) where T : Component;
		int GetTotalFrames();
		void AddKeyframe<T>(int entityId, int frame, T value) where T : Component;
		IKeyframeSpline<Component> GetAllKeyframesOfType<T>() where T : Component;

	}
}