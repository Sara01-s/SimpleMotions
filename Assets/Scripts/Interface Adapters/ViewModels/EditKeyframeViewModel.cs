using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IEditKeyframeViewModel {
        ReactiveValue<(int entityId, int originalEase, int targetEase)> NewKeyframeEase { get; }
        ReactiveValue<(int entityId, int targetFrane, int originalFrame)> NewKeyframeFrame { get; }
    }

    public class EditKeyframeViewModel : IEditKeyframeViewModel {

        public ReactiveValue<(int entityId, int originalEase, int targetEase)> NewKeyframeEase { get; } = new();
        public ReactiveValue<(int entityId, int targetFrane, int originalFrame)> NewKeyframeFrame { get; } = new();

        public KeyframeStorage _keyframeStorage;

        public EditKeyframeViewModel(IKeyframeStorage keyframeStorage) {
            NewKeyframeFrame.Subscribe(keyframe => {
                var transformKeyframe = keyframeStorage.GetEntityKeyframeOfType<Transform>(keyframe.entityId, keyframe.originalFrame);
            });
        }

    }
}