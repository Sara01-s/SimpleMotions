
namespace SimpleMotions {

    public interface IExportSettingsViewModel {
        ReactiveValue<int> Framerate { get; }
        ReactiveValue<string> InvalidFilePath { get; }

        ReactiveCommand<int> OnSetFramerate { get; }
        ReactiveCommand<string> OnSetOutputFilePath { get; }
        ReactiveCommand<string> OnSetFileName { get; }

        ReactiveCommand OnExport { get; }
    }

    public class ExportSettingsViewModel : IExportSettingsViewModel {

        public ReactiveValue<int> Framerate { get; private set; } = new();
        public ReactiveValue<string> InvalidFilePath { get; private set; } = new();

        public ReactiveCommand<int> OnSetFramerate { get; private set; } = new();
        public ReactiveCommand<string> OnSetOutputFilePath { get; } = new();
        public ReactiveCommand<string> OnSetFileName { get; } = new();
        
        public ReactiveCommand OnExport { get; } = new();
        public ReactiveCommand OnPresent { get; } = new();
        
        private IExportModel _exportModel;


        public ExportSettingsViewModel(IExportModel exportModel) {
            // ¿Igualar asi todas las variables reactivas en el constructor?
            Framerate.Value = exportModel.TargetFrameRate.Value;   

            OnSetFramerate.Subscribe(SetFramerate);
            OnSetOutputFilePath.Subscribe(SetOutputFilePath);
            OnSetFileName.Subscribe(SetFileName);
            OnExport.Subscribe(() => _exportModel.Export.Execute());
            
            exportModel.TargetFrameRate.Subscribe(framerate => Framerate.Value = framerate);
            exportModel.OnFilePathInvalid.Subscribe(filePath => InvalidFilePath.Value = filePath);

            _exportModel = exportModel;
        }

        private void SetFramerate(int frameRate) {
            _exportModel.TargetFrameRate.Value = frameRate;
        }

        private void SetOutputFilePath(string outputFilePath) {
            _exportModel.OutputFilePath.Value = outputFilePath;
        }

        private void SetFileName(string fileName) {
            _exportModel.FileName.Value = fileName;
        }

    }
}