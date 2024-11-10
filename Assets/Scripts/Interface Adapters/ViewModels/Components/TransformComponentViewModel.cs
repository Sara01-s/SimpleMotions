
namespace SimpleMotions {


	public interface ITransformComponentViewModel {

		ReactiveCommand<string> PositionX { get; }
		public ReactiveCommand<string> PositionY { get; }
		public ReactiveCommand<string> ScaleW { get; }
		public ReactiveCommand<string> ScaleH { get; }
		public ReactiveCommand<string> RollAngles { get; }

	}

    public class TransformComponentViewModel : ITransformComponentViewModel {

        public ReactiveCommand<string> PositionX { get; } = new();
		public ReactiveCommand<string> PositionY { get; } = new();
		public ReactiveCommand<string> ScaleW { get; } = new();
		public ReactiveCommand<string> ScaleH { get; } = new();
		public ReactiveCommand<string> RollAngles { get; } = new();

		public TransformComponentViewModel() {

		}

    }
}