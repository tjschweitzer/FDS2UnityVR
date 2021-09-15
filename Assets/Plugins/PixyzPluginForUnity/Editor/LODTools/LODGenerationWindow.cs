using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Pixyz.OptimizeSDK;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.Utilities.Editor;
using Pixyz.Plugin4Unity;

namespace Pixyz.LODTools.Editor
{
    public class LODGenerationWindow : EditorWindow
    {
        [System.Serializable]
        private class LODPreBuild : IDisposable
        {
            public bool foldout = false;

            public LODProcessInspector processEditor = null;
            public LODGenerationData pastGeneration = null;
            public LODProcess buildProcess = null;
            public LODBuilder builder = new LODBuilder();

            public List<GameObject> gos = new List<GameObject>();
            public List<Renderer> srcRenderers = new List<Renderer>();

            public LODPreBuild(GameObject go)
            {
                gos = new List<GameObject>() { go };
                buildProcess = LODProcess.CreateInstance(true);
            }

            public LODPreBuild(LODGenerationData data)
            {
                pastGeneration = data;
                buildProcess = data.ProcessUsed;
            }

            public LODPreBuild()
            {
                gos = new List<GameObject>();
            }

            public void Dispose()
            {
                if (!AssetDatabase.IsMainAsset(buildProcess) && pastGeneration == null)
                    GameObject.DestroyImmediate(buildProcess);
            }
        }

        [SerializeField]
        private bool _useSelectionAsOne = true;

        [SerializeField]
        private List<LODPreBuild> _preBuilds = new List<LODPreBuild>();

        [SerializeReference]
        private LODPreBuild _currentEditing = null;

        [SerializeReference]
        private LODPreBuild _mainPrebuild = null;
         
        [SerializeField]
        private Vector2 _scrollPos = Vector2.zero;

        [SerializeField]
        private Queue<LODPreBuild> _buildQueue = new Queue<LODPreBuild>();

        private bool _generationInProgress = false;

        [MenuItem("Pixyz/LOD/LOD Generation (Preview)", false, 121)]
        public static void Open()
        {
            var window = GetWindow<LODGenerationWindow>();
            window.titleContent = new GUIContent("LOD Generation (Preview)");
            window.OnSelectionChange();
            window.Focus();
            window.Repaint();
            EditorGUIExtensions.dirtyChanged.AddListener(() => { window.Repaint(); });
        }

        public void OnEnable()
        {
            _mainPrebuild = null;
            _currentEditing = null;
            _preBuilds = new List<LODPreBuild>();
            _buildQueue = new Queue<LODPreBuild>();
        }

        private void UpdateProgressBar()
        {
            if (_generationInProgress)
            {
                LODPreBuild preBuild = _buildQueue.Peek();
                string stepName = "";
                int total = preBuild.buildProcess.Rules.Count;
                int current = preBuild.builder.currentIndex;
                if (preBuild.pastGeneration != null)
                {
                    stepName = preBuild.pastGeneration.name;
                }
                else if (preBuild.gos.Count > 1)
                {
                    stepName = "Multiple objects LODGroup";
                }
                else
                {
                    stepName = preBuild.gos[0].name;
                }
                float progress = ((float)current - 1f) / (float)total;

#if UNITY_2020
                Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(300), GUILayout.Height(15));
                r.position += new Vector2(0, 2);

                EditorGUI.DrawRect(r, new Color(0.3f, 0.3f, 0.3f));
                EditorGUI.DrawRect(new Rect(r.position, new Vector2(r.size.x * progress, r.size.y)), new Color(0.0f, 0.65f, 0.0f));

                var centeredStyle = GUI.skin.GetStyle("Label");
                centeredStyle.alignment = TextAnchor.UpperCenter;
                centeredStyle.normal.textColor = Color.white;
                r.position -= new Vector2(1, 2);
                r.size += new Vector2(0, 2);
                EditorGUI.LabelField(r, $"{stepName} - Generating LOD{current}", centeredStyle);
#else
                EditorUtility.DisplayProgressBar(stepName, $"Generating LOD{current}", progress);
#endif
            }
        }

