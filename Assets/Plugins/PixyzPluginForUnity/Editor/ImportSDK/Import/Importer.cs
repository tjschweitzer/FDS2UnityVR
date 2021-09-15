  using Pixyz.Commons.Extensions;
using Pixyz.Commons.Utilities;
using Pixyz.OptimizeSDK.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Pixyz.ImportSDK.Native;
using NUnit.Framework.Constraints;
using Pixyz.ImportSDK.Native.Core;
using System.Reflection;

namespace Pixyz.ImportSDK {

    public delegate void ExceptionHandler(Exception exception);

    /// <summary>
    /// Single-use class for importing 3D data to Unity.<br/>
    /// This class can be used in the editor as well as in runtime, if the license requirements are met.
    /// </summary>
    public sealed class Importer {

        public static Importer RunningInstance;

        private static ImportStamp _LatestModelImportedObject;
        /// <summary>
        /// The GameObject reference to the latest imported model. Returns null if no model was imported during this session.
        /// </summary>
        public static ImportStamp LatestModelImportedObject {
            get {
                if (_LatestModelImportedObject == null) {
                    _LatestModelImportedObject = GameObject.FindObjectsOfType<ImportStamp>().OrderByDescending(x => x.importTime).FirstOrDefault();
                }
                return _LatestModelImportedObject;
            }
        }

        /// <summary>
        /// The file path to the latest imported model. Returns null if no model was imported during this session.
        /// </summary>
        public static string LatestModelImportedPath { get; private set; }

        private static Dictionary<string, ImportSettingsTemplate> _SettingsTemplate = new Dictionary<string, ImportSettingsTemplate>();
        /// <summary>
        /// Add a pre-process action to run for a specific file format.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="preprocessingAction"></param>
        public static void AddOrSetTemplate(string format, ImportSettingsTemplate template) {
            format = format.ToLower();
            if (_SettingsTemplate.ContainsKey(format.ToLower())) {
                _SettingsTemplate[format] = template;
            } else {
                _SettingsTemplate.Add(format, template);
            }
        }
        
        private static Dictionary<string, SubProcess> _Preprocesses = new Dictionary<string, SubProcess>();
        /// <summary>
        /// Add a pre-process action to run for a specific file format.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="preprocessingAction"></param>
        public static void AddOrSetPreprocess(string format, SubProcess preprocessingAction) {
            format = format.ToLower();
            if (_Preprocesses.ContainsKey(format.ToLower())) {
                _Preprocesses[format] = preprocessingAction;
            } else {
                _Preprocesses.Add(format, preprocessingAction);
            }
        }

        private static Dictionary<string, SubProcess> _Postprocesses = new Dictionary<string, SubProcess>();
        /// <summary>
        /// Add a post-process action to run for a specific file format.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="postprocessingAction"></param>
        public static void AddOrSetPostprocess(string format, SubProcess postprocessingAction) {
            format = format.ToLower();
            if (_Postprocesses.ContainsKey(format.ToLower())) {
                _Postprocesses[format] = postprocessingAction;
            } else {
                _Postprocesses.Add(format, postprocessingAction);
            }
        }
        
        public static ImportSettingsTemplate GetSettingsTemplate(string filePath) {
            ImportSettingsTemplate template;
            if (!string.IsNullOrEmpty(filePath) && _SettingsTemplate.TryGetValue(Path.GetExtension(filePath).ToLower(), out template)) {
                return template;
            } else {
                return ImportSettingsTemplate.Default;
            }
        }

        
        public static SubProcess GetPreprocess(string filePath) {
            SubProcess preprocess;
            if (!string.IsNullOrEmpty(filePath) && _Preprocesses.TryGetValue(Path.GetExtension(filePath).ToLower(), out preprocess)) {
                return preprocess;
            } else {
                return null;
            }
        }

