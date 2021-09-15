using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections;
using Pixyz.OptimizeSDK.Utils;
using Pixyz.Commons.Editor;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.Utilities;
using Pixyz.Commons.Utilities.Editor;
using Pixyz.Commons.UI.Editor;
using Pixyz.Plugin4Unity.Core;
using UnityEngine.Rendering;
using Pixyz.Plugin4Unity;

namespace Pixyz.ImportSDK {

    /// <summary>
    /// Editor Window for the Import Window
    /// </summary>
    [ExecuteInEditMode]
    public class ImportWindow : SingletonEditorWindow
    {
        public const string DEFAULT_IMPORT_SETTINGS_NAME = "Default Import Settings";

        public override string WindowTitle => "Import Model in Scene";

        protected string _fileToImport = null;
#if UNITY_2020_1_OR_NEWER
        protected BackgroundProgressBar _progressBar;
#else
        protected ProgressBar _progressBar;
#endif
        protected bool _isSettingsOpen = true;
        protected bool _isPreprocessOpen = true;
        protected Vector2 _scrollPosition;
        protected UnityEditor.Editor _settingsEditor;
        protected Importer _importer;
        protected static string DefaultSettingsPath => Preferences.DefaultImportSettingsLocation;

        protected bool isReady => !(string.IsNullOrEmpty(_fileToImport) || !Formats.IsFileSupported(_fileToImport) || !File.Exists(_fileToImport));

        public static void Open(string file)
        {
            var window = EditorExtensions.OpenWindow<ImportWindow>();
            window._fileToImport = file;

            if (Preferences.AutomaticUpdate)
                Plugin4Unity.EditorWindows.UpdateWindow.AutoPopup();
        }

        protected static bool? _CreatePrefab;
        public static bool CreatePrefab {
            get {
                if (_CreatePrefab == null) {
                    _CreatePrefab = EditorPrefs.GetBool("Pixyz_CreatePrefab", false);
                }
                return (bool)_CreatePrefab;
            }
            set {
                if (CreatePrefab != value) {
                    _CreatePrefab = value;
                    EditorPrefs.SetBool("Pixyz_CreatePrefab", value);
                }
            }
        }

        protected static ImportSettings _ImportSettings;
        public static ImportSettings ImportSettings {
            get {
                if (_ImportSettings == null) {
                    _ImportSettings = AssetDatabase.LoadAssetAtPath<ImportSettings>(DefaultSettingsPath + '/' + DEFAULT_IMPORT_SETTINGS_NAME + ".asset");
                    if (_ImportSettings == null) {

                        if (!Directory.Exists(Preferences.DefaultImportSettingsLocation))
                            Directory.CreateDirectory(Preferences.DefaultImportSettingsLocation);

                        _ImportSettings = ScriptableObject.CreateInstance<ImportSettings>();
                        _ImportSettings.name = DEFAULT_IMPORT_SETTINGS_NAME;
                        EditorExtensions.SaveAsset(_ImportSettings, DEFAULT_IMPORT_SETTINGS_NAME, false, DefaultSettingsPath);
                    }
                }
                return _ImportSettings;
            }
            set {
                if (_ImportSettings == value)
                    return;
                _ImportSettings = value;
            }
        }

        protected Action postProcessOver;

        protected void OnEnable()
        {
            setSize();
            titleContent.image = TextureCache.GetTexture("IconBrowse"); // Not working for some reasons ?
        }

        protected void setSize()
        {
            minSize = new Vector2(430f, 500f);
            maxSize = new Vector2(430f, 1000f);
        }



