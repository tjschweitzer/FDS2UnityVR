using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.LODTools;
using Pixyz.LODTools.Editor;

/// <summary>
/// This is very wrong, but at least it shows the point.
/// If this workflow goes more official, we should ask for opening a few APIs
/// This hack consists in reimplementing CustomEditor for ModelImporter (Unity picks this one)
/// Then we call the original Editor when we need classic behaviour, otherwise we add our logic
/// </summary>
[CustomEditor(typeof(ModelImporter))]
public class CustomModelImporterEditor : Editor
{
    public ModelImporter modelImporter => serializedObject.targetObject as ModelImporter;

    private string[] newTabNames = new string[] { "Model", "Rig", "Animation", "Materials", "LODs" };

    // From base editor (using reflection)
    public Editor baseEditor;
    private Action baseApplyRevertGUI;
    private Action baseOnHeaderGUI;
    private Action<UnityEngine.Object> baseInternalSetAssetImporterTargetEditor;
    private Func<bool> baseShouldHideOpenButton;
    private FieldProxy<int> baseActiveTabIndex;
    private FieldProxy<Array> baseTabs;
    private PropertyProxy<object> baseActiveTab;
    private FieldProxy<string[]> baseTabNames;
    private FieldProxy<bool> baseInstantApply;
    private FieldProxy<object> baseAssetEditor;

    private LODProcess lodProcess;

