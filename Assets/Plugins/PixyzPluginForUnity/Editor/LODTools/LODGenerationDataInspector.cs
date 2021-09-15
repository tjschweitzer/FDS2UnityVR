using UnityEditor;
using UnityEngine;

namespace Pixyz.LODTools.Editor
{
    [CustomEditor(typeof(LODGenerationData))]
    public class LODGenerationDataInspector : UnityEditor.Editor
    {
        [SerializeField]
        private LODGenerationData _data;

        private void OnEnable()
        {
            _data = (LODGenerationData)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Generation process");

            EditorGUI.BeginChangeCheck();
            LODProcess process = EditorGUILayout.ObjectField(_data.ProcessUsed, typeof(LODProcess), false) as LODProcess;

            if(EditorGUI.EndChangeCheck())
            {
                if(process != _data.ProcessUsed)
                {
                    _data.SetNewProcess(process);
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("Source renderers count : " + _data.SourceRenderers.Count);
            EditorGUILayout.LabelField($"Status : {(_data.IsDirty() ? "Generation might be needed" : "Seems Up-to-date")} ({_data.GenerationProcessHash})");

            EditorGUILayout.Space();

            if(GUILayout.Button("Generate"))
            {
                LODGenerationWindow.Open();
            }
        }
    }
}