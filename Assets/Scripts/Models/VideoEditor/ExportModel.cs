using System.IO;

namespace SimpleMotions {

    public interface IExportModel {
        ReactiveValue<int> TargetFrameRate { get; }
        ReactiveValue<string> OutputFilePath { get; }
        ReactiveValue<string> FileName { get; }

        ReactiveCommand Export { get; }

        ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath, string fileName)> OnExport { get; }
        ReactiveCommand<string> OnFilePathInvalid { get; }
    }

    public class ExportModel : IExportModel {

        public ReactiveValue<int> TargetFrameRate { get; } = new();
        public ReactiveValue<string> OutputFilePath { get; } = new();
        public ReactiveValue<string> FileName { get; } = new();

        public ReactiveCommand Export { get; } = new();

        public ReactiveCommand<(int totalFrames, int targetFrameRate, string outputFilePath, string fileName)> OnExport { get; } = new();
        public ReactiveCommand<string> OnFilePathInvalid { get; } = new();

        private int _totalFrames;

        public ExportModel(IVideoPlayerData videoPlayerData) {
            TargetFrameRate.Value = videoPlayerData.TargetFrameRate.Value;
            _totalFrames = videoPlayerData.TotalFrames.Value;
            videoPlayerData.TotalFrames.Subscribe(totalFrames => _totalFrames = totalFrames);

            Export.Subscribe(() => {
                if (IsNameNotUsed()) {
                    OnExport.Execute((_totalFrames, TargetFrameRate.Value, OutputFilePath.Value, FileName.Value));
                }
            });
        }

        private bool IsNameNotUsed() {
            string filePath = Path.Combine(OutputFilePath.Value, FileName.Value);

            if (!filePath.EndsWith(".mp4")) {
                filePath += ".mp4";
            }

            if (File.Exists(filePath)) {
                UnityEngine.Debug.Log($"El archivo en la ruta: {filePath} ya existe.");
                OnFilePathInvalid.Execute(filePath);
                return false;
            }

            return true;
        }

    }
}