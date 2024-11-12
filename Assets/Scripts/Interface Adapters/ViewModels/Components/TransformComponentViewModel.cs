using SimpleMotions.Internal;

namespace SimpleMotions {


	public interface ITransformComponentViewModel {

		ReactiveCommand<string> PositionX { get; }
		ReactiveCommand<string> PositionY { get; }
		ReactiveCommand<string> ScaleW { get; }
		ReactiveCommand<string> ScaleH { get; }
		ReactiveCommand<string> Roll { get; }

	}

    public class TransformComponentViewModel : ITransformComponentViewModel {

		public ReactiveCommand<string> PositionX { get; } = new();
		public ReactiveCommand<string> PositionY { get; } = new();
		public ReactiveCommand<string> ScaleW { get; } = new();
		public ReactiveCommand<string> ScaleH { get; } = new();
		public ReactiveCommand<string> Roll { get; } = new();

		private readonly System.Func<Transform> _getSelectedEntityTransform; 

		public TransformComponentViewModel(IComponentStorage componentStorage, IEntitySelector entitySelector) {
			_getSelectedEntityTransform = () => {
				int selectedEntityId = entitySelector.SelectedEntity.Id;
				return componentStorage.GetComponent<Transform>(selectedEntityId);
			};

			PositionX.Subscribe(ModifyEntityPositionX);
			PositionY.Subscribe(ModifyEntityPositionY);
			ScaleW.Subscribe(ModifyEntityScaleWidth);
			ScaleH.Subscribe(ModifyEntityScaleHeight);
			Roll.Subscribe(ModifyEntityRollAngleDegrees);
		}

		private void ModifyEntityPositionX(string positionX) {
			if (float.TryParse(positionX, out float x)) {
				_getSelectedEntityTransform().Position.X = x;
			}
		}

		private void ModifyEntityPositionY(string positionY) {
			if (float.TryParse(positionY, out float y)) {
				_getSelectedEntityTransform().Position.Y = y;
			}
		}

		private void ModifyEntityScaleWidth(string scaleWidth) {
			if (float.TryParse(scaleWidth, out float w)) {
				_getSelectedEntityTransform().Scale.Width = w;
			}
		}

		private void ModifyEntityScaleHeight(string scaleHeight) {
			if (float.TryParse(scaleHeight, out float h)) {
				_getSelectedEntityTransform().Scale.Height = h;
			}
		}

		private void ModifyEntityRollAngleDegrees(string angleDegrees) {
			if (float.TryParse(angleDegrees, out float angle)) {
				_getSelectedEntityTransform().Roll.AngleDegrees = angle;
			}
		}

    }
}