using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Pixyz.Commons.Extensions.Editor;

namespace Pixyz.LODTools.Editor
{
    [CustomEditor(typeof(LODProcess))]
    public class LODProcessInspector : UnityEditor.Editor
    {
        [SerializeField]
        private LODProcess _process = null;

        private void OnEnable()
        {
            if (target == null)
                return;

            _process = (LODProcess)target;
            _process.CheckRulesExistence();

            EditorGUIExtensions.dirtyChanged.AddListener(() => {
                    Repaint();
                }
            );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            using(EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
            {
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rulesThresholds"), new GUIContent("", ""));
                GUILayout.Space(10);

                List<LODRule> deletedRules = new List<LODRule>();

                ++EditorGUI.indentLevel;

                EditorGUILayout.LabelField($"LOD #0", EditorStyles.boldLabel);

                EditorGUI.BeginDisabledGroup(true);

                ++EditorGUI.indentLevel;
                EditorGUILayout.LabelField("Source LOD", EditorStyles.miniBoldLabel);
                --EditorGUI.indentLevel;
                --EditorGUI.indentLevel;

                GUILayout.Space(15);

                EditorGUI.EndDisabledGroup();

                for (int i = 0; i < _process.Rules.Count; ++i)
                {
                    LODRule rule = _process.Rules[i];

                    EditorGUILayout.BeginHorizontal();
                    
                    GUILayout.Space(-10);
                    if (GUILayout.Button("X", GUILayout.Width(25.0f), GUILayout.Height(20.0f)))
                    {
                        deletedRules.Add(rule);
                    }

                    GUILayout.Space(0);

                    EditorGUILayout.LabelField($"LOD #{i + 1}", EditorStyles.boldLabel, GUILayout.Width(75));

                    EditorGUI.BeginChangeCheck();

                    LODRule newRuleToSwap = EditorGUILayout.ObjectField(rule, typeof(LODRule), false, GUILayout.Width(150)) as LODRule;

                    if (EditorGUI.EndChangeCheck())
                    {
                        _process.SwapRule(newRuleToSwap, i);
                    }

                    GUILayout.FlexibleSpace();

                    EditorGUILayout.LabelField($"Source LOD", GUILayout.Width(75));
                    EditorGUI.BeginChangeCheck();

                    int sourceIndex = EditorGUILayout.IntField("", _process.Sources[i], GUILayout.Width(35));

                    if(EditorGUI.EndChangeCheck())
                    {
                        _process.SetLODSource(i, sourceIndex);
                    }

                    EditorGUILayout.EndHorizontal();

                    ++EditorGUI.indentLevel;

                    DisplayLODRule(rule);

                    --EditorGUI.indentLevel;

                    GUILayout.Space(25);
                }

                foreach (LODRule rule in deletedRules)
                {
                    _process.RemoveRule(rule);
                }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Add custom rule", GUILayout.Width(150));
                LODRule newRule = EditorGUILayout.ObjectField(null, typeof(LODRule), false, GUILayout.Width(150)) as LODRule;

                if (newRule != null)
                {
                    double lastThreshold = _process.Thresholds.Count > 0 ? _process.Thresholds[0] : 1;
                    double baseThreshold = _process.Thresholds.Count > 1 ? _process.Thresholds[1] : 0;
                    if(_process.Thresholds.Count > 2)
                    {
                        _process.AddRule(newRule, _process.Thresholds[_process.Thresholds.Count-2] + (_process.Thresholds[_process.Thresholds.Count - 3] - _process.Thresholds[_process.Thresholds.Count - 2]) / 2, 0);
                    }
                    else
                    {
                        _process.AddRule(newRule, (1 + lastThreshold) / 2, 0);
                    }
                    
                }

                GUILayout.Space(50);

                if (GUILayout.Button("Add default", GUILayout.Width(150)))
                {
                    double threshold = 0;
                    int count = _process.Thresholds.Count;
                    if (count <= 2) threshold = 1 - (1 - _process.Thresholds[0]) / 2;
                    else threshold = _process.Thresholds[count - 3] / 2;
                    _process.AddRule(GenerateDefault(), threshold, 0);
                }

                if (check.changed)
                {
                    EditorUtility.SetDirty(target);
                }

                EditorGUILayout.EndHorizontal();
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private void DisplayLODRule(LODRule rule)
        {
            rule.GetEditor<LODRuleInspector>().OnInspectorGUI();
        }

        private LODRule GenerateDefault()
        {
            if(!AssetDatabase.IsMainAsset(_process))
                return LODRule.CreateInstance(_process.Rules.Count + 1);

            string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(target)) + "/New LOD Rule";
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            LODRule rule = LODRule.CreateInstance(_process.Rules.Count + 1);

            EditorExtensions.SaveAsset(rule, Path.GetFileNameWithoutExtension(path), false, Path.GetDirectoryName(path));

            return rule;
        }
    }
}