        public void OnGUI()
        {
            // DRAW BACKGROUND PIXYZ BACKGROUND
            GUI.DrawTexture(new Rect(position.width/2 - 250, position.height/2 - 150, 500, 500), TextureCache.GetTexture("IconPixyzWatermark"));

            EditorGUILayout.HelpBox($"{Selection.gameObjects.Length} gameObject selected - {_preBuilds.Count} LOD generation planned", MessageType.Info);
            
            EditorGUI.BeginChangeCheck();

            _useSelectionAsOne = EditorGUILayout.ToggleLeft("Use the selection as one group", _useSelectionAsOne);

            if(EditorGUI.EndChangeCheck())
            {
                if(_useSelectionAsOne)
                {
                    ReGroupPreBuilds();
                }
                else
                {
                    SingularizePreBuild();
                }
                _currentEditing = null;
            }
            
            EditorGUILayout.Space();

            Rect r = GUILayoutUtility.GetLastRect();

            _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, false, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Height(position.size.y - (r.position.y + r.size.y) - 125), GUILayout.Width(position.width));

            EditorGUILayout.Space();

            foreach (LODPreBuild preBuild in _preBuilds)
            {
                DisplayPreBuild(preBuild);
            }


            EditorGUILayout.EndScrollView();

            if(_preBuilds.Count > 0)
            {

                EditorGUILayout.BeginHorizontal();

                GUILayout.FlexibleSpace();

                UpdateProgressBar();

                EditorGUI.BeginDisabledGroup(_generationInProgress);
                if (GUILayout.Button("Generate All", GUILayout.Width(125)))
                {
                    GenerateLODs(_preBuilds);
                }

                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();
            }
        }

        private void DisplayPreBuild(LODPreBuild prebuild)
        {
            EditorGUILayout.BeginVertical("HelpBox");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.BeginHorizontal();

            if (prebuild.pastGeneration != null)
            {
                prebuild.foldout = EditorGUILayout.Foldout(prebuild.foldout, $"LOD Process -  {prebuild.pastGeneration.gameObject.name}: {(prebuild.pastGeneration.IsDirty() ? "out of date" : "up to date")}");
            }
            else
            {
                prebuild.foldout = EditorGUILayout.Foldout(prebuild.foldout, $"LOD Process - {(prebuild.gos.Count == 1 ? prebuild.gos[0].name : ($"{prebuild.gos.Count.ToString()} objects selected"))}");
            }

            if(prebuild.foldout)
            {
                GUILayout.FlexibleSpace();
                EditorGUI.BeginDisabledGroup(_generationInProgress);

                if (GUILayout.Button("Generate", GUILayout.Width(75)))
                {
                    GenerateLODs(new List<LODPreBuild>() { prebuild });
                }

                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal(); 

            if (EditorGUI.EndChangeCheck())
            {
                if(prebuild.foldout)
                {
                    if (_currentEditing != prebuild)
                    {
                        CloseCurrentEditing();
                    }

                    _currentEditing = prebuild;
                    prebuild.processEditor = prebuild.buildProcess.GetEditor<LODProcessInspector>();
                }
            }

            if(prebuild.foldout)
            {
                EditorGUI.BeginDisabledGroup(_buildQueue.Count > 0);

                EditorGUILayout.BeginHorizontal();
                
                ++EditorGUI.indentLevel;
                EditorGUI.BeginChangeCheck();

                LODProcess newProcess = EditorGUILayout.ObjectField(prebuild.buildProcess, typeof(LODProcess), false, GUILayout.Width(250)) as LODProcess;
                bool refreshProcessEditor = false;

                if(EditorGUI.EndChangeCheck())
                {
                    if (newProcess != prebuild.buildProcess)
                    {
                        refreshProcessEditor = true;
                    }
                }

                if(GUILayout.Button("Set to default", GUILayout.Width(100)))
                {
                    newProcess = LODProcess.CreateInstance(true);
                    refreshProcessEditor = true;
                }

                if(refreshProcessEditor)
                {
                    EditorUtility.SetDirty(prebuild.processEditor.serializedObject.targetObject);
                    prebuild.processEditor.serializedObject.ApplyModifiedPropertiesWithoutUndo();

                    prebuild.buildProcess = newProcess;
                    prebuild.processEditor = prebuild.buildProcess.GetEditor<LODProcessInspector>();
                }
                --EditorGUI.indentLevel;

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                GUILayout.Space(50);

                EditorGUILayout.BeginVertical();

                if(prebuild.processEditor == null)
                    prebuild.processEditor = prebuild.buildProcess.GetEditor<LODProcessInspector>();

                prebuild.processEditor.OnInspectorGUI();

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();

                EditorGUI.EndDisabledGroup();
            }

            EditorGUILayout.EndVertical();
        }

        private void OnSelectionChange()
        {
            if (_generationInProgress)
                return;

            GameObject[] gos = Selection.gameObjects;
            List<GameObject> filteredSelection = CleanCurrentPreBuild(gos);

            LODPreBuild mainPreBuild = new LODPreBuild();

            foreach(GameObject go in filteredSelection)
            {
                LODGenerationData pastGeneration = go.GetComponent<LODGenerationData>();
                if(pastGeneration != null)
                {
                    if(pastGeneration.ProcessUsed == null)
                    {
                        Debug.LogWarning(go.name + " - The selected LODGroup doesn't have a process attached, generation is not possible.");
                    }
                    else
                    {
                        _preBuilds.Add(new LODPreBuild(pastGeneration));
                    }
                }
                else
                {
                    Renderer r = go.GetComponentInChildren<Renderer>(true);
                    if (r == null)
                        continue;

                    if(_useSelectionAsOne)
                    {
                        mainPreBuild.gos.Add(go);
                    }
                    else
                    {
                        _preBuilds.Add(new LODPreBuild(go));
                    }
                }
            }

            if (_mainPrebuild == null && mainPreBuild.gos.Count > 0)
            {
                _mainPrebuild = mainPreBuild;
                _mainPrebuild.buildProcess = LODProcess.CreateInstance(true);
                _preBuilds.Add(_mainPrebuild);
            }
            else if(_mainPrebuild != null)
            {
                bool empty = _mainPrebuild.gos.Count == 0;
                _mainPrebuild.gos.AddRange(mainPreBuild.gos);
                if (mainPreBuild.gos.Count > 0 && empty)
                    _preBuilds.Add(_mainPrebuild);

                if (_mainPrebuild.gos.Count == 0 && _currentEditing == _mainPrebuild)
                    CloseCurrentEditing();
            }

            Repaint();
        }
        private void CloseCurrentEditing()
        {
            if (_currentEditing == null)
                return;

            EditorUtility.SetDirty(_currentEditing.processEditor.serializedObject.targetObject);
            _currentEditing.processEditor.serializedObject.ApplyModifiedPropertiesWithoutUndo();

            _currentEditing.foldout = false;
            _currentEditing = null;
        }

        private List<GameObject> CleanCurrentPreBuild(GameObject[] source)
        {
            List<GameObject> filteredList = new List<GameObject>();
            List<LODPreBuild> toDelete = new List<LODPreBuild>();
            foreach(LODPreBuild preBuild in _preBuilds)
            {
                if(preBuild.pastGeneration != null)
                {
                    if(!source.Contains(preBuild.pastGeneration.gameObject))
                    {
                        toDelete.Add(preBuild);
                    }
                    else
                    {
                        filteredList.Add(preBuild.pastGeneration.gameObject);
                    }
                }
                else
                {
                    preBuild.gos = preBuild.gos.Intersect(source).ToList();
                    if (preBuild.gos.Count == 0)
                        toDelete.Add(preBuild);
                    else
                        filteredList.AddRange(preBuild.gos);
                }
            }

            _preBuilds = _preBuilds.Except(toDelete).ToList();
            return source.Except(filteredList).ToList();
        }

        private void SingularizePreBuild()
        {
            List<LODPreBuild> preBuilds = new List<LODPreBuild>();
            foreach(LODPreBuild prebuild in _preBuilds)
            {
                if (prebuild.pastGeneration != null)
                {
                    preBuilds.Add(prebuild);
                }
                else
                {
                    if(prebuild.gos.Count == 1 && _mainPrebuild != prebuild)
                    {
                        preBuilds.Add(prebuild);
                    }
                    else
                    {
                        foreach(GameObject go in prebuild.gos)
                        {
                            preBuilds.Add(new LODPreBuild(go));
                        }
                    }
                }
            }
            if (_mainPrebuild != null)
                _preBuilds.Remove(_mainPrebuild);

            _preBuilds = preBuilds;
        }

        private void ReGroupPreBuilds()
        {
            List<LODPreBuild> preBuilds = new List<LODPreBuild>();

            if(_mainPrebuild != null)
            {
                _mainPrebuild.gos.Clear();

                foreach (LODPreBuild prebuild in _preBuilds)
                {
                    if (prebuild.pastGeneration != null)
                    {
                        preBuilds.Add(prebuild);
                    }
                    else
                    {
                        _mainPrebuild.gos.AddRange(prebuild.gos);
                    }
                }

                if (_mainPrebuild.gos.Count > 0)
                    preBuilds.Add(_mainPrebuild);

                _preBuilds = preBuilds;
            }
            
        }

        private void GenerateLODs(List<LODPreBuild> preBuilds)
        {
            if (_generationInProgress)
                return;

            if (preBuilds.Count == 0)
                return;

            _buildQueue = new Queue<LODPreBuild>(preBuilds);
            _generationInProgress = true;
            Selection.objects = new UnityEngine.Object[0];

            NextGeneration();
        }

        private void GenerateLOD(LODPreBuild preBuild)
        {
            List<Renderer> renderers = new List<Renderer>();
            preBuild.builder.indexChanged.AddListener(UpdateProgressBar);
            try
            {
                if (preBuild.pastGeneration != null)
                {
                    if(preBuild.pastGeneration.SourceRenderers.Count == 0)
                        renderers = new List<Renderer>(preBuild.pastGeneration.GetComponent<LODGroup>().GetLODs()[0].renderers);
                    else
                        renderers = new List<Renderer>(preBuild.pastGeneration.SourceRenderers.Where(r => r != null).ToList());
                }
                else
                {
                    foreach (GameObject go in preBuild.gos)
                    {
                        renderers.AddRange(go.GetComponentsInChildren<Renderer>(true));
                    }
                }

                MeshData data = new MeshData();
                data.renderers = new List<Renderer>(renderers.Count);
                data.meshes = new List<Mesh>(renderers.Count);
                data.materials = new List<Material[]>(renderers.Count);

                for (int i = 0; i < renderers.Count; ++i)
                {
                    Renderer r = renderers[i];
                    if (r is SkinnedMeshRenderer)
                    {
                        data.meshes.Add(((SkinnedMeshRenderer)r).sharedMesh);
                    }
                    else if (r is MeshRenderer)
                    {
                        data.meshes.Add(r.GetComponent<MeshFilter>().sharedMesh);
                    }
                    else
                    {
                        continue;
                    }
                    data.renderers.Add(renderers[i]);
                    data.materials.Add(r.sharedMaterials);
                }
                preBuild.srcRenderers = data.renderers.ToList();

                PixyzContext context = new PixyzContext();

                uint[] meshIds = context.UnityMeshesToPixyzCreate(data);

                preBuild.builder.generationCompleted += GenerationCompleted;

#pragma warning disable CS4014
                preBuild.builder.BuildLOD(context, preBuild.buildProcess);
#pragma warning restore CS4014
            }
            catch (System.Exception e)
            {
                FinishGeneration();
                Debug.LogError(e.Message + "\n" + e.StackTrace);
            }
        }

        private void GenerationCompleted(bool finishWithoutErrors)
        {
            if(!finishWithoutErrors)
            {
                FinishGeneration();
                return;
            }

            try
            {
                LODPreBuild preBuild = _buildQueue.Dequeue();
                LODGroup group = null;
                UnityEngine.LOD[] lods = new UnityEngine.LOD[preBuild.builder.Contexts.Length];
                var thresholds = preBuild.buildProcess.Thresholds;

                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                if (preBuild.pastGeneration == null)
                {
                    GameObject newLodGroupGameobject = new GameObject("Generated LOD Group - " + unixTimestamp);
                    group = newLodGroupGameobject.AddComponent<LODGroup>();

                    preBuild.pastGeneration = newLodGroupGameobject.AddComponent<LODGenerationData>();
                }
                else
                {
                    group = preBuild.pastGeneration.GetComponent<LODGroup>();
                    if (group.lodCount > 0)
                    {
                        UnityEngine.LOD[] srcLods = group.GetLODs();

                        if(srcLods.Length > lods.Length)
                        {
                            lods = new UnityEngine.LOD[srcLods.Length];
                        }

                        Array.Copy(srcLods, lods, srcLods.Length);
                    }
                }

                UnityEngine.LOD lod0 = lods[0];

                if(lod0.renderers != null)
                {
                    foreach (Renderer r in lod0.renderers)
                    {
                        if (r == null)
                            continue;
                        
                        if (preBuild.srcRenderers.Contains(r))
                            continue;

                        GameObject.DestroyImmediate(r.gameObject);
                    }
                }

                if (_currentEditing == preBuild)
                {
                    CloseCurrentEditing();
                }

                if (preBuild == _mainPrebuild)
                {
                    _mainPrebuild = null;
                    List<GameObject> selection = new List<GameObject>(Selection.gameObjects);
                    selection.Add(group.gameObject);

                    Selection.objects = selection.ToArray();
                }

                preBuild.pastGeneration.SetNewGeneration(preBuild.buildProcess, preBuild.srcRenderers);

                lod0.screenRelativeTransitionHeight = (float)thresholds[0];
                
                List<Renderer> newLOD0Renderers = new List<Renderer>();

                foreach (Renderer r in preBuild.srcRenderers)
                {
                    GameObject copiedGameobject = new GameObject("Duplicate - " + r.gameObject.name);
                    copiedGameobject.transform.position = r.transform.position;
                    copiedGameobject.transform.rotation = r.transform.rotation;
                    copiedGameobject.transform.localScale = r.transform.lossyScale;

                    if(r is MeshRenderer)
                    {
                        UnityEditorInternal.ComponentUtility.CopyComponent(r.GetComponent<MeshFilter>());
                        UnityEditorInternal.ComponentUtility.PasteComponentAsNew(copiedGameobject);
                    }

                    UnityEditorInternal.ComponentUtility.CopyComponent(r);
                    UnityEditorInternal.ComponentUtility.PasteComponentAsNew(copiedGameobject);
                    newLOD0Renderers.Add(copiedGameobject.GetComponent<Renderer>());

                    copiedGameobject.transform.parent = preBuild.pastGeneration.transform;
                }

                lod0.renderers = newLOD0Renderers.ToArray();

                lods[0] = lod0;
                
                ++unixTimestamp;

                for (int i = 1; i < preBuild.builder.Contexts.Length; ++i)
                {
                    unixTimestamp += i;

                    PixyzContext context = preBuild.builder.Contexts[i];
                    Renderer[] generatedRenderer = context.PixyzMeshToUnityRenderer(context.pixyzMeshes);

                    UnityEngine.LOD lod = lods[i];

                    if (lod.renderers != null)
                    {
                        foreach (Renderer r in lod.renderers)
                        {
                            if (r == null)
                                continue;

                            PrefabInstanceStatus status = PrefabUtility.GetPrefabInstanceStatus(r.transform);

                            if (status == PrefabInstanceStatus.Connected || status == PrefabInstanceStatus.MissingAsset)
                                r.gameObject.SetActive(false);
                            else
                                GameObject.DestroyImmediate(r.gameObject);

                        }
                    }

                    lod.screenRelativeTransitionHeight = (float)thresholds[i];

                    for (int j = 0; j < generatedRenderer.Length; ++j)
                    {
                        unixTimestamp += j;

                        Renderer r = generatedRenderer[j];
                        r.gameObject.name = $"Generated LOD{i} object - {unixTimestamp}";

                        Matrix4x4 transform = Conversions.ConvertMatrix(context.pixyzMatrices[j]);

                        transform.GetTRS(out Vector3 pos, out Quaternion rot, out Vector3 scale);
                        r.transform.position = pos;
                        r.transform.rotation = rot;
                        r.transform.localScale = scale;

                        r.transform.parent = preBuild.pastGeneration.transform;
                    }
                    lod.renderers = generatedRenderer.ToArray();

                    lods[i] = lod;
                }

                group.SetLODs(lods);

                if(preBuild.gos[0] !=null)
                    preBuild.gos[0].SetActive(false);
                preBuild.builder.Contexts[0].Dispose();
                preBuild.builder = new LODBuilder();
                preBuild.srcRenderers = new List<Renderer>();
            }
            catch(System.Exception e)
            {
                UnityEngine.Debug.LogError($"[LODGenerationError] {e.Message}\n{e.StackTrace}");
                FinishGeneration();
            }

            NextGeneration();
        }

        private void NextGeneration()
        {
            if (_buildQueue.Count > 0)
            {
                GenerateLOD(_buildQueue.Peek());
            }
            else
            {
                FinishGeneration();
            }
        }

        private void FinishGeneration()
        {
            _generationInProgress = false;
            _buildQueue.Clear();
            EditorUtility.ClearProgressBar();
            OnSelectionChange();
        }
    }
}