    private void OnEnable()
    {
        // Find protected types
        /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Modules/AssetPipelineEditor/ImportSettings/ModelImporterEditor.cs
        Type modelImporterEditorType = Type.GetType("UnityEditor.ModelImporterEditor, UnityEditor");
        /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Modules/AssetPipelineEditor/ImportSettings/AssetImporterTabbedEditor.cs
        Type assetImporterTabbedEditorType = modelImporterEditorType.BaseType;
        ///https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/GameObjectInspector.cs
        Type gameObjectInspectorType = Type.GetType("UnityEditor.GameObjectInspectorType, UnityEditor");

        // Avoid index errors
        EditorPrefs.SetInt(modelImporterEditorType.Name + "ActiveEditorIndex", 0);

        // Create instance of builtin Editor
        baseEditor = Editor.CreateEditor(modelImporter, modelImporterEditorType);

        // Easy call to reflected methods with delegates
        baseInternalSetAssetImporterTargetEditor = (Action<UnityEngine.Object>)Delegate.CreateDelegate(typeof(Action<UnityEngine.Object>), baseEditor, "InternalSetAssetImporterTargetEditor", true);
        baseApplyRevertGUI = (Action)Delegate.CreateDelegate(typeof(Action), baseEditor, "ApplyRevertGUI", true);
        baseOnHeaderGUI = (Action)Delegate.CreateDelegate(typeof(Action), baseEditor, "OnHeaderGUI", true);
        baseShouldHideOpenButton = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), baseEditor, "ShouldHideOpenButton", true);

        // Easy get/set to reflected properties and fields
        baseTabs = new FieldProxy<Array>(assetImporterTabbedEditorType.GetField("m_Tabs", BindingFlags.NonPublic | BindingFlags.Instance), baseEditor);
        baseActiveTabIndex = new FieldProxy<int>(assetImporterTabbedEditorType.GetField("m_ActiveEditorIndex", BindingFlags.NonPublic | BindingFlags.Instance), baseEditor);
        baseActiveTab = new PropertyProxy<object>(assetImporterTabbedEditorType.GetProperty("activeTab", BindingFlags.Public | BindingFlags.Instance), baseEditor);
        baseTabNames = new FieldProxy<string[]>(assetImporterTabbedEditorType.GetField("m_TabNames", BindingFlags.NonPublic | BindingFlags.Instance), baseEditor);
        baseInstantApply = new FieldProxy<bool>(assetImporterTabbedEditorType.BaseType.GetField("m_InstantApply", BindingFlags.NonPublic | BindingFlags.Instance), baseEditor);
        baseAssetEditor = new FieldProxy<object>(assetImporterTabbedEditorType.BaseType.GetField("m_AssetEditor", BindingFlags.NonPublic | BindingFlags.Instance), baseEditor);

        // Set instant apply to false
        baseInstantApply.Value = false;

        // Assign new tab names (which adds LODs)
        baseTabNames.Value = newTabNames;

        // Assign new tab at the end, just to avoid index errors
        Array newBaseTabsV = Array.CreateInstance(baseTabs.Value.GetType().GetElementType(), baseTabs.Value.Length + 1);
        Array.Copy(baseTabs.Value, newBaseTabsV, newBaseTabsV.Length - 1);
        newBaseTabsV.SetValue(newBaseTabsV.GetValue(0), newBaseTabsV.Length - 1);
        baseTabs.Value = newBaseTabsV;

        // Force active tab to first
        baseActiveTabIndex.Value = 0;
        baseActiveTab.Value = baseTabs.Value.GetValue(0);

        // Set asset editor (mimick InternalSetAssetImporterTargetEditor)
        GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(modelImporter.assetPath);
        if (gameObject)
            baseAssetEditor.Value = Editor.CreateEditor(AssetDatabase.LoadAssetAtPath<GameObject>(modelImporter.assetPath), gameObjectInspectorType);
    }

    public override void OnInspectorGUI()
    {
        if (baseActiveTabIndex == 4)
        {
            serializedObject.Update();

            // Always allow user to switch between tabs even when the editor is disabled, so they can look at all parts
            // of read-only assets
            using (new EditorGUI.DisabledScope(false)) // this doesn't enable the UI, but it seems correct to push the stack
            {
                GUI.enabled = true;
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        int newIndex = GUILayout.Toolbar(4, newTabNames, "LargeButton", GUI.ToolbarButtonSize.FitToContents);
                        if (check.changed)
                        {
                            baseActiveTabIndex.Value = newIndex;
                            EditorPrefs.SetInt("UnityEditor.ModelImporterEditorActiveEditorIndex", newIndex);
                            // Todo : Change active Tab
                            baseActiveTab.Value = baseTabs.Value.GetValue(newIndex);
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
            }

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                lodProcess = EditorGUILayout.ObjectField("LOD Process", lodProcess, typeof(LODProcess), true) as LODProcess;

                if (!lodProcess)
                {
                    //GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(modelImporter.assetPath);
                    //LODGenerationData gdata = go.GetComponent<LODGenerationData>();
                    //if (gdata)
                    //    lodProcess = gdata.ProcessUsed;
                    var splt = modelImporter.userData.Split('_');
                    if (splt.Length == 2)
                    {
                        lodProcess = (LODProcess)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(splt[0]), typeof(LODProcess));
                    }
                }

                if (lodProcess)
                {
                    LODProcessInspector lodEditor = EditorExtensions.GetEditor<LODProcessInspector>(lodProcess);
                    lodEditor.OnInspectorGUI();
                }

                if (check.changed)
                {
                    string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(lodProcess));
                    // Set modelImport dirty to trigger redraw
                    modelImporter.userData = guid + "_" + UnityEngine.Random.Range(10000000, 99999999).ToString();
                    // EditorUtility.SetDirty(modelImporter); // Not working in this case
                }
            }

            baseApplyRevertGUI();

            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            baseEditor.OnInspectorGUI();
        }
    }

    public override bool HasPreviewGUI()
    {
        return baseEditor.HasPreviewGUI();
    }

    public override string GetInfoString()
    {
        return baseEditor.GetInfoString();
    }

    public override GUIContent GetPreviewTitle()
    {
        return baseEditor.GetPreviewTitle();
    }

    public override VisualElement CreateInspectorGUI()
    {
        return baseEditor.CreateInspectorGUI();
    }

    public override void DrawPreview(Rect previewArea)
    {
        baseEditor.DrawPreview(previewArea);
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        baseEditor.OnInteractivePreviewGUI(r, background);
    }

    public override void OnPreviewSettings()
    {
        baseEditor.OnPreviewSettings();
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        baseEditor.OnPreviewGUI(r, background);
    }

    public override bool RequiresConstantRepaint()
    {
        return baseEditor.RequiresConstantRepaint();
    }

    public override void ReloadPreviewInstances()
    {
        baseEditor.ReloadPreviewInstances();
    }

    public override Texture2D RenderStaticPreview(string assetPath, UnityEngine.Object[] subAssets, int width, int height)
    {
        return baseEditor.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    public override bool UseDefaultMargins()
    {
        return baseEditor.UseDefaultMargins();
    }

    protected override void OnHeaderGUI()
    {
        baseOnHeaderGUI();
    }

    protected override bool ShouldHideOpenButton()
    {
        return baseShouldHideOpenButton();
    }
}

public class FieldProxy<T>
{
    public T Value
    {
        // Could be faster with delegates
        get => (T)fieldInfo.GetValue(instance);
        set
        {
            fieldInfo.SetValue(instance, value);
        }
    }

    private FieldInfo fieldInfo;
    private object instance;

    public FieldProxy(FieldInfo fieldInfo, object instance)
    {
        this.fieldInfo = fieldInfo;
        this.instance = instance;
    }

    public static implicit operator T(FieldProxy<T> proxy) => proxy.Value;
}

public class PropertyProxy<T>
{
    public T Value
    {
        // Could be faster with delegates
        get => (T)propInfo.GetValue(instance);
        set
        {
            propInfo.SetValue(instance, value);
        }
    }

    private PropertyInfo propInfo;
    private object instance;

    public PropertyProxy(PropertyInfo propInfo, object instance)
    {
        this.propInfo = propInfo;
        this.instance = instance;
    }

    public static implicit operator T(PropertyProxy<T> proxy) => proxy.Value;
}