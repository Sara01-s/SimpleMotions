using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

    public interface IEditKeyframeViewModel {
        ReactiveValue<(KeyframeDTO keyframeDTO, int targetFrame)> NewKeyframeFrame { get; }
        ReactiveValue<(KeyframeDTO keyframeDTO, int targetEase)> NewKeyframeEase { get; }
    }

    public class EditKeyframeViewModel : IEditKeyframeViewModel {

        public ReactiveValue<(KeyframeDTO keyframeDTO, int targetFrame)> NewKeyframeFrame { get; } = new();
        public ReactiveValue<(KeyframeDTO keyframeDTO, int targetEase)> NewKeyframeEase { get; } = new();

        public KeyframeStorage _keyframeStorage;

        public EditKeyframeViewModel(IKeyframeStorage keyframeStorage) {
            NewKeyframeFrame.Subscribe(values => {
                var keyframes = keyframeStorage.GetEntityKeyframesAtFrame(values.keyframeDTO.Id, values.targetFrame);

				// TODO - EDIT KEYFRAME
            });

            NewKeyframeEase.Subscribe(values => {
                var transformKeyframe = keyframeStorage.GetEntityKeyframeOfType<Transform>(values.keyframeDTO.Id, values.targetEase);

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