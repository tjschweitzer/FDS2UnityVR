using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.Styles.Editor;
using Pixyz.Commons.Utilities;
using Pixyz.Commons.Utilities.Editor;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Utils;
using Pixyz.Plugin4Unity.Core;

namespace Pixyz.Toolbox.Editor
{
    public class ToolboxWindow : EditorWindow {

        private static ActionBase lastAction;

        const int TRANSITION_SIZE = 36;
        const int HEADER_HEIGHT = 30;

        private int actionId;

        private ActionBase _action;
        public ActionBase action {
            get {
                if (_action == null)
                    action = ToolsBase.GetRegisteredAction(actionId);
                
                return _action;
            }
            set {
                _action = value;
                if (_action != null)
                    actionId = _action.id;
                _hasProcessed = false; 
            }
        }

        public static void ShowToolbox(ActionBase action) {
            var window = (ToolboxWindow)EditorExtensions.GetEditorWindow(typeof(ToolboxWindow), true, "Toolbox");
            lastAction = (lastAction != null && lastAction.id == action.id) ? lastAction : action;
            window.action = lastAction;
            window.ShowUtility();
            Selection.selectionChanged.Invoke();
        }

        private static bool _initialized = false;
        private ColoredTheme _style;

        private static bool _IncludeChildren = true;

        void OnEnable() {
            _style = new ColoredTheme(new Color(1f, 1f, 1f, 0.5f));
            action.onSelectionChanged(getSelectedGameObjects());
            Selection.selectionChanged += ()=>{ statsBeforeProcess = null; action.onSelectionChanged(getSelectedGameObjects()); Repaint(); _hasProcessed = false; };
        }

        private GUIStyle _marginStyle;
        public GUIStyle marginStyle {
            get {
                if (_marginStyle == null) {
                    _marginStyle = new GUIStyle();
                    _marginStyle.padding = new RectOffset(28, 28, 10, 10);
                }
                return _marginStyle;
            }
        }

        private StatsReport statsBeforeProcess;
        private StatsReport statsAfterProcess;

