
namespace SimpleMotions {

    public interface IExportModel {
        ReactiveValue<int> TargetFrameRate { get; }
        ReactiveValue<string> OutputFilePath { get; }

        ReactiveCommand Export { get; }
        ReactiveCommand Present { get; }

        ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath)> OnExport { get; }
    }

    public class ExportModel : IExportModel {

        public ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath)> OnExport { get; } = new();

        public ReactiveValue<int> TargetFrameRate { get; } = new();
        public ReactiveValue<string> OutputFilePath { get; } = new();

        public ReactiveCommand Export { get; } = new();
        public ReactiveCommand Present { get; } = new();

        private IVideoPlayerData _videoPlayerData;
        
        private int _totalFrames;

        public ExportModel(IVideoPlayerData videoPlayerData) {
            _videoPlayerData = videoPlayerData;

            TargetFrameRate.Value = _videoPlayerData.TargetFrameRate.Value;

            _totalFrames = _videoPlayerData.TotalFrames.Value;

            _videoPlayerData.TotalFrames.Subscribe(totalFrames => _totalFrames = totalFrames);
            
            Export.Subscribe(value => OnExport.Execute((_totalFrames, TargetFrameRate.Value, OutputFilePath.Value)));
        }

    }
}