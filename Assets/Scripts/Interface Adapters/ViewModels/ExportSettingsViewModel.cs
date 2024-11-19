
namespace SimpleMotions {

    public interface IExportSettingsViewModel {
        ReactiveValue<int> Framerate { get; }

        ReactiveCommand<int> OnSetFramerate { get; }
        ReactiveCommand<string> OnSetOutputFilePath { get; }

        ReactiveCommand OnExport { get; }
        ReactiveCommand OnPresent { get; }
    }

    public class ExportSettingsViewModel : IExportSettingsViewModel {

        public ReactiveValue<int> Framerate { get; private set; } = new();

        public ReactiveCommand<int> OnSetFramerate { get; private set; } = new();
        public ReactiveCommand<string> OnSetOutputFilePath { get; } = new();
        
        public ReactiveCommand OnExport { get; } = new();
        public ReactiveCommand OnPresent { get; } = new();

        private IExportModel _exportModel;

        public ExportSettingsViewModel(IExportModel exportModel) {
            _exportModel = exportModel;

            // ¿Igualar asi todas las variables reactivas en el constructor?
            Framerate.Value = _exportModel.TargetFrameRate.Value;   

            _exportModel.TargetFrameRate.Subscribe(framerate => Framerate.Value = framerate);

            OnSetFramerate.Subscribe(UpdateFramerate);
            OnSetOutputFilePath.Subscribe(UpdateOutputFilePath);
            OnExport.Subscribe(Export);
            OnPresent.Subscribe(Present);
        }

        private void UpdateFramerate(int frameRate) {
            _exportModel.TargetFrameRate.Value = frameRate;
        }

        private void UpdateOutputFilePath(string outputFilePath) {
            _exportModel.OutputFilePath.Value = outputFilePath;
        }

        private void Export() {
            _exportModel.Export.Execute();
        }

        private void Present() {
            _exportModel.Present.Execute();
        }

    }
}