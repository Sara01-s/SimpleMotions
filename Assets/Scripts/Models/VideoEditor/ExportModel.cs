
namespace SimpleMotions {

    public interface IExportModel {
        ReactiveValue<int> TargetFrameRate { get; }
        ReactiveValue<string> OutputFilePath { get; }
        ReactiveValue<string> FileName { get; }

        ReactiveCommand Export { get; }
        ReactiveCommand Present { get; }

        ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath, string fileName)> OnExport { get; }
    }

    public class ExportModel : IExportModel {

        public ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath, string fileName)> OnExport { get; } = new();

        public ReactiveValue<int> TargetFrameRate { get; } = new();
        public ReactiveValue<string> OutputFilePath { get; } = new();
        public ReactiveValue<string> FileName { get; } = new();

        public ReactiveCommand Export { get; } = new();
        public ReactiveCommand Present { get; } = new();

        private int _totalFrames;

        public ExportModel(IVideoPlayerData videoPlayerData) {
            TargetFrameRate.Value = videoPlayerData.TargetFrameRate.Value;
            _totalFrames = videoPlayerData.TotalFrames.Value;
            videoPlayerData.TotalFrames.Subscribe(totalFrames => _totalFrames = totalFrames);

            Export.Subscribe(value => OnExport.Execute((_totalFrames, TargetFrameRate.Value, OutputFilePath.Value, FileName.Value)));
        }

    }
}