        public static SubProcess GetPostprocess(string filePath) {
            SubProcess postprocess;
            if (!string.IsNullOrEmpty(filePath) && _Postprocesses.TryGetValue(Path.GetExtension(filePath).ToLower(), out postprocess)) {
                return postprocess;
            } else {
                return null;
            }
        }
        
        /// <summary>
        /// Callback function triggered everytime the importer has progressed.
        /// Always occurs in the main thread.
        /// </summary>
        public event ProgressHandler progressed;

        /// <summary>
        /// Callback function trigerred when the import failed
        /// </summary>
        public event ExceptionHandler failed;

        private System.Diagnostics.Stopwatch _stopwatch;
        /// <summary>
        /// Elasped ticks since the begining of the import.
        /// </summary>
        public long elaspedTicks { get { return _stopwatch.ElapsedTicks; } }

        /// <summary>
        /// Callback function triggered when the importer has finished importing.
        /// In Async mode, this callback is triggered only when everything is finished.
        /// Always occurs in the main thread.
        /// </summary>
        public event GameObjectToVoidHandler completed;

        private string _file;
        /// <summary>
        /// The file to import
        /// </summary>
        public string filePath {
            get { return _file; }
            set { if (_hasStarted) { throw new AccessViolationException("Can only set Importer properties before it runs"); } _file = value; } }

        private bool _isAsynchronous = true;
        /// <summary>
        /// [Default is True]
        /// If set to true, the import process will run as much as it can on different threads than the main one, so that it won't freeze the Editor/Application and performances are kept to the maximum.
        /// In Asynchronous mode, it is recommended to use callback methods to get information on the import status.
        /// </summary>
        public bool isAsynchronous {
            get { return _isAsynchronous; }
            set { if (_hasStarted) { throw new AccessViolationException("Can only set Importer properties before it runs"); } _isAsynchronous = value; } }

        private bool _printMessageOnCompletion = false;
        /// <summary>
        /// [Default is True]
        /// If set to true, the importer will print a message in the console on import completion.
        /// </summary>
        public bool printMessageOnCompletion {
            get { return _printMessageOnCompletion; }
            set { if (_hasStarted) { throw new AccessViolationException("Can only set Importer properties before it runs"); } _printMessageOnCompletion = value; }
        }

        private ImportSettings _importSettings;
        /// <summary>
        /// Returns the ImportSettings reference 
        /// </summary>
        public ImportSettings importSettings { get { return _importSettings; } set { if (_hasStarted) { throw new AccessViolationException("Can only set Importer properties before it runs"); } _importSettings = value; } }
        
        public SubProcess preprocess => GetPreprocess(filePath);

        public SubProcess postprocess => GetPostprocess(filePath);
        

        private ImportStamp _importStamp;
        public ImportStamp importedModel => _importStamp;

        //public int polycount => _sceneConverter.PolyCount;
        //public int gameObjectCount => _sceneConverter.ObjectCount;

        private ImportSettings _importSettingsCopy;

        //private Native.SceneExtract _scene;
        Native.Scene.PackedTree _sceneTree;

        private SceneTreeConverter _sceneTreeConverter;

        //private SceneExtractToUnity _sceneConverter;
        private bool _hasStarted = false;
        private string _seed;
        Native.Core.Ident _root;
        public Native.Core.Ident root { get { return _root; } set { _root = value; } }

        const string CANCEL_MESSAGE = "Import has been canceled.";

        public Importer(string file, ImportSettings importSettings) {

            filePath = file;
            if (importSettings)
                this.importSettings = importSettings;
            else
                this.importSettings = ScriptableObject.CreateInstance<ImportSettings>();

            if (!File.Exists(file)) {
                throw new Exception($"File '{file}' does not exist");
            }
            if (!Plugin4Unity.Formats.IsFileSupported(file)) {
                throw new Exception($"File '{file}' is not supported by Pixyz");
            }
        }

