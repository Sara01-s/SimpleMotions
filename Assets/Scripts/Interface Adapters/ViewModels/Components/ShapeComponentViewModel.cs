using UnityEngine;

namespace SimpleMotions {

    public interface IShapeComponentViewModel {

        ReactiveCommand<string> Hex { get; }
        ReactiveCommand<string> Alpha { get; }

    }

    public class ShapeComponentViewModel : IShapeComponentViewModel {

        public ReactiveCommand<string> Hex { get; } = new();
        public ReactiveCommand<string> Alpha { get; } = new();


        public ShapeComponentViewModel() {

        }

    }
}