        void OnGUI()
        {
            EditorGUIUtility.labelWidth = 180;

            if (action == null) {
                Close();
                return;
            }

            if(action.IsRunning)
            {
                EditorUtility.DisplayProgressBar("Pixyz", action.StepName, action.StepProgress);
            }

            titleContent = new GUIContent(action?.displayNameToolbox, action?.tooltip);

            Rect rect = EditorGUILayout.BeginVertical();

            GUILayout.Space(10);

            // DRAW BACKGROUND PIXYZ BACKGROUND
            GUI.DrawTexture(new Rect(position.width - 430, position.height - 430, 500, 500), TextureCache.GetTexture("IconPixyzWatermark"));

            EditorGUI.BeginDisabledGroup(action.IsRunning);

            Rect rect1 = EditorGUILayout.BeginVertical(_style.ruleBlock);
            {
                GUILayout.Label(new GUIContent(_hasProcessed ? "Before Process" : "Selection"), _style.toolboxBlockTitle);
                EditorGUILayout.BeginVertical();
                {
                    if (!_hasProcessed && statsBeforeProcess == null)
                        statsBeforeProcess = getStats(getSelectedGameObjects());
                    EditorGUI.BeginChangeCheck();
                    _IncludeChildren = EditorGUILayout.Toggle("Include Children", _IncludeChildren);
                    if (EditorGUI.EndChangeCheck()) {
                        statsBeforeProcess = getStats(getSelectedGameObjects());
                    }
                    GUILayout.Space(4);
                    drawStats(statsBeforeProcess);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            /// Transition
            GUI.DrawTexture(new Rect(35, rect1.y + rect1.height - 4, TRANSITION_SIZE, TRANSITION_SIZE), (false) ? _style.iconDownFluxMid : _style.iconDownFlux);
            GUI.Label(new Rect(70, rect1.y + rect1.height + (TRANSITION_SIZE - 20) / 2 - 2, TRANSITION_SIZE, 20), "GameObjects", _style.downFluxTitle);
            GUILayout.Space(TRANSITION_SIZE - 8);

            Rect rect2 = EditorGUILayout.BeginVertical(_style.ruleBlock);
            {
                GUILayout.Label(titleContent, _style.toolboxBlockTitle);
                EditorGUILayout.BeginVertical();
                {
                    action.onBeforeDraw();
                    action.fieldInstances.DrawGUILayout();
                    action.onAfterDraw();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            /// Transition
            GUI.DrawTexture(new Rect(35, rect2.y + rect2.height - 4, TRANSITION_SIZE, TRANSITION_SIZE), (false) ? _style.iconDownFluxMid : _style.iconDownFlux);
            GUI.Label(new Rect(70, rect2.y + rect2.height + (TRANSITION_SIZE - 20) / 2 - 2, TRANSITION_SIZE, 20), "GameObjects", _style.downFluxTitle);
            GUILayout.Space(TRANSITION_SIZE - 8);

            EditorGUI.EndDisabledGroup();

            Rect rect3 = EditorGUILayout.BeginVertical(_style.ruleBlock);
            {
                string label = "Process";
                if (_hasProcessed)
                {
                    label = "After Process";
                }
                else if(action.IsRunning)
                {
                    label = "Processing ...";
                }
                GUILayout.Label(new GUIContent(label), _style.toolboxBlockTitle);
                EditorGUILayout.BeginVertical();
                {
                    if (_hasProcessed) {
                        drawStats(statsAfterProcess, statsBeforeProcess);
                        var executionErrors = action.executionErrors;
                        if (executionErrors != null && executionErrors.Count > 0)
                        {
                            EditorGUILayout.HelpBox(String.Join("\n", executionErrors), MessageType.Error);
                        }
                    } 
                    //else {
                        var errors = action.getErrors();
                        var warnings = action.getWarnings();
                        bool enabled = true;

                        if (warnings != null && warnings.Count > 0)
                        {
                            EditorGUILayout.HelpBox(String.Join("\n", warnings), MessageType.Warning);
                        }

                        if (errors != null && errors.Count > 0) {
                            EditorGUILayout.HelpBox(String.Join("\n", errors), MessageType.Error);
                            enabled = false;
                        }

                        if (Selection.gameObjects.Length == 0) {
                            EditorGUILayout.HelpBox("At least one GameObject must be selected", MessageType.Error);
                            enabled = false;
                        }

                        if (action.IsRunning)
                            enabled = false;

                        EditorGUI.BeginDisabledGroup(!enabled);
                        if (GUILayout.Button(action.IsRunning ? "" : "Execute")) {
                            Dispatcher.StartCoroutine(Execute());
                        }

                        EditorGUI.EndDisabledGroup();
#if UNITY_2020
                        Rect refRect = GUILayoutUtility.GetLastRect();
                        Rect r = EditorGUILayout.GetControlRect(GUILayout.Width(20), GUILayout.Height(20));

                        if (refRect.size.x > 1 && refRect.size.y > 1 && action.IsRunning)
                        {
                            Vector2 pos = new Vector2(r.position.x + 3, refRect.position.y + 3);
                            Vector2 size = new Vector2(refRect.size.x - 5, refRect.size.y - 5);
                            Rect newRect = new Rect(pos, size);

                            EditorGUI.DrawRect(new Rect(newRect.position, new Vector2(newRect.size.x * action.StepProgress, newRect.size.y)), new Color(0.0f, 0.65f, 0.0f));

                            var centeredStyle = GUI.skin.GetStyle("Label");
                            centeredStyle.alignment = TextAnchor.UpperCenter;
                            centeredStyle.normal.textColor = Color.white;
                            newRect.position -= new Vector2(1, 2);
                            newRect.size += new Vector2(0, 2);
                            EditorGUI.LabelField(newRect, action.StepName, centeredStyle);
                        }
#endif
                    //}
                }
                    EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            if (rect.height > 20 && _lastHeight != rect.height) {
                _lastHeight = rect.height;

                Rect pos = position;
                pos.width = 360;
                pos.height = rect.height + 10;
                position = pos;
                minSize = new Vector2(pos.width, pos.height);
                maxSize = new Vector2(pos.width, pos.height);

                if (!_initialized && Event.current.type == EventType.Repaint) {
                    _initialized = true;
                    //var ms = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                    //pos.x = ms.x;
                    //pos.y = ms.y;
                    EditorExtensions.CenterOnEditor(this);
                }
            }
        }

        private StatsReport getStats(IList<GameObject> gameObjects) {

            if (gameObjects == null)
                return new StatsReport();

            int triangles = 0;
            int vertices = 0;
            int renderers = 0;

            var meshes = new HashSet<Mesh>();
            var materials = new HashSet<Material>();

            for (int i = 0; i < gameObjects.Count; i++) {

                if (!gameObjects[i])
                    continue;

                gameObjects[i].GetMeshData(out Renderer renderer, out Mesh mesh, out Material[] lmaterials);

                if (mesh != null)
                {
                    meshes.Add(mesh);
                    triangles += mesh.GetPolycount();
                    vertices += mesh.vertices.Length;
                }
                if (lmaterials != null)
                {
                    renderers++;
                    foreach (var mat in lmaterials)
                    {
                        materials.Add(mat);
                    }
                }
            }

            return new StatsReport(
                gameObjects.Count,
                materials.Count,
                meshes.Count,
                renderers,
                triangles,
                vertices
            );
        }

        private void drawStats(StatsReport stats, StatsReport before = null) {
            if (stats == null)
                return;
            EditorGUILayout.LabelField("GameObjects", getFormattedValue(stats.gameObjects, (before != null) ? before.gameObjects : -1));
            GUILayout.Space(4);
            EditorGUILayout.LabelField("Materials", getFormattedValue(stats.materials, (before != null) ? before.materials : -1));
            GUILayout.Space(4);
            EditorGUILayout.LabelField("Shared Meshes", getFormattedValue(stats.uniqueMeshes, (before != null) ? before.uniqueMeshes : -1));
            GUILayout.Space(4);
            EditorGUILayout.LabelField("Renderers", getFormattedValue(stats.renderers, (before != null) ? before.renderers : -1));
            GUILayout.Space(4);
            EditorGUILayout.LabelField("Triangles", getFormattedValue(stats.triangles, (before != null) ? before.triangles : -1));
            GUILayout.Space(4);
            EditorGUILayout.LabelField("Vertices", getFormattedValue(stats.vertices, (before != null) ? before.vertices : -1));
        }

        private string getFormattedValue(int current, int before) {
            string value = current.ToString("# ### ### ##0").Trim();
            if (before != -1) {
                float percent = 100.0f * (current - before) / before;
                if (percent != 0 && !float.IsInfinity(percent) && !float.IsNaN(percent))
                    value += " (" + percent.ToString("+0.##;-0.##;0") + "%)";
            }
            return value;
        }

        private IList<GameObject> getSelectedGameObjects() {
            return _IncludeChildren ? Selection.gameObjects.GetChildren(true, true) : Selection.gameObjects;
        }

        float _lastHeight;
        bool _hasProcessed = false;

        private IEnumerator Execute() {

            Repaint();
            var gos = Selection.gameObjects;

            if (Selection.gameObjects.Length == 0)
                yield break;

            yield return Dispatcher.GoMainThread();

            IList<GameObject> input = getSelectedGameObjects();

            // Unpack (security)
            foreach (var gameObject in input)
            {
                var transformPrefabStatus = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(gameObject);
                if (transformPrefabStatus == UnityEditor.PrefabInstanceStatus.Connected || transformPrefabStatus == UnityEditor.PrefabInstanceStatus.MissingAsset)
                {
                    UnityEditor.PrefabUtility.UnpackPrefabInstance(UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(gameObject), UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);
                }
            }

            foreach (GameObject go in input)
            {
                Undo.RegisterFullObjectHierarchyUndo(go, action.displayNameToolbox);
            }

            try
            {
                EditorUtility.DisplayProgressBar("Pixyz", $"Processing...", 0);
                StartLogTime();

                action.progressChanged.AddListener(Repaint);

                if(!action.invokePreProcess(input))
                {
                    throw new Exception("[ToolBox_Action]Failed to pre process input");
                }

                if(action.isAsync)
                {
                    action.completed.AddListener(ActionAsyncCompleted);
                    Task t = Task.Factory.StartNew(() =>
                    {
                        action.invoke(input);
                        Dispatcher.StartCoroutine(action.postProcessCoroutine());
                    });
                }
                else
                {
                    object result = action.invoke(input);
                    statsAfterProcess = getStats((result as IList<GameObject>)?.ToArray());
                    _hasProcessed = true;
                    EndLogTime();
                    CleanUpAction();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Error] {e.Message} \n {e.StackTrace}");
                action.executionErrors.Add(e.Message);
                CleanUpAction();
            }
        }

        private void ActionAsyncCompleted()
        {
            try
            {
                statsAfterProcess = getStats((action.Output as IList<GameObject>)?.ToArray());
                EndLogTime();
                _hasProcessed = true;
                CleanUpAction();
            }
            catch (Exception ex)
            {
                CleanUpAction();
                Debug.LogException(ex);
            }
        }

        private void CleanUpAction()
        {
            Repaint();
            action.Dispose();
            EditorUtility.ClearProgressBar();
            Undo.FlushUndoRecordObjects();
            Undo.SetCurrentGroupName(action.displayNameToolbox + " (Pixyz)");
        }

        private void StartLogTime()
        {
            if (!Preferences.LogTimeWithToolbox)
                return;

            Profiling.Start("ToolboxAction");
        }

        private void EndLogTime()
        {
            if (!Preferences.LogTimeWithToolbox)
                return;

            var timespan = Profiling.End("ToolboxAction");
            string log = $"Pixyz Toolbox > {action.displayNameToolbox} done in {timespan.FormatNicely()}";
            if (Preferences.LogTimeWithToolbox)
            {
                BaseExtensions.LogColor(Color.green, log);
            }
        }
    }

    internal class StatsReport {
        public readonly int gameObjects;
        public readonly int materials;
        public readonly int uniqueMeshes;
        public readonly int renderers;
        public readonly int triangles;
        public readonly int vertices;
        public StatsReport(int gameObjects, int materials, int uniqueMeshes, int renderers, int triangles, int vertices) {
            this.gameObjects = gameObjects;
            this.materials = materials;
            this.uniqueMeshes = uniqueMeshes;
            this.renderers = renderers;
            this.triangles = triangles;
            this.vertices = vertices;
        }
        public StatsReport() { }
    }
}

