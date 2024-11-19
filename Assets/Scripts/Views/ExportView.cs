using System.Collections.Generic;
using System.Collections;
using SimpleMotions;
using UnityEngine;
using System.IO;

public class ExportView : MonoBehaviour {

    [SerializeField] private Camera _cameraToCapture;
    [SerializeField] private int _captureWidth;
    [SerializeField] private int _captureHeight;

    [SerializeField] private FullscreenView _fullscreen;
    [SerializeField] private FFMPEGExporter _ffmpegExporter;

    private IExportViewModel _exportViewModel;
    private List<byte[]> _images = new();

    public void Configure(IExportViewModel exportViewModel) {
        _exportViewModel = exportViewModel;

        _exportViewModel.OnExport.Subscribe(StartExport);
    }

    private void StartExport((int totalFrames, int targetFrameRate, string outputFilePath) data) {
        _fullscreen.SetFullscreen(false);

        StartCoroutine(CO_ExportFrames(data.totalFrames, data.outputFilePath, data.targetFrameRate));

    }

    private IEnumerator CO_ExportFrames(int totalFrames, string outputFilePath, int targetFrameRate) {
        for (int i = 0; i <= totalFrames; i++) {
            _exportViewModel.CurrentFrame.Value = i;
            _images.Add(SaveFrame(i));

            if (i % 5 == 0) {
                yield return null;
            }
        }

        string temporaryFilePath = SaveImagesToDisk();

        _ffmpegExporter.GenerateVideo(temporaryFilePath, outputFilePath, targetFrameRate);

        if (Directory.Exists(temporaryFilePath)) {
            Directory.Delete(temporaryFilePath, true);
        }

        Debug.Log("Exportación completada.");

        _exportViewModel.CurrentFrame.Value = 0;
        _fullscreen.SetDefaultScreen();
    }

    private byte[] SaveFrame(int i) {
        RenderTexture renderTexture = new RenderTexture(_captureWidth, _captureHeight, 24);
        _cameraToCapture.targetTexture = renderTexture; 

        Texture2D highQualityTexture = new Texture2D(_captureWidth, _captureHeight, TextureFormat.RGB24, false);

        _cameraToCapture.Render(); // Renderiza la escena a la textura
        RenderTexture.active = renderTexture; // Establecer el RenderTexture como activo
        highQualityTexture.ReadPixels(new Rect(0, 0, _captureWidth, _captureHeight), 0, 0); // Lee los píxeles
        highQualityTexture.Apply(); // Aplica los cambios

        // Limpiar
        _cameraToCapture.targetTexture = null; 
        RenderTexture.active = null; 

        byte[] imageBytes = highQualityTexture.EncodeToPNG();

        return imageBytes;
    }

    private string SaveImagesToDisk() {
        var temporaryFolderPath = Path.Combine(Application.persistentDataPath, "TempFrames");

        if (!Directory.Exists(temporaryFolderPath)) {
            Directory.CreateDirectory(temporaryFolderPath);
        }

        for (int i = 0; i < _images.Count; i++) {
            string filePath = Path.Combine(temporaryFolderPath, $"frame_{i:D5}.png"); 
            File.WriteAllBytes(filePath, _images[i]);
        }

        Debug.Log($"Imágenes guardadas en {temporaryFolderPath}");

        return temporaryFolderPath;
    }

}