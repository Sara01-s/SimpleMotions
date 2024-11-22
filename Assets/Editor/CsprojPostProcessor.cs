using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public static class CsprojPostProcessor {

    [PostProcessBuild]
    public static void OnGeneratedCSProjectFiles() {
        string[] csprojFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csproj");
		
        foreach (var csprojFile in csprojFiles) {
            var lines = File.ReadAllLines(csprojFile);

            for (int i = 0; i < lines.Length; i++) {
                if (lines[i].Contains("<PropertyGroup>")) {
                    lines[i] += "\n    <Nullable>enable</Nullable>";
                    break;
                }
            }
			
            File.WriteAllLines(csprojFile, lines);
        }
    }
}
