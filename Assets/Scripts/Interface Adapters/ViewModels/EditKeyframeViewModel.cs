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
				var componentType = ComponentDTOToType(values.originalKeyframeDTO.ComponentDTO);

                if (!Enum.IsDefined(typeof(Ease), values.targetEase)) {
                    throw new ArgumentException($"{values.targetEase} is not defined in enum: {typeof(Ease)}");
                }

				var newEase = (Ease)values.targetEase;
				keyframeStorage.SetKeyframeEase(entityId, componentType, originalFrame, newEase);
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