using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Pixyz.Commons.Editor;
using Pixyz.Plugin4Unity;
using Pixyz.Plugin4Unity.Core;
using Pixyz.Plugin4Unity.EditorWindows;

namespace Pixyz.Commons.Extensions.Editor
{
    public enum ColorType { Highlight, Active }

    /// <summary>
    /// Extension methods for Unity Editor
    /// </summary>
    public static class EditorExtensions
    {

        private static Dictionary<ScriptableObject, UnityEditor.Editor> _Editors = new Dictionary<ScriptableObject, UnityEditor.Editor>();

        public static E GetEditor<E>(this ScriptableObject scriptableObject) where E : UnityEditor.Editor
        {
            if (scriptableObject == null)
                return null;

            if (!_Editors.ContainsKey(scriptableObject))
            {
                _Editors.Add(scriptableObject, UnityEditor.Editor.CreateEditor(scriptableObject, typeof(E)) as E);
            }

            if (_Editors[scriptableObject] == null || _Editors[scriptableObject].serializedObject.targetObject == null)
            {
                _Editors[scriptableObject] = UnityEditor.Editor.CreateEditor(scriptableObject, typeof(E)) as E;
            }

            return (E)_Editors[scriptableObject];
        }
        /// <summary>
        /// Creates an instance of a ScriptableObject, as well as creating an Asset for it at the active object location
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T CreateAsset<T>(string assetName = null, bool focusOnSave = true) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            asset.SaveAsset(assetName, focusOnSave);
            return asset;
        }

        public static void DisplaySeparator(string separatorName, bool indent = false)
        {
            EditorGUILayout.BeginHorizontal();

            Vector2 labelSize = EditorStyles.boldLabel.CalcSize(new GUIContent(separatorName));
            EditorGUILayout.LabelField(separatorName, EditorStyles.boldLabel, GUILayout.Width(labelSize.x + (indent ? 15.0f : 0.0f)));

            //Rect rLast = GUILayoutUtility.GetLastRect();

            //EditorGUILayout.BeginVertical();
            //Rect r = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth - (rLast.width + rLast.x) - 15, 1.0f);
            //
            //if(Event.current.type == EventType.Repaint)
            //    EditorGUI.DrawRect(r, Color.grey);
            //
            //DrawUILine(Color.grey, 1, (int)(labelSize.x + 15));
            //EditorGUILayout.EndVertical();
            HorizontalLine(Color.grey);
            //EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.EndHorizontal();
        }

        public static bool CustomFoldout(bool state)
        {
            Color c = GUI.backgroundColor;
            GUI.backgroundColor = Color.clear;

            char arrow = state ? '\u25bc' : '\u25B6';
            if (GUILayout.Button(arrow.ToString(), GetBtnStyle(), GUILayout.Width(20), GUILayout.Height(20)))
            {
                state = !state;
            }
            GUI.backgroundColor = c;

            return state;
        }

        private static GUIStyle GetBtnStyle()
        {
            var s = new GUIStyle(GUI.skin.box);
            s.fontSize = 10;
            s.alignment = TextAnchor.MiddleCenter;
            s.margin = new RectOffset(0, 0, 2, 0);
            return s;
        }

        static void HorizontalLine(Color color)
        {
            GUIStyle horizontalLine;
            horizontalLine = new GUIStyle();
            horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
            horizontalLine.margin = new RectOffset(0, 0, 15, 4);
            horizontalLine.fixedHeight = 1;

            var c = GUI.color;
            GUI.color = color;
            GUILayout.Box(GUIContent.none, horizontalLine);
            GUI.color = c;
        }

        public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        /// <summary>
        /// Opens a browser to select a file
        /// </summary>
        /// <returns></returns>
        public static string SelectFile(string[] filter) {
            if (!Configuration.CheckLicense())
            {
                if (EditorUtility.DisplayDialog("Pixyz Warning", "A Pixyz Plugin license is required", "Open License Manager", "Close"))
                {
                    OpenWindow<LicensingWindow>();
                }
                return null;
            }

            string file = EditorUtility.OpenFilePanelWithFilters("Select File", "", filter);
            if (string.IsNullOrEmpty(file))
                return null;

            if (!File.Exists(file))
                throw new FileNotFoundException();

            return file;
        }

