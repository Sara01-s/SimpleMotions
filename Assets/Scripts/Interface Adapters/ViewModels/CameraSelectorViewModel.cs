
namespace SimpleMotions {

    public interface ICameraSelectorViewModel {
        ReactiveCommand OnExportStarted { get; }
        ReactiveCommand OnExportEnded { get; }
    }

    public class CameraSelectorViewModel : ICameraSelectorViewModel {

        public ReactiveCommand OnExportStarted { get; } = new();
        public ReactiveCommand OnExportEnded { get; } = new();

        public CameraSelectorViewModel(IExportViewModel exportViewModel) {
            exportViewModel.OnExportStart.Subscribe(_ => OnExportStarted.Execute());
            exportViewModel.OnExportEnd.Subscribe(OnExportEnded.Execute);
        }

    }

}
