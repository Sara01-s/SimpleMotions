
namespace SimpleMotions {

    public interface ITextComponentViewModel {

        ReactiveCommand<string> Text { get; }

    }

    public class TextComponentViewModel : ITextComponentViewModel {

        public ReactiveCommand<string> Text { get; } = new();

        public TextComponentViewModel() {
            
        }

    }
}