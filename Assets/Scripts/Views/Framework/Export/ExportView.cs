using System.Collections;
using SimpleMotions;
using UnityEngine;
using System.IO;

public class ExportView : MonoBehaviour {

    [SerializeField] private Camera _cameraToCapture;
	[SerializeField] private Vector2Int _videoResolution;

    [SerializeField] private FullscreenView _fullscreen;
    [SerializeField] private FFMPEGExporter _ffmpegExporter;

    private IExportViewModel _exportViewModel;

	private bool _exportStopped;

    public void Configure(IExportViewModel exportViewModel) {
        exportViewModel.OnExportStart.Subscribe(StartExport);
        _exportViewModel = exportViewModel;
    }

	public void CancelExport() {
		_exportStopped = true;
	}

    private void StartExport(VideoExportDTO videoExportDTO) {
		_exportStopped = false;
        _fullscreen.SetFullscreen(withPlayback: false);
        StartCoroutine(CO_ExportFrames(videoExportDTO.TotalFrames, videoExportDTO.TargetFrameRate, videoExportDTO.OutputFilepath, videoExportDTO.FileName));
    }

	private IEnumerator CO_ExportFrames(int totalFrames, int targetFrameRate, string outputFilePath, string fileName) {
		yield return new WaitForEndOfFrame();

		string tempFrameImagesFilePath = GetFramesTempDirectory();

		var frameRenderTexture = new RenderTexture(_videoResolution.x, _videoResolution.y, depth: 0, RenderTextureFormat.ARGB32) {
			enableRandomWrite = true,
		};
		
		frameRenderTexture.Create();
		var outputTexture = new Texture2D(_videoResolution.x, _videoResolution.y, TextureFormat.ARGB32, false);

		if (!_cameraToCapture.isActiveAndEnabled) {
		    Debug.LogError("Camera is not active or enabled.");
		}

		for (int frame = 0; frame <= totalFrames; frame++) {
			if (_exportStopped) {
				break;
			}

			_exportViewModel.CurrentFrame.Value = frame;

			byte[] frameBytes = GetFrameAsPng(frameRenderTexture, outputTexture);
			string frameAbsoluteFilepath = Path.Combine(tempFrameImagesFilePath, $"frame_{frame:D5}.png");
			File.WriteAllBytes(frameAbsoluteFilepath, frameBytes);

			yield return null;
		}

		_cameraToCapture.targetTexture = null;

		RenderTexture.active = null;
		frameRenderTexture.Release();
		Destroy(frameRenderTexture);
		Destroy(outputTexture);

		if (!_exportStopped) {
			_ffmpegExporter.GenerateVideo(tempFrameImagesFilePath, outputFilePath, fileName, targetFrameRate);
		}

		if (Directory.Exists(tempFrameImagesFilePath)) {
			Directory.Delete(tempFrameImagesFilePath, recursive: true);
		}

		_exportViewModel.OnExportEnd.Execute();
		_exportViewModel.CurrentFrame.Value = 0;
		_fullscreen.SetDefaultScreen();
		Debug.Log("Export completed succesfully.");
	}

	private byte[] GetFrameAsPng(RenderTexture frameRenderTexture, Texture2D outputTexture) {
		_cameraToCapture.targetTexture = frameRenderTexture;
		_cameraToCapture.Render();

		RenderTexture.active = frameRenderTexture;
		outputTexture.ReadPixels(new Rect(0, 0, frameRenderTexture.width, frameRenderTexture.height), 0, 0);
		outputTexture.Apply();

		return outputTexture.EncodeToPNG();
	}

	private string GetFramesTempDirectory() {
		const string tempFramesDirectoryName = ".TempFrames";
        var tempDirectoryPath = Path.Combine(Application.persistentDataPath, tempFramesDirectoryName);

        if (!Directory.Exists(tempDirectoryPath)) {
            Directory.CreateDirectory(tempDirectoryPath);
        }

		return tempDirectoryPath;
	}

}