        /// <summary>
        /// Starts importing the file. Can be executed only once per Importer instance.
        /// </summary>
        public void run() {
            try {
                if(!Native.NativeInterface.CheckLicense())
                    throw new NoValidLicenseException();
                
                if (!OnImportStart())
                    return;

                RunningInstance = this;
                Dispatcher.StartCoroutine(runCoreCommands());
            } catch (Exception exception) {
                reportImportProgressed(1f, "Exception");
                invokeFailed(exception);
                Debug.LogException(exception);
            }
        }
        internal static bool OnImportStart()
        {
            bool isCallerIdentified = false;
            var stackTrace = new System.Diagnostics.StackTrace();
            try
            {
                for (int i = 2; i < 10; i++)
                {
                    MethodBase methodBase = stackTrace.GetFrame(i).GetMethod();
                    string typeName = methodBase.DeclaringType.Name;
                    string methodName = methodBase.Name;
                    //UnityEngine.Debug.Log($"TYPE:{typeName}, METHOD:{methodName}");
                    if (isCallerIdentified = typeName == "ScriptedImporter" && methodName == "OnImportAsset")
                        break;
                    if (isCallerIdentified = typeName == "ImportWindow" && methodName == "OnImportClicked")
                        break;
                }
            }
            catch { }
            if (!isCallerIdentified)
            {
                Debug.LogError(new OutOfTermsException().Message);
                return false;
            }

            return true;
        }

        delegate uint ImportFileDelegate(string file);

        private bool checkIfCanceled() {
            if (_isStopped) {
                Native.NativeInterface.ResetSession();
                reportImportProgressed(1f, CANCEL_MESSAGE);
                Debug.LogWarning(CANCEL_MESSAGE);
                return true;
            }
            return false;
        }
                
    /// <summary>
    /// Read the file in the Pixyz Core native assemblies
    /// </summary>
    private IEnumerator runCoreCommands() {

//#if UNITY_EDITOR
//            UnityEditor.EditorPrefs.SetBool("kAutoRefresh", false);
//#endif

            if (_hasStarted) {
                throw new Exception("An Importer instance can only import once. Please create a new Importer instance to import another file.");
            }

            _hasStarted = true;
            _stopwatch = new System.Diagnostics.Stopwatch();
            _stopwatch.Start();

            _seed = UnityEngine.Random.Range(0, int.MaxValue).ToString();

            Profiling.Start("Importer " + _seed);
            reportImportProgressed(0f, "Initializing...");

            /// Freezing the ImportSettings, and eventually change some settings depending on context
            /// For example,
            /// - Ensures it's not making LODs and doing some tree processing if file is .pxz
            /// - Ensures it's not loading metadata if there is some tree processing
            /// - Loads default shader if not override shader is specified
            _importSettingsCopy = UnityEngine.Object.Instantiate(importSettings).Verify(GetSettingsTemplate(filePath));

            if (_importSettingsCopy.shader == null) {
                _importSettingsCopy.shader = ShaderUtilities.GetDefaultShader();
            }

            if (isAsynchronous)
                yield return Dispatcher.GoThreadPool();

            /// This part runs in another thread to keep the Editor/App smooth and improve performances.
            try {
                
                /// Assembly
                reportImportProgressed(0.10f, "Reading file...");
                Native.NativeInterface.SetPixyzMainThread();
                Native.NativeInterface.ResetSession();

                if (checkIfCanceled())
                    yield break;

                //Native.NativeInterface.ImportFile();
                //Import File
                _root = 0;
                string ext = filePath.Substring(filePath.LastIndexOf('.')).ToLower();

                NativeInterface.PushAnalytic("ImportFormat", ext);
                if (ext == ".pxz")
                {
                    //Load no script
                    NativeInterface.LoadNoScript(filePath);
                    _root = NativeInterface.GetRootOccurrence();
                }
                else
                {
                    _root = Native.NativeInterface.ImportSceneNoScript(filePath, _root);
                }

                if (checkIfCanceled())
                    yield break;

                
                if (preprocess != null) {
                    try {
                        reportImportProgressed(0.30f, "Pre-processing...");
                        preprocess.run(this);
                    } catch (Exception exception) {
                        Debug.LogError("An exception occurred in the pre-process : " + exception);
                        yield break;
                    }
                }
                

                reportImportProgressed(0.40f, "Processing...");

                //var settings = _importSettingsCopy.ToInterfaceObject(GetSettingsTemplate(filePath));
                //var rootChild = Native.NativeInterface.RunAutomaticProcess(settings);
                //Run automatic process
                _root = AutomaticImportProcess(_root, _importSettingsCopy);

                if (checkIfCanceled())
                    yield break;

                /// Scene
                reportImportProgressed(0.60f, "Extracting...");
                _sceneTree = NativeInterface.GetCompleteTree(_root, Native.Scene.VisibilityMode.Inherited);                

            } catch (Exception coreException) {
                /// An exception has occured in the Core
                Native.NativeInterface.ResetSession();
                reportImportProgressed(1f, "Core exception");
                invokeFailed(coreException);
                Debug.LogException(coreException);
                yield break;
            }

            if (_isStopped) {
                reportImportProgressed(1f, CANCEL_MESSAGE);
                Debug.LogWarning(CANCEL_MESSAGE + " before creating objects");
                yield break;
            }

            reportImportProgressed(0.70f, "Converting...");
            if (isAsynchronous)
                yield return Dispatcher.GoMainThread();

            /// Running this Part in the main Unity thread.
            reportImportProgressed(0.80f, "Creating objects...");
            try {
                /// Converting the Scene from Core data to Unity data structure
                _sceneTreeConverter = new SceneTreeConverter(_sceneTree, filePath, _importSettingsCopy, finalize);
                _sceneTreeConverter.convert();
                
            } catch (Exception sceneConversionException) {
                /// An exception has occured while trying to convert the Scene
                reportImportProgressed(1f, "Extraction exception");
                invokeFailed(sceneConversionException);
                Debug.LogException(sceneConversionException);
            }
        }