        protected void OnGUI()
        {
            EditorGUIUtility.labelWidth = 190;

            EditorGUILayout.BeginVertical();

            // Draw pixyz background
            GUI.DrawTexture(new Rect(position.width - 430, position.height - 430, 500, 500), TextureCache.GetTexture("IconPixyzWatermark"));

            GUIStyle margin = new GUIStyle { margin = new RectOffset(10, 2, 4, 4) };
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            
            var compatibility = Configuration.IsPluginCompatible();
            if (compatibility != Compatibility.COMPATIBLE) {
                EditorGUILayout.BeginVertical(new GUIStyle { margin = new RectOffset(6, 6, 6, 0) });
                string message = "";
                if (compatibility == Compatibility.UNTESTED) {
                    message += $"The Pixyz Plugin for Unity {Configuration.PixyzVersion} is not compatible with Unity {Configuration.UnityVersion}";
                    if (Configuration.UpdateStatus?.newVersionAvailable == true) { // This won't trigger the check itself. If the check was done, it will be skipped.
                        message += "\nHowever, another version of the Pixyz Plugin for Unity is available online";
                    }
                } else {
                    message += $"The Pixyz Plugin for Unity {Configuration.PixyzVersion} is not compatible with Unity {Configuration.UnityVersion}";
                }
                EditorGUILayout.HelpBox(message, MessageType.Warning);
                EditorGUILayout.EndVertical();
            }

            if (Configuration.CurrentLicense != null && !Configuration.IsFloatingLicense()) {
                if(Configuration.CurrentLicense.endDate.year != -1)
                {
                    int remainingDays = (Configuration.CurrentLicense.endDate.ToUnityObject() - DateTime.Today).Days;
                    if (remainingDays < 30 && remainingDays >= 0)
                    {
                        EditorGUILayout.BeginHorizontal(new GUIStyle { padding = new RectOffset(10, 10, 10, 0) });
                        GUIStyle myStyle = GUI.skin.GetStyle("HelpBox");
                        myStyle.richText = true;

                        EditorGUILayout.HelpBox("Your Pixyz license will expire in <color=orange>" + remainingDays + " days</color>\nPlease contact us at sales@pi.xyz for renewal", MessageType.Warning);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                
            }

            string ext = Formats.GetExtension2(_fileToImport);
            if (ext == ".wire" && !File.Exists(Preferences.AliasExecutable)) {
                EditorGUILayout.HelpBox("Path to Autodesk Alias executable must be set in \"Preferences > Pixyz\" to import .wire files", MessageType.Error);
            }
            if (ext == ".vpb" && !File.Exists(Preferences.VREDExecutable)) {
                EditorGUILayout.HelpBox("Path to Autodesk VRED executable must be set in \"Preferences > Pixyz\" to import .vpb files", MessageType.Error);
            }

            if ((ext == ".rcp" || ext == ".rcs") && !Directory.Exists(Preferences.RecapSDKPath))
            {
                EditorGUILayout.HelpBox("Path to Autodesk ReCap executable must be set in \"Preferences > Pixyz\" to import .rcp files", MessageType.Error);
            }

            EditorGUILayout.BeginVertical(margin);

            // File Block
            EditorGUIExtensions.DrawCategory(ImportSettingsEditor.Style, "File", () => {
                EditorGUILayout.BeginHorizontal();
                _fileToImport = EditorGUILayout.TextField((FilePath)_fileToImport);
                GUI.SetNextControlName("BrowseButton");
                if (GUILayout.Button("Browse", UnityEditor.EditorStyles.miniButton, GUILayout.Width(55))) {
                    FilePath file = (FilePath)EditorExtensions.SelectFile(Formats.SupportedFormatsForFileBrowser);
                    if (!String.IsNullOrEmpty(file))
                        _fileToImport = file;
                    GUI.FocusControl("BrowseButton");
                }
                EditorGUILayout.EndHorizontal();
            }, "File to import in Unity");

            if (!File.Exists(_fileToImport) || !Formats.IsFileSupported(_fileToImport)) {
                goto skipUI;
            }

            // Preprocess
            DrawPreprocess();

            // Settings Block
            DrawSettings();

            //PostProcess
            EditorGUIExtensions.DrawCategory(ImportSettingsEditor.Style, "Post-Processing", ()=> {
                DrawPostProcess();
            }, "Post-Processing operations that happen after the file has been importer. Only works in editor mode.");
                
     
            skipUI:;

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUIExtensions.DrawLine();

            // Import Button
            DrawImportButton();
            
            EditorGUILayout.EndVertical();
        }

        private void editorChanged()
        {
            Repaint();
        }

        public void OnImportClicked()
        {
#if UNITY_2020_1_OR_NEWER
            _progressBar = new BackgroundProgressBar(importCanceled, $"Importing \"{Path.GetFileName(_fileToImport)}\"");
#else
            _progressBar = new ProgressBar(importCanceled, $"Importing \"{Path.GetFileName(_fileToImport)}\"");
#endif

            Native.NativeInterface.SetModuleProperty("IO", "AliasApiDllPath", Preferences.AliasExecutable);
            Native.NativeInterface.SetModuleProperty("IO", "VredExecutablePath", Preferences.VREDExecutable);
            Native.NativeInterface.SetModuleProperty("IO", "RecapSDKPath", Preferences.RecapSDKPath);

            _importer = new Importer(_fileToImport, ImportSettings);
            _importer.printMessageOnCompletion = Preferences.LogImportTime;
            _importer.completed += importEnded;
            _importer.progressed += importProgressed;

            _importer.run();

            Close();
        }

        private void importCanceled()
        {
            _importer?.stop();
        }

        protected virtual void importEnded(GameObject gameObject)
        {

            if (gameObject == null) {
                Debug.LogError("Import Failed.");
                return;
            }

            Dispatcher.StartCoroutine(runPostProcesses(gameObject));
        }

        protected virtual IEnumerator runPostProcesses(GameObject gameObject)
        {
            yield return Dispatcher.DelayFrames(1);

            // Creating Frozen ImportSettings, with import times.
            //ImportStamp importedModel = gameObject.GetComponent<ImportStamp>();
            if (CreatePrefab)
                postProcessOver += () => { createPrefab(gameObject); };
            RunAdditionalPostProcess();

            importProgressed(1f, null);
        }

        protected void createPrefab(GameObject gameObject)
        {
            ImportStamp importedModel = gameObject.GetComponent<ImportStamp>();
            // Prefab
            //if (CreatePrefab)
            //{
                // Creating a prefab
                NonPersistentSerializer.Enabled = false;
                try
                {
                    importProgressed(0.8f, "Creating prefab...");

                    string path = "Assets/" + Preferences.PrefabFolder + "/" + gameObject.name;

                    var volatileDependencies = EditorExtensions.GetNonPersistentDependenciesRecursive(gameObject);

                    switch (Preferences.PrefabDependenciesDestination)
                    {
                        case PrefabDependenciesDestination.InPrefab:
                            if (!AssetDatabase.Contains(importedModel.importSettings))
                                CollectionExtensions.Append(ref volatileDependencies, importedModel.importSettings);
                            gameObject = gameObject.CreatePrefab(path, volatileDependencies);
                            break;
                        case PrefabDependenciesDestination.InFolder:
                            AssetDatabase.StartAssetEditing();
                            foreach (var dep in volatileDependencies)
                            {
                                string depPath = path + "/" + dep.name;
                                dep.CreateAsset(depPath);
                            }
                            AssetDatabase.StopAssetEditing();
                            gameObject = gameObject.CreatePrefab(path, AssetDatabase.Contains(importedModel.importSettings) ? null : new UnityEngine.Object[] { importedModel.importSettings });
                            break;
                    }
                }
                catch (Exception prefabException)
                {
                    // An exception has occured while making prefab
                    importProgressed(1f, "Extraction exception");
                    Debug.LogException(prefabException);
                }
                NonPersistentSerializer.Enabled = true;
            //}
        }

        private void importProgressed(float progress, string message)
        {
            _progressBar.SetProgress(progress, message);
        }

        protected virtual void DrawPreprocess()
        {
            var preprocess = Importer.GetPreprocess(_fileToImport);
            if (preprocess != null)
            {
                preprocess.onBeforeDraw(ImportSettings);
                _isPreprocessOpen = EditorGUIExtensions.DrawCategory(ImportSettingsEditor.Style, preprocess.name, true, () =>
                {
                    if (_isPreprocessOpen)
                        preprocess.fieldInstances.DrawGUILayout();
                }, "Preprocess to run",
                _isPreprocessOpen);
            }
        }

        protected virtual void DrawSettings()
        {
            _isSettingsOpen = EditorGUIExtensions.DrawCategory(ImportSettingsEditor.Style, "Settings", true, () => {

                EditorGUILayout.BeginHorizontal();

                if (!ImportSettings.IsSerialized())
                {
                    var newSettings = (ImportSettings)EditorGUILayout.ObjectField("Preset", null, typeof(ImportSettings), false);
                    if (newSettings != null)
                        ImportSettings = newSettings;
                    // Save the preset
                    if (GUILayout.Button("Create", UnityEditor.EditorStyles.miniButton, GUILayout.Width(50)))
                    {
                        EditorExtensions.SaveAsset(ImportSettings, null, true); // Bug relou de naming ici (très mineur)
                    }
                }
                else
                {
                    ImportSettings = (ImportSettings)EditorGUILayout.ObjectField("Preset", ImportSettings, typeof(ImportSettings), false);
                }
                EditorGUILayout.EndHorizontal();
                if (_isSettingsOpen)
                {
                    // Draw Settings Editor GUI
                    var editor = ImportSettings.GetEditor<ImportSettingsEditor>();
                    editor.drawGUI(Importer.GetSettingsTemplate(_fileToImport));
                    if (_settingsEditor != editor)
                    {
                        _settingsEditor = editor;
                        editor.changed += editorChanged;
                    }
                }
            }, "Settings are stored in a ImportSettings object. Click create to save the settings below in an ImportSettings file (in the Assets) for later reuse.",
               _isSettingsOpen);
        }
    
        protected virtual void DrawPostProcess()
        {
            CreatePrefab = EditorGUILayout.Toggle("Create Prefab", CreatePrefab);
            if (CreatePrefab)
                EditorGUILayout.HelpBox("This process might take a long time depending on the complexity of the imported model.", MessageType.Warning);
        }
    
        protected virtual void DrawImportButton()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.Space(46);
                EditorGUI.BeginDisabledGroup(!isReady);
                if (GUI.Button(new Rect(position.width / 2 - 100, position.height - 40, 200, 30), "Import"))
                {
                    OnImportClicked();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void RunAdditionalPostProcess() { }
    }
}