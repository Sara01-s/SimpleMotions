using System.Diagnostics;
using UnityEngine;

public class FFMPEGExporter : MonoBehaviour {

    [SerializeField] private string _ffmpegPath;

    public void GenerateVideo(string inputFolder, string outputFilePath, int frameRate) {
        if (!outputFilePath.EndsWith(".mp4")) {
            outputFilePath += ".mp4";
        }

        string inputPattern = $"{inputFolder}/frame_%05d.png";
        string arguments = $"-y -framerate {frameRate} -i \"{inputPattern}\" -c:v libx264 -pix_fmt yuv420p -b:v 5000k -preset medium \"{outputFilePath}\"";

        var ffmpegCommand = new Process();
        ffmpegCommand.StartInfo.FileName = _ffmpegPath;
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

}