using SimpleMotions.Internal;
using System;

namespace SimpleMotions {

    public interface IEditKeyframeViewModel {
        ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetFrame)> NewKeyframeFrame { get; }
        ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetEase)> NewKeyframeEase { get; }
		ReactiveCommand<int> UpdateKeyframeFrame { get; }
    }

    public class EditKeyframeViewModel : IEditKeyframeViewModel {

		public ReactiveCommand<int> UpdateKeyframeFrame { get; } = new();
        public ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetFrame)> NewKeyframeFrame { get; } = new();
        public ReactiveValue<(KeyframeDTO originalKeyframeDTO, int targetEase)> NewKeyframeEase { get; } = new();

        public KeyframeStorage _keyframeStorage;

        public EditKeyframeViewModel(IKeyframeStorage keyframeStorage) {
            NewKeyframeFrame.Subscribe(values => {
                int entityId = values.originalKeyframeDTO.EntityId;
                int originalFrame = values.originalKeyframeDTO.Frame;
				var componentType = ComponentDTOToType(values.originalKeyframeDTO.ComponentDTO);

				keyframeStorage.SetKeyframeFrame(entityId, componentType, originalFrame, values.targetFrame);
				UpdateKeyframeFrame.Execute(values.targetFrame);
			});

            NewKeyframeEase.Subscribe(values => {
                int entityId = values.originalKeyframeDTO.EntityId;
                int originalFrame = values.originalKeyframeDTO.Frame;

				IKeyframe<Component> originalKeyframe = values.originalKeyframeDTO.ComponentDTO switch {
					ComponentDTO.Transform => keyframeStorage.GetEntityKeyframeOfType<Transform>(entityId, originalFrame),
					ComponentDTO.Shape => keyframeStorage.GetEntityKeyframeOfType<Shape>(entityId, originalFrame),
					_ => throw new NotImplementedException(),
				};

                if (!Enum.IsDefined(typeof(Ease), values.targetEase)) {
                    throw new ArgumentException($"{values.targetEase} is not defined in enum: {typeof(Ease)}");
                }

				Ease targetEase = (Ease)values.targetEase;

				keyframeStorage.RemoveKeyframeOfType(originalKeyframe.Value.GetType(), entityId, originalKeyframe.Frame);
				keyframeStorage.AddKeyframe(entityId, originalFrame, originalKeyframe.Value, targetEase);
            });
		}

		private Type ComponentDTOToType(ComponentDTO componentDTO) {
			return componentDTO switch {
				ComponentDTO.Transform => typeof(Transform),
				ComponentDTO.Shape => typeof(Shape),
				_ => throw new NotImplementedException(),
			};
		}

	}
}