        private void finalize() {

            if (checkIfCanceled())
                return;

            try {
                GameObject gameObject =  _sceneTreeConverter.gameObject;

                /// Sets LatestModelImported (useful for RuleEngine or any other script running after an import)
                TimeSpan time = Profiling.End("Importer " + _seed);
                _stopwatch.Stop();

                /// Recreating ImportedModel stamp
                _importStamp = gameObject.AddComponent<ImportStamp>();
                _importStamp.stamp(filePath, elaspedTicks);
                _importStamp.importSettings = _importSettingsCopy;

                _LatestModelImportedObject = _importStamp;

                
                /// Post-Process
                if (postprocess != null) {
                    try {
                        reportImportProgressed(0.90f, "Post-processing...");
                        postprocess.run(this);
                    } catch (Exception exception) {
                        Debug.LogError("An exception occurred in the post-process : " + exception);
                    }
                }
                
                /// Import is finished. Sets progress to 100% and runs callbackEnded.
                reportImportProgressed(1f, "Done !");
                if (printMessageOnCompletion)
                    BaseExtensions.LogColor(UnityEngine.Color.green, $"Pixyz Import > File imported in {time.FormatNicely()}");

                invokeCompleted(gameObject);

            } catch (Exception exception) {
                reportImportProgressed(1f, "Finalization exception");
                invokeFailed(exception);
                Debug.LogException(exception);
            }

            clear();
        }

        private void invokeFailed(Exception exception)
        {
          RunningInstance = null;
        //#if UNITY_EDITOR
        //            UnityEditor.EditorPrefs.SetBool("kAutoRefresh", true);
        //#endif
        failed?.Invoke(exception);
        }

        private void invokeCompleted(GameObject gameObject)
        {
            RunningInstance = null;
//#if UNITY_EDITOR
//            UnityEditor.EditorPrefs.SetBool("kAutoRefresh", true);
//#endif
            completed?.Invoke(gameObject);
        }

