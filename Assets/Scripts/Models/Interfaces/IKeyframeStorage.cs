using System.Collections.Generic;

namespace SimpleMotions {

	public interface IKeyframeStorage {

#nullable enable
		IKeyframeSpline? GetKeyframeSplineOfType<T>() where T : Component;
		IKeyframeSpline? GetEntityKeyframesOfType<T>(int entityId) where T : Component;
#nullable disable

		IKeyframe<Component> GetKeyframeAt<T>(int frame) where T : Component;
		IKeyframe<Component> GetKeyframeAt(int frame);
		KeyframesData GetKeyframesData();
		void AddKeyframe<T>(int entityId, IKeyframe<T> keyframe) where T : Component;
		IKeyframe<Component> AddKeyframe<T>(int entityId, int frame, T value) where T : Component;
		IEnumerable<IKeyframeSpline> GetEntityKeyframes(int entityId);

		IEnumerable<System.Type> GetKeyframeTypes();

		bool FrameHasKeyframe(int frame);
		bool EntityHasKeyframesOfType<T>(int entityId) where T : Component;
		bool TryGetAllKeyframesOfType<T>(out IKeyframeSpline keyframeSpline) where T : Component;
		bool EntityHasKeyframesOfAnyType(int entityId);

		int GetTotalFrames();
		IEnumerable<IKeyframe<Component>> GetAllKeyframesAt(int currentFrame);
	}
}