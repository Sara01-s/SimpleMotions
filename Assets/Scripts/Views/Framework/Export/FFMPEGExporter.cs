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
            UnityEngine.Debug.LogError($"ffmpeg not found at: {ffmpegBinPath}");
            return;
        }

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		AllowWritePermissonsOSX();
#endif

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
            UnityEngine.Debug.Log($"Video generated successfully: {outputFilePath}");
        }
    }

	private void AllowWritePermissonsOSX() {
		string ffmpegBinPath = GetFFmpegBinPath();
		string program = "xattr";
		string arguments = $"-r com.apple.quarantine {ffmpegBinPath}";

		var cmd = new Process();
		cmd.StartInfo.FileName = program;
		cmd.StartInfo.Arguments = arguments;
        cmd.StartInfo.UseShellExecute = false;
        cmd.StartInfo.RedirectStandardError = true;
        cmd.StartInfo.RedirectStandardOutput = true;
		cmd.Start();

		string errorOutput = cmd.StandardError.ReadToEnd();

        cmd.WaitForExit();

        if (cmd.ExitCode != 0) {
            UnityEngine.Debug.LogError($"xattr error: {errorOutput}");
        }
	}

    private string GetFFmpegBinPath() {
        string path = Application.streamingAssetsPath;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        return Path.Combine(path, "Windows/bin/ffmpeg.exe");
#elif UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
        return Path.Combine(path, "Linux/ffmpeg");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        return Path.Combine(path, "OSX/ffmpeg");
#endif
    }

}