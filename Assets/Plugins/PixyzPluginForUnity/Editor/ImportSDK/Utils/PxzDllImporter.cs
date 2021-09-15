using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.IO;
using Pixyz.Plugin4Unity;

public class PixyzDLLImporter : AssetPostprocessor
{
	public void OnPreprocessAsset()
    {
        if (!assetImporter.assetPath.Contains("PixyzPluginForUnity/Editor/Bin/GIT_FORCE_DIRECTORY.txt") && !assetImporter.assetPath.Contains("PixyzPluginForUnity/Editor/Bin/version.txt"))
            return;

        string[] dllFiles = Directory.GetFiles(Application.dataPath.Remove(Application.dataPath.Length - 6, 6) + assetImporter.assetPath.Replace("GIT_FORCE_DIRECTORY.txt", ""), "*.dll", SearchOption.AllDirectories);
        foreach (string dllPath in dllFiles)
        {
            PluginImporter importer = AssetImporter.GetAtPath(dllPath.Remove(0, Application.dataPath.Length - 6)) as PluginImporter;
            if (importer == null)
            {
                Debug.Log(dllPath.Remove(0, Application.dataPath.Length - 6));
                continue;
            }
            
            string dllName = Path.GetFileNameWithoutExtension(dllPath);

            if (dllName == "PixyzPluginUnity" || dllName == "PiXYZPlugin4Unity" || dllName == "PiXYZOptimizeSDK" || dllName == "PiXYZImportSDK")
                importer.SetCompatibleWithEditor(true);
            else
                importer.SetCompatibleWithEditor(false);
            
            importer.SetCompatibleWithAnyPlatform(false);
            importer.SaveAndReimport();
        }

        ClearConsole();
        AssetDatabase.SaveAssets();
        CompilationPipeline.RequestScriptCompilation();
    }

    private void ClearConsole()
    {
        var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");

        var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

        clearMethod.Invoke(null, null);
    }
}
