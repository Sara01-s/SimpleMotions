using System.Collections.Generic;

namespace SimpleMotions {

	public interface IKeyframeStorage {

#nullable enable
		IKeyframeSpline<Component>? GetKeyframeSplineOfType<T>() where T : Component;
#nullable disable

		IKeyframe<Component> GetKeyframeAt<T>(int frame) where T : Component;
		IKeyframe<Component> GetKeyframeAt(int frame);
		KeyframesData GetKeyframesData();
		void AddKeyframe<T>(int entityId, IKeyframe<T> keyframe) where T : Component;
		IKeyframe<Component> AddKeyframe<T>(int entityId, int frame, T value) where T : Component;
		IList<IKeyframe<Component>> GetEntityKeyframes(int entityId);
		IList<IKeyframe<Component>> GetEntityKeyframesOfType<T>(int entityId) where T : Component;

		IEnumerable<System.Type> GetKeyframeTypes();

		bool EntityHasKeyframesOfType<T>(int entityId) where T : Component;
		int GetTotalFrames();
		bool TryGetAllKeyframesOfType<T>(out IKeyframeSpline<Component> keyframeSpline) where T : Component;
		bool EntityHasKeyframesOfAnyType(int entityId);
		IEnumerable<IKeyframe<Component>> GetAllKeyframesAt(int currentFrame);
	}
}