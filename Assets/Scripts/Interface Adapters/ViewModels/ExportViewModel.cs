
namespace SimpleMotions {

    public interface IExportViewModel {
		
        ReactiveCommand<VideoExportDTO> OnExportStart { get; }
        ReactiveCommand OnExportEnd { get; }
        ReactiveValue<int> CurrentFrame { get; }
		
    }

    public class ExportViewModel : IExportViewModel {

        public ReactiveCommand<VideoExportDTO> OnExportStart { get; } = new();
        public ReactiveCommand OnExportEnd { get; } = new();
        public ReactiveValue<int> CurrentFrame { get; } = new();

        private readonly IExportModel _exportModel;
        private readonly IVideoPlayerData _videoPlayerData;

        public ExportViewModel(IExportModel exportModel, IVideoPlayerData videoPlayerData, IEntitySelectorViewModel entitySelectoViewModel) {
            _exportModel = exportModel;
            _videoPlayerData = videoPlayerData;

            _exportModel.OnExport.Subscribe(exportSettings => {
                entitySelectoViewModel.DeselectEntity.Execute();
                OnExportStart.Execute(exportSettings);
            });
            CurrentFrame.Subscribe(currentFrame => _videoPlayerData.CurrentFrame.Value = currentFrame);

        }

    }

}