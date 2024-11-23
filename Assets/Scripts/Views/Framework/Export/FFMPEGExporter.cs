using System.Diagnostics;
using UnityEngine;
using System.IO;

public class FFMPEGExporter : MonoBehaviour {

    public void GenerateVideo(string inputFolder, string outputFilePath, string fileName, int frameRate) {
        outputFilePath = Path.Combine(outputFilePath, fileName);

        if (!outputFilePath.EndsWith(".mp4")) {
            outputFilePath += ".mp4";
        }

        string inputPattern = $"{inputFolder}/frame_%05d.png";
        string arguments = $"-y -framerate {frameRate} -i \"{inputPattern}\" -c:v libx264 -pix_fmt yuv420p -b:v 5000k -preset medium \"{outputFilePath}\"";

        string ffmpegBinPath = GetFFmpegBinPath();

        if (string.IsNullOrEmpty(ffmpegBinPath) || !File.Exists(ffmpegBinPath)) {
            UnityEngine.Debug.LogError($"No se encontró FFmpeg en la ruta especificada: {ffmpegBinPath}");
            return;
        }

        var ffmpegCommand = new Process();
        ffmpegCommand.StartInfo.FileName = ffmpegBinPath;
        ffmpegCommand.StartInfo.Arguments = arguments;
        ffmpegCommand.StartInfo.UseShellExecute = false;
        ffmpegCommand.StartInfo.RedirectStandardError = true;
        ffmpegCommand.StartInfo.RedirectStandardOutput = true;  
        ffmpegCommand.Start();

        string errorOutput = ffmpegCommand.StandardError.ReadToEnd();

        ffmpegCommand.WaitForExit();

        if (ffmpegCommand.ExitCode != 0) {
            UnityEngine.Debug.LogError($"FFMPEG Error: {errorOutput}");
        }
        else {
            UnityEngine.Debug.Log($"Video generado exitosamente: {outputFilePath}");
        }
    }

    private string GetFFmpegBinPath() {
        string path = Application.streamingAssetsPath;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        return Path.Combine(path, "FFMPEG/Windows/bin/ffmpeg.exe");
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
        return Path.Combine(path, "FFMPEG/Linux/ffmpeg");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        return Path.Combine(path, "FFMPEG/Mac/ffmpeg_mac");
#else
        return null;
#endif
    }

}