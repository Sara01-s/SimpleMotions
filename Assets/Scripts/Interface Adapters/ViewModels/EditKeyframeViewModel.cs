using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IEditKeyframeViewModel {
        ReactiveValue<KeyframeDTO> OnKeyframeEdit { get; }

        ReactiveValue<int> EntityKeyframeEase { get; }
        ReactiveValue<int> EntityKeyframeFrame { get; }
    }

    public class EditKeyframeViewModel : IEditKeyframeViewModel {

        public ReactiveValue<KeyframeDTO> OnKeyframeEdit { get; } = new();

        public ReactiveValue<int> EntityKeyframeEase { get; } = new();
        public ReactiveValue<int> EntityKeyframeFrame { get; } = new();

        public KeyframeStorage _keyframeStorage;

        public EditKeyframeViewModel(IKeyframeStorage keyframeStorage) {
            OnKeyframeEdit.Subscribe(keyframeDTO => {
                if (keyframeDTO.ComponentDTO == ComponentDTO.Transform) {
                    var keyframe = keyframeStorage.GetEntityKeyframeOfType<Transform>(keyframeDTO.Id, keyframeDTO.Frame);
                    keyframeStorage.EditKeyframe(keyframe, keyframeDTO.Ease);
                }
                else if (keyframeDTO.ComponentDTO == ComponentDTO.Shape) {
                    var keyframe = keyframeStorage.GetEntityKeyframeOfType<Shape>(keyframeDTO.Id, keyframeDTO.Frame);
                    keyframeStorage.EditKeyframe(keyframe, keyframeDTO.Ease);
                }
                else {
                    var keyframe = keyframeStorage.GetEntityKeyframeOfType<Text>(keyframeDTO.Id, keyframeDTO.Frame);
                    keyframeStorage.EditKeyframe(keyframe, keyframeDTO.Ease);
                }
            });
        }

    }
}