using System.Collections.Generic;
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
    private readonly List<byte[]> _frameImages = new();

    public void Configure(IExportViewModel exportViewModel) {
        exportViewModel.OnExport.Subscribe(StartExport);
        _exportViewModel = exportViewModel;
    }

    private void StartExport((int totalFrames, int targetFrameRate, string outputFilePath) data) {
        _fullscreen.SetFullscreen(withPlayback: false);
        StartCoroutine(CO_ExportFrames(data.totalFrames, data.outputFilePath, data.targetFrameRate));
    }

    private IEnumerator CO_ExportFrames(int totalFrames, string outputFilePath, int targetFrameRate) {
        for (int frame = 0; frame <= totalFrames; frame++) {
            _exportViewModel.CurrentFrame.Value = frame;
            _frameImages.Add(GetFrameAsPng());

			yield return null;
        }

        string tempFrameImagesFilePath = GetFramesTempDirectory();

		SaveFrameImagesToDisk(tempFrameImagesFilePath);
        _ffmpegExporter.GenerateVideo(tempFrameImagesFilePath, outputFilePath, targetFrameRate);

        if (Directory.Exists(tempFrameImagesFilePath)) {
            Directory.Delete(tempFrameImagesFilePath, recursive: true);
        }

        Debug.Log("Exportación completada.");

        _exportViewModel.CurrentFrame.Value = 0;
        _fullscreen.SetDefaultScreen();
    }

    private byte[] GetFrameAsPng() {
        _cameraToCapture.targetTexture = new RenderTexture(_videoResolution.x, _videoResolution.y, depth: 24);
        _cameraToCapture.Render(); // Renderiza la escena a la textura
        RenderTexture.active = _cameraToCapture.targetTexture; // Establecer el RenderTexture como activo

        var highQualityTexture = new Texture2D(_videoResolution.x, _videoResolution.y, TextureFormat.RGB24, false);
        highQualityTexture.ReadPixels(new Rect(0.0f, 0.0f, _videoResolution.x, _videoResolution.y), 0, 0); // Lee los píxeles
        highQualityTexture.Apply();

        _cameraToCapture.targetTexture = null; 
        RenderTexture.active = null; 

        return highQualityTexture.EncodeToPNG();
    }

	private string GetFramesTempDirectory() {
        var tempDirectoryPath = Path.Combine(Application.persistentDataPath, ".TempFrames");

        if (!Directory.Exists(tempDirectoryPath)) {
            Directory.CreateDirectory(tempDirectoryPath);
        }

		return tempDirectoryPath;
	}

    private void SaveFrameImagesToDisk(string filePath) {
        for (int i = 0; i < _frameImages.Count; i++) {
            string frameAbsoluteFilepath = Path.Combine(filePath, $"frame_{i:D5}.png");
            File.WriteAllBytes(frameAbsoluteFilepath, _frameImages[i]);
        }

        Debug.Log($"Imágenes guardadas en {filePath}");
    }

}