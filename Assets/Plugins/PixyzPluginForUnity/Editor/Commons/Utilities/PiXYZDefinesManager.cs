using System.IO;
using UnityEditor;
using UnityEditor.Compilation;

namespace Pixyz.Plugin4Unity
{
    public class PixyzDefinesManager
    {
        static readonly string[] PIXYZ_DEFINES_ARRAY = {
            "PIXYZ",
            "PIXYZ_2019_2",
            "PIXYZ_2019_2_OR_NEWER",
            "PIXYZ_2020_1",
            "PIXYZ_2020_1_OR_NEWER",
            "PIXYZ_2020_2",
            "PIXYZ_2020_2_OR_NEWER",
            "PIXYZ_RULE_ENGINE",
            "PXZ_CUSTOM_DLL_PATH"
        };
        public const string PIXYZ_DEFINES = "PIXYZ;" +
            "PIXYZ_2019_2;PIXYZ_2019_2_OR_NEWER;" +
            "PIXYZ_2020_1;PIXYZ_2020_1_OR_NEWER;" +
            "PIXYZ_2020_2;PIXYZ_2020_2_OR_NEWER;" +
            "PIXYZ_RULE_ENGINE" +
            "PXZ_CUSTOM_DLL_PATH";


        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            AddDefines();
#if UNITY_2019_1_OR_NEWER
            CompilationPipeline.compilationStarted += (o) => CompilationStarted();
#else
            CompilationPipeline.assemblyCompilationStarted += (o) => CompilationStarted();
#endif
        }

        private static void CompilationStarted()
        {
            string thisSourceFilePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            // The code below might execute even if this file got deleted (it is still in the last built binaries).
            // If this file no longer exists, it means that Pixyz is not longer present (or parts of it are missing) so defines are removed
            if (!File.Exists(thisSourceFilePath)) {
                RemoveDefines();
            }
        }

        public static void AddDefines()
        {
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            for(int i=0;i< PIXYZ_DEFINES_ARRAY.Length;i++)
            {
                if(!currentDefines.Contains(PIXYZ_DEFINES_ARRAY[i]))
                {
                    currentDefines = $"{PIXYZ_DEFINES_ARRAY[i]};{currentDefines}";
                }
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefines);
            //if (!currentDefines.Contains(PIXYZ_DEFINES)) {
            //    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, $"{PIXYZ_DEFINES};{currentDefines}");
            //}
        }

        public static void RemoveDefines()
        {
            var currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (currentDefines.Contains(PIXYZ_DEFINES)) {
                currentDefines = currentDefines.Replace($"{PIXYZ_DEFINES}", "");
                currentDefines = currentDefines.Trim(new[] { ';' });
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, currentDefines);
            }
        }
    }
}