        /// <summary>
        /// Creates an instance of a ScriptableObject, as well as creating an Asset for it at the active object location
        /// </summary>
        /// <typeparam name="TScriptableObject"></typeparam>
        public static TScriptableObject CreateAsset<TScriptableObject>(string assetName = null) where TScriptableObject : ScriptableObject {
            TScriptableObject asset = ScriptableObject.CreateInstance<TScriptableObject>();
            asset.SaveAsset(assetName, true);
            return asset;
        }

        /// <summary>
        /// Saves a ScriptableObject as an Asset.
        /// </summary>
        /// <param name="scriptableObject"></param>
        /// <param name="assetName">Asset name (which will be the file name without .asset extension)</param>
        public static void SaveAsset(this ScriptableObject scriptableObject, string assetName, bool focusOnSave = false, string path = null) {

            // If the assetName isn't given, it will use the Type name instead
            if (string.IsNullOrEmpty(assetName))
                assetName = "New " + scriptableObject.GetType().Name;

            if (string.IsNullOrEmpty(path)) {
                path = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (string.IsNullOrEmpty(path)) {
                    path = "Assets";
                } else if (Path.GetExtension(path) != "") {
                    path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
                }
            }

            string assetPathAndName = $"{path}/{assetName}.asset";
            assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(assetPathAndName);

            AssetDatabase.CreateAsset(scriptableObject, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (focusOnSave) {
                Selection.activeObject = scriptableObject;
                EditorUtility.FocusProjectWindow();
            }
        }

        /// <summary>
        /// Gets the main Unity Editor position and size
        /// </summary>
        /// <returns></returns>
        public static Rect GetEditorMainWindowPos() {
            var containerWinType = AppDomain.CurrentDomain.GetAllDerivedTypes<ScriptableObject>().Where(t => t.Name == "ContainerWindow").FirstOrDefault();
            if (containerWinType == null)
                throw new MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
            var showModeField = containerWinType.GetField("m_ShowMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var positionProperty = containerWinType.GetProperty("position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (showModeField == null || positionProperty == null)
                throw new MissingFieldException("Can't find internal fields 'm_ShowMode' or 'position'. Maybe something has changed inside Unity");
            UnityEngine.Object[] ewindows = Resources.FindObjectsOfTypeAll(containerWinType);
            foreach (var win in ewindows) {
                var showmode = (int)showModeField.GetValue(win);
                if (showmode == 4) // main window
                {
                    var pos = (Rect)positionProperty.GetValue(win, null);
                    return pos;
                }
            }
            throw new NotSupportedException("Can't find internal main window. Maybe something has changed inside Unity");
        }


        public static bool IsSerialized(this UnityEngine.Object unityObject) {
            return !string.IsNullOrEmpty(AssetDatabase.GetAssetPath(unityObject));
        }

        /// <summary>
        /// Centers the given EditorWindow relatively to the main Unity Editor
        /// </summary>
        /// <param name="editorWindow"></param>
        public static void CenterOnEditor(this EditorWindow editorWindow) {
            var main = GetEditorMainWindowPos();
            var pos = editorWindow.position;
            float w = (main.width - pos.width) * 0.5f;
            float h = (main.height - pos.height) * 0.5f;
            pos.x = main.x + w;
            pos.y = main.y + h;
            editorWindow.position = pos;
        }

        /// <summary>
        /// Uses reflection to return a tooltip contained in a TooltipAttribute for a given field. Otherwise, returns null.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        private static TooltipAttribute GetTooltip(FieldInfo field, bool inherit) {
            TooltipAttribute[] attributes = field.GetCustomAttributes(typeof(TooltipAttribute), inherit) as TooltipAttribute[];
            return attributes.Length > 0 ? attributes[0] : null;
        }

        /// <summary>
        /// Creates an Unity prefab from a GameObject and dependencies
        /// </summary>
        /// <param name="gameObject">Root GameObject to save a prefab</param>
        /// <param name="path">Path to the prefab without extension (Asset/..../MyPrefab)</param>
        /// <param name="dependencies">Dependencies to save inside the prefab</param>
        /// <returns></returns>
        public static GameObject CreatePrefab(this GameObject gameObject, string path, IList<UnityEngine.Object> dependencies = null)
        {
            path = path + ".prefab";
            /// Ensures directory exists (recursive)
            string projectPath = Directory.GetParent(Application.dataPath).FullName;
            Directory.CreateDirectory(Path.GetDirectoryName(projectPath + "/" + path));
            if (!Preferences.OverridePrefabs)
            {
                path = AssetDatabase.GenerateUniqueAssetPath(path);
            }
            /// We create prefab
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, path);
            /// We add the dependencies
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    if (dependency != null)
                    {
                        AssetDatabase.AddObjectToAsset(dependency, prefab);
                    }
                }
            }
            /// On resauvegarde le prefab... (pas le choix, sinon les liens vers le dependences sont perdues)
            prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, path, InteractionMode.AutomatedAction);
            AssetDatabase.SaveAssets();

            return prefab;
        }

        public static void CreateAsset<TUnityObject>(this TUnityObject unityObject, string path) where TUnityObject : UnityEngine.Object
        {
            if (unityObject is UnityEngine.Material) {
                path += ".mat";
            } else {
                path += ".asset";
            }

            /// Ensures directory exists (recursive)
            string projectPath = Directory.GetParent(Application.dataPath).FullName;
            Directory.CreateDirectory(Path.GetDirectoryName(projectPath + "/" + path));

            path = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(unityObject, path);
        }

        /// <summary>
        /// Returns an editor window.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="utility">If true, a floating window is created. If false, a normal window is created</param>
        /// <param name="windowTitle"></param>
        /// <returns></returns>
        public static EditorWindow GetEditorWindow(Type type, bool utility, string windowTitle) {
            var window = EditorWindow.GetWindow(type, utility);
            window.titleContent = new GUIContent(windowTitle);
            return window;
        }

        /// <summary>
        /// Opens an editor window
        /// </summary>
        /// <typeparam name="TEditorWindow"></typeparam>
        public static TEditorWindow OpenWindow<TEditorWindow>() where TEditorWindow : SingletonEditorWindow
        {
            var window = EditorWindow.GetWindow(typeof(TEditorWindow), true) as TEditorWindow;
            window.titleContent = new GUIContent(window.WindowTitle);
            window.CenterOnEditor();
            window.Show();
            return window;
        }

        ///// <summary>
        ///// Returns all non persistent (also called volatile) dependencies from a given GameObject and it's children recursively.
        ///// </summary>
        ///// <param name="gameObject"></param>
        ///// <returns></returns>
        public static UnityEngine.Object[] GetNonPersistentDependenciesRecursive(this GameObject gameObject)
        {
            var gameObjects = gameObject.GetChildren(true, true);
            var dependencies = new HashSet<UnityEngine.Object>();

            foreach (var child in gameObjects)
            {
                GetNonPersistentDependencies(child, ref dependencies);
            }

            return dependencies.ToArray();
        }

        /// <summary>
        /// Returns all non persistent (also called volatile) dependencies from given GameObjects.
        /// </summary>
        /// <param name="gameObjects"></param>
        /// <returns></returns>
        public static void GetNonPersistentDependencies(this GameObject gameObject, ref HashSet<UnityEngine.Object> dependencies)
        {
            var components = gameObject.GetComponents<Component>();
            foreach (var component in components)
            {
                SerializedObject objSO = new SerializedObject(component);
                SerializedProperty property = objSO.GetIterator();

                do {
                    if (property.propertyType != SerializedPropertyType.ObjectReference
                        || !property.objectReferenceValue)
                        continue;

                    switch (property.objectReferenceValue) {
                        case Material material:
                            Shader shader = material.shader;
                            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++) {
                                if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv) {
                                    Texture texture = material.GetTexture(ShaderUtil.GetPropertyName(shader, i));
                                    if (texture && !AssetDatabase.Contains(texture)) {
                                        dependencies.Add(texture);
                                    }
                                }
                            }
                            if (!AssetDatabase.Contains(material))
                            {
                                dependencies.Add(material);
                            }
                            break;
                        case Mesh mesh:
                            if (!AssetDatabase.Contains(mesh)) {
                                dependencies.Add(mesh);
                            }
                            break;
                        default:
                            break;
                    }
                } while (property.Next(true));
            }
        }
    }
}