        private void reportImportProgressed(float progress, string message) {
            progressed?.Invoke(progress, message);
        }

        private void clear() {
            _sceneTree = new Native.Scene.PackedTree();
            _sceneTreeConverter = null;
        }

        private bool _isStopped = false;
        public bool isStopped => _isStopped;

        public void stop() {
            _isStopped = true;
        }
    
        private Ident AutomaticImportProcess(Ident root, ImportSettings settings)
        {
            string ext = filePath.Substring(filePath.LastIndexOf('.')+1).ToLower();
            //Create occurrenceList
            Native.Scene.OccurrenceList rootList = new Native.Scene.OccurrenceList(1);
            rootList[0] = NativeInterface.GetRootOccurrence();

            reportImportProgressed(0.5f, "Processing imported model");
            if (settings.importPoints)
            {
                if (ext == "e57" || ext == "pts" || ext == "ptx" || ext == "xyz" || ext == "rcp")
                {
                    var bounds = NativeInterface.GetAABB(rootList);

                    if (settings.voxelizeGridSize > 1)
                    {
                        double voxelGridSize = Math.Max(1, Math.Min(80, settings.voxelizeGridSize));
                        double voxelSize = Math.Pow((bounds.high.x - bounds.low.x) * (bounds.high.y - bounds.low.y) * (bounds.high.z - bounds.low.z), 1.0 / 3.0) / voxelGridSize;
                        NativeInterface.MergeByTreeLevel(rootList, 2, Native.Scene.MergeHiddenPartsMode.Destroy);
                        NativeInterface.VoxelizePointClouds(rootList, voxelSize);
                    }

                    if(settings.hasLODs && settings.qualities.lods.Length > 1)
                    {
                        List<int> qualities = new List<int>();
                        for (int i=0;i< settings.qualities.lods.Length;i++)
                        {
                            if (settings.qualities.lods[i].quality == LODTools.LodQuality.CULLED)
                                continue;

                            qualities.Add((int)settings.qualities.lods[i].quality);
                        }
                            
                        //Generate LOD
                        NativeInterface.GenerateLOD(rootList,  new IntList(qualities.ToArray()));
                    }
                    else
                    {
                        //Decimating point cloud
                        double voxelSizePointCloud = Mathf.Pow(((float)bounds.high.x - (float)bounds.low.x) * ((float)bounds.high.y - (float)bounds.low.y) * ((float)bounds.high.z - (float)bounds.low.z), 1.0f / 3.0f);
                        double splatSize = 0.0035;
                        double distance = Mathf.Max(0.002f * (float)voxelSizePointCloud, (float)(voxelSizePointCloud * splatSize)) / settings.qualities.quality.threshold;

                        if(settings.qualities.quality.quality != LODTools.LodQuality.MAXIMUM)
                            NativeInterface.DecimatePointClouds(rootList, distance);
                    }
                    return root;
                }
            }
            else
                NativeInterface.DeleteFreeVertices(rootList);

            if(settings.mergeFinalLevel)
            {
                NativeInterface.MergeFinalLevel(rootList, Native.Scene.MergeHiddenPartsMode.Destroy, true);
            }

            if(!settings.importLines)
            {
                NativeInterface.DeleteLines(rootList);
            }

            if (settings.repair)
            {
                NativeInterface.RepairCAD(rootList, 0.1, false);
                NativeInterface.RepairMesh(rootList, 0.1, true, false);
            }

            //Tolerance
            Tolerances tolerances = new Tolerances(settings.qualities.quality.quality);

            if (tolerances.doDecimation)
            {
                //Decimating (if original model has mesh data)...
                NativeInterface.Decimate(rootList, tolerances.surfacicTolerance, tolerances.lineicTolerance, tolerances.normalTolerance, tolerances.uvTolerance, false);
            }

            //Tessellating (if original model has BREP data)...
            NativeInterface.SetModuleProperty("Tessellate", "GenerateQuads", "False");
            NativeInterface.Tessellate(rootList, tolerances.maxSag, -1, tolerances.maxAngle, true, Native.Algo.UVGenerationMode.NoUV, 1, 0, false, false, false, false);

            if (settings.repairInstances)
            {
                NativeInterface.CreateInstancesBySimilarity(rootList, 00.99, 0.99, false, true, true);
            }

            if(settings.orient)
            {
                NativeInterface.RepairMesh(rootList, 0.1, true, true);
            }
            else if(settings.repair)
            {
                NativeInterface.RepairMesh(rootList, 0.1, true, false);
            }
            
            bool isOccurrenceSingle = NativeInterface.GetOccurrenceChildren(root).list.Length==0;

            NativeInterface.MergeMaterials();

            switch (settings.treeProcess)
            {
                case TreeProcessType.TRANSFER_ALL_UNDER_ROOT:
                    NativeInterface.Rake(root, true);
                    break;
                case TreeProcessType.CLEANUP_INTERMEDIARY_NODES:
                    root = NativeInterface.Compress(root);
                    break;
                case TreeProcessType.MERGE_ALL:
                    NativeInterface.MergeByTreeLevel(rootList, 1, Native.Scene.MergeHiddenPartsMode.Destroy);
                    root = NativeInterface.GetOccurrenceChildren(rootList[0])[0];
                    break;
                 case TreeProcessType.MERGE_BY_MATERIAL:
                    NativeInterface.MergeByTreeLevel(rootList, 1, Native.Scene.MergeHiddenPartsMode.Destroy);
                    NativeInterface.ExplodePartByMaterials(rootList);
                    root = NativeInterface.GetOccurrenceChildren(rootList[0])[0];
                    break;
                default:
                    break;
                
            }

            //LOD management
            if(settings.hasLODs)
            {
                if (settings.qualities.lods.Length > 1)
                {
                    List<int> qualities = new List<int>();
                    for (int i = 0; i < settings.qualities.lods.Length; i++)
                    {
                        if (settings.qualities.lods[i].quality == LODTools.LodQuality.CULLED)
                            continue;

                        qualities.Add((int)settings.qualities.lods[i].quality);
                    }

                    //Generate LOD
                    NativeInterface.GenerateLOD(rootList, new IntList(qualities.ToArray()));
                }
            }
            //---------------
            if(settings.importPatchBorders)
            {
                NativeInterface.CreateFreeEdgesFromPatches(rootList);
            }

            if(settings.combinePatchesByMaterial)
            {
                NativeInterface.DeletePatches(rootList, true);
            }

            NativeInterface.CreateNormals(rootList, -1, false, true);

            if(settings.splitTo16BytesIndex)
            {
                NativeInterface.ExplodeVertexCount(rootList, 65534, 65534, false);
            }

            if(settings.mapUV3dSize >= 0)
            {
                NativeInterface.MapUvOnAABB(rootList, false, settings.mapUV3dSize, 0, true);

                NativeInterface.CreateTangents(rootList, -1, 0, false);
            }

            if(settings.lightmapResolution > 0)
            {
                // Compute lightmap uvs in one UV space per part. Probably slower but more logical. However Unity scales himself lightmap uvs for each part...
                NativeInterface.MapUvOnAABB(rootList, false, 1, 1, true);
                NativeInterface.RepackUV(rootList, 1, false, settings.lightmapResolution, settings.uvPadding, false, 3, false);
                NativeInterface.NormalizeUV(rootList, 1, -1, true, false, false);
            }

            if (settings.singularizeSymmetries)
            {
                NativeInterface.RemoveSymmetryMatrices(root);
            }

            NativeInterface.TransferCADMaterialsOnPartOccurrences(root);

            NativeInterface.ResetPartTransform(root);
            //NativeInterface.Save("C:/temp/debug_plugin.pxz");

            if (ext =="pxz")
            {
                return root;
            }
            else
            {
                return root;//set matrix to id
            }

        }

    
    }

   
}