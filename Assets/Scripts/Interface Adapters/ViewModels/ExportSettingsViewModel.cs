
namespace SimpleMotions {

    public interface IExportSettingsViewModel {
        string Framerate { get; }
        ReactiveCommand<int> OnFramerateUpdate { get; }

        ReactiveCommand OnExport { get; }
        ReactiveCommand OnPresent { get; }
    }

    public class ExportSettingsViewModel : IExportSettingsViewModel {

        public string Framerate { get; private set; }
        public ReactiveCommand<int> OnFramerateUpdate { get; private set; }
        public ReactiveCommand OnExport { get; } = new();
        public ReactiveCommand OnPresent { get; } = new();

        public ExportSettingsViewModel() {
            OnFramerateUpdate.Subscribe(UpdateFramerate);
            OnExport.Subscribe(Export);
            OnPresent.Subscribe(Present);
        }

        private void UpdateFramerate(int framerate) {
            
        }

        private void Export() {

        }

        private void Present() {

        }

    }
}