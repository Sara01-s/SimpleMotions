using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

    public interface IEditKeyframeViewModel {
        ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetFrame)> NewKeyframeFrame { get; }
        ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetEase)> NewKeyframeEase { get; }
    }

    public class EditKeyframeViewModel : IEditKeyframeViewModel {

        public ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetFrame)> NewKeyframeFrame { get; } = new();
        public ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetEase)> NewKeyframeEase { get; } = new();

        public KeyframeStorage _keyframeStorage;

        public EditKeyframeViewModel(IKeyframeStorage keyframeStorage) {
            NewKeyframeFrame.Subscribe(values => {
                int entityId = values.originalKeyframeDTO.EntityId;
                int originalFrame = values.originalKeyframeDTO.Frame;
                var keyframe = keyframeStorage.GetEntityKeyframeOfType<Transform>(entityId, originalFrame);
                
                keyframeStorage.SetKeyframeFrame(keyframe, values.targetFrame);
            });

            NewKeyframeEase.Subscribe(values => {
                var transformKeyframe = keyframeStorage.GetEntityKeyframeOfType<Transform>(values.originalKeyframeDTO.EntityId, values.targetEase);

                if (Enum.IsDefined(typeof(Ease), values.targetEase)) {
                    keyframeStorage.SetKeyframeEase(transformKeyframe, (Ease)values.targetEase);
                }
                else {
                    throw new ArgumentException($"{values.targetEase} is not defined in enum: {typeof(Ease)}");
                }
            });
        }

    }
}