
namespace SimpleMotions {

    public interface IExportViewModel {
		
		// TODO - Use DTO
        ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath, string fileName)> OnExport { get; }
        ReactiveValue<int> CurrentFrame { get; }
		
    }

    public class ExportViewModel : IExportViewModel {

        public ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath, string fileName)> OnExport { get; } = new();
        public ReactiveValue<int> CurrentFrame { get; } = new();

        private readonly IExportModel _exportModel;
        private readonly IVideoPlayerData _videoPlayerData;

        public ExportViewModel(IExportModel exportModel, IVideoPlayerData videoPlayerData) {
            _exportModel = exportModel;
            _videoPlayerData = videoPlayerData;

            _exportModel.OnExport.Subscribe(OnExport.Execute);
            CurrentFrame.Subscribe(currentFrame => _videoPlayerData.CurrentFrame.Value = currentFrame);
        }

    }

}