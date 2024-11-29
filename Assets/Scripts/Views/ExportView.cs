using System.Collections;
using SimpleMotions;
using UnityEngine;
using System.IO;

public class ExportView : MonoBehaviour {

    [SerializeField] private Camera _cameraToCapture;
	[SerializeField] private Vector2Int _videoResolution;

    [SerializeField] private FullscreenView _fullscreen;
    [SerializeField] private FFMPEGExporter _ffmpegExporter;

	[SerializeField] private ComputeShader _exportVideo;

    private IExportViewModel _exportViewModel;

	private bool _exportSttoped;

    public void Configure(IExportViewModel exportViewModel) {
        exportViewModel.OnExportStart.Subscribe(StartExport);
        _exportViewModel = exportViewModel;
    }

	public void CancelExport() {
		_exportSttoped = true;
	}

    private void StartExport((int totalFrames, int targetFrameRate, string outputFilePath, string fileName) data) {
		_exportSttoped = false;
        _fullscreen.SetFullscreen(withPlayback: false);
        StartCoroutine(CO_ExportFrames(data.totalFrames, data.targetFrameRate, data.outputFilePath, data.fileName));
    }

	private IEnumerator CO_ExportFrames(int totalFrames, int targetFrameRate, string outputFilePath, string fileName) {
		string tempFrameImagesFilePath = GetFramesTempDirectory();

		var processedRT = new RenderTexture(_videoResolution.x, _videoResolution.y, 0, RenderTextureFormat.ARGB32) {
			enableRandomWrite = true,
		};
		
		processedRT.Create();
		var outputTexture = new Texture2D(_videoResolution.x, _videoResolution.y, TextureFormat.RGBA32, false);

		for (int frame = 0; frame <= totalFrames; frame++) {
			if (_exportSttoped) {
				break;
			}

			_exportViewModel.CurrentFrame.Value = frame;

			byte[] frameBytes = GetFrameAsPng(processedRT, outputTexture);
			string frameAbsoluteFilepath = Path.Combine(tempFrameImagesFilePath, $"frame_{frame:D5}.png");
			File.WriteAllBytes(frameAbsoluteFilepath, frameBytes);

			yield return null;
		}

		_cameraToCapture.targetTexture = null;

		RenderTexture.active = null;
		processedRT.Release();
		Destroy(processedRT);
		Destroy(outputTexture);

		if (!_exportSttoped) {
			_ffmpegExporter.GenerateVideo(tempFrameImagesFilePath, outputFilePath, fileName, targetFrameRate);
		}

		if (Directory.Exists(tempFrameImagesFilePath)) {
			Directory.Delete(tempFrameImagesFilePath, recursive: true);
		}

		Debug.Log("Exportación completada.");
		_exportViewModel.OnExportEnd.Execute();

		_exportViewModel.CurrentFrame.Value = 0;
		_fullscreen.SetDefaultScreen();
	}

	private byte[] GetFrameAsPng(RenderTexture processedRT, Texture2D outputTexture) {
		_cameraToCapture.targetTexture = processedRT;
		_cameraToCapture.Render();

		int kernel = _exportVideo.FindKernel("ExportFrame");
		_exportVideo.SetTexture(kernel, "_InputTexture", processedRT);
		_exportVideo.SetTexture(kernel, "_ResultTexture", processedRT);

		int threadGroupsX = Mathf.CeilToInt(processedRT.width / 8.0f);
		int threadGroupsY = Mathf.CeilToInt(processedRT.height / 8.0f);
		_exportVideo.Dispatch(kernel, threadGroupsX, threadGroupsY, 1);

		RenderTexture.active = processedRT;
		outputTexture.ReadPixels(new Rect(0, 0, processedRT.width, processedRT.height), 0, 0);
		outputTexture.Apply();

		return outputTexture.EncodeToPNG();
	}


	private string GetFramesTempDirectory() {
        var tempDirectoryPath = Path.Combine(Application.persistentDataPath, ".TempFrames");

        if (!Directory.Exists(tempDirectoryPath)) {
            Directory.CreateDirectory(tempDirectoryPath);
        }

		return tempDirectoryPath;
	}

}