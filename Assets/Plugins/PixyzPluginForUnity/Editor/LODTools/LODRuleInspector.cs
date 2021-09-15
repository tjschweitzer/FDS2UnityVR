using UnityEditor;
using UnityEngine;
using Pixyz.LODTools;
using Pixyz.OptimizeSDK.Runtime;
using Pixyz.Commons.Extensions.Editor;

namespace Pixyz.LODTools.Editor
{
    [CustomEditor(typeof(LODRule))]
    public class LODRuleInspector : UnityEditor.Editor
    {
        [SerializeField]
        private LODRule _rule;

        [SerializeField]
        private bool isRepairFoldout = false;
        [SerializeField]
        private bool isDecimateFoldout = false;
        [SerializeField]
        private bool isOcclusionFoldout = false;
        [SerializeField]
        private bool isRemeshFoldout = false;
        [SerializeField]
        private bool isCombineFoldout = false;
        [SerializeField]
        private bool isImposterFoldout = false;
        [SerializeField]
        private float _fieldNumberSize = 150;
        [SerializeField]
        private float _toggleSize = 40;
        [SerializeField]
        private float _fieldToggleSize = 150;
        [SerializeField]
        private float _popUpSize = 150;
        [SerializeField]
        private float _labelSize = 185;

        private void OnEnable()
        {
            if (target == null)
                return;

            _rule = (LODRule)serializedObject.targetObject;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
            {
                DisplayRepairParam();
                DisplayRemeshParam();
                DisplayOcculusionParam();
                DisplayDecimate();
                DisplayCombineParam();
                DisplayImposterParam();

                if (check.changed)
                {
                    EditorUtility.SetDirty(target);
                }
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private void DisplayRepairParam()
        {
            EditorGUILayout.BeginHorizontal();

            _rule.isRepairEnable = EditorGUILayout.ToggleLeft("", _rule.isRepairEnable, GUILayout.Width(_toggleSize));

            isRepairFoldout = EditorExtensions.CustomFoldout(isRepairFoldout);

            EditorExtensions.DisplaySeparator("Mesh reparation", true);

            EditorGUILayout.EndHorizontal();

            if (isRepairFoldout)
            {
                EditorGUI.indentLevel += 2;
                if (_rule.isRepairEnable)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Distance tolerance", GUILayout.MaxWidth(_labelSize));
                    _rule.repairParameters.tolerance = EditorGUILayout.DoubleField(_rule.repairParameters.tolerance, GUILayout.MaxWidth(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.LabelField("Vertices within a very small tolerance will be welded in any case");
                }
                EditorGUI.indentLevel -= 2;
                EditorGUILayout.Space();
            }
        }
        private void DisplayImposterParam()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            bool otherOptiActivated = _rule.isCombineMaterialsActivated || _rule.isCombineMeshesActivated || _rule.isDecimateToQualityActivated || _rule.isDecimateToTargetActivated || _rule.isRemeshActivated || _rule.isRemeshFieldAlignedActivated || _rule.isOcclusionActivated;

            EditorGUI.BeginDisabledGroup(otherOptiActivated);
            _rule.isImposterActivated = EditorGUILayout.ToggleLeft("", _rule.isImposterActivated, GUILayout.Width(_toggleSize));

            if (EditorGUI.EndChangeCheck())
            {
                if (_rule.isImposterActivated)
                {
                    _rule.isCombineMaterialsActivated = false;
                    _rule.isCombineMeshesActivated = false;
                    _rule.isDecimateToQualityActivated = false;
                    _rule.isDecimateToTargetActivated = false;
                    _rule.isRemeshActivated = false;
                    _rule.isRemeshFieldAlignedActivated = false;
                    _rule.isOcclusionActivated = false;

                    isCombineFoldout = false;
                    isDecimateFoldout = false;
                    isRemeshFoldout = false;
                    isOcclusionFoldout = false;
                }
                else
                {
                    isImposterFoldout = false;
                }
            }
            EditorGUI.EndDisabledGroup();

            isImposterFoldout = EditorExtensions.CustomFoldout(isImposterFoldout);

            EditorExtensions.DisplaySeparator("Billboard Generation", true);

            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(!_rule.isImposterActivated);

            EditorGUI.indentLevel += 2;

            if (otherOptiActivated)
            {
                EditorGUILayout.LabelField("Other optimizations are active");
            }

            if (isImposterFoldout)
            {

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Maps resolution", GUILayout.MaxWidth(_labelSize));
                _rule.imposterParameters.resolution = EditorGUILayout.IntField(_rule.imposterParameters.resolution, GUILayout.Width(_fieldNumberSize));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("X+", GUILayout.MaxWidth(_labelSize));
                _rule.imposterParameters.XPositiveEnable = EditorGUILayout.ToggleLeft("", _rule.imposterParameters.XPositiveEnable, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("X-", GUILayout.MaxWidth(_labelSize));
                _rule.imposterParameters.XNegativeEnable = EditorGUILayout.ToggleLeft("", _rule.imposterParameters.XNegativeEnable, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Y+", GUILayout.MaxWidth(_labelSize));
                _rule.imposterParameters.YPositiveEnable = EditorGUILayout.ToggleLeft("", _rule.imposterParameters.YPositiveEnable, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Y-", GUILayout.MaxWidth(_labelSize));
                _rule.imposterParameters.YNegativeEnable = EditorGUILayout.ToggleLeft("", _rule.imposterParameters.YNegativeEnable, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Z+", GUILayout.MaxWidth(_labelSize));
                _rule.imposterParameters.ZPositiveEnable = EditorGUILayout.ToggleLeft("", _rule.imposterParameters.ZPositiveEnable, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();


                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Z-", GUILayout.MaxWidth(_labelSize));
                _rule.imposterParameters.ZNegativeEnable = EditorGUILayout.ToggleLeft("", _rule.imposterParameters.ZNegativeEnable, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
            }

            EditorGUI.indentLevel -= 2;

            EditorGUI.EndDisabledGroup();
        }

        private void DisplayCombineParam()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginDisabledGroup(_rule.isImposterActivated);
            bool combineState = EditorGUILayout.ToggleLeft("", _rule.isCombineMaterialsActivated || _rule.isCombineMeshesActivated, GUILayout.Width(_toggleSize));

            if (EditorGUI.EndChangeCheck())
            {
                if (!combineState)
                {
                    _rule.isCombineMaterialsActivated = false;
                    _rule.isCombineMeshesActivated = false;
                    isCombineFoldout = false;
                }
                else
                {
                    _rule.isCombineMaterialsActivated = true;
                    _rule.isImposterActivated = false;
                }
            }

            EditorGUI.EndDisabledGroup();

            isCombineFoldout = EditorExtensions.CustomFoldout(isCombineFoldout);

            EditorExtensions.DisplaySeparator("Combination", true);
            EditorGUILayout.EndHorizontal();


            EditorGUI.BeginDisabledGroup(!_rule.isCombineMaterialsActivated && !_rule.isCombineMeshesActivated);

            if (isCombineFoldout)
            {
                EditorGUI.indentLevel += 2;
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Combine materials", GUILayout.MaxWidth(_labelSize));
                _rule.isCombineMaterialsActivated = EditorGUILayout.ToggleLeft("", _rule.isCombineMaterialsActivated, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();

                if (combineState)
                    _rule.isCombineMeshesActivated = !_rule.isCombineMaterialsActivated;

                if (_rule.isCombineMaterialsActivated)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Re-Generate UV", GUILayout.MaxWidth(_labelSize));
                    _rule.combineMeshesParameters.forceUVGeneration = EditorGUILayout.ToggleLeft("", _rule.combineMeshesParameters.forceUVGeneration, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Maps resolution", GUILayout.MaxWidth(_labelSize));
                    _rule.combineMeshesParameters.resolution = EditorGUILayout.IntField(_rule.combineMeshesParameters.resolution, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Padding", GUILayout.MaxWidth(_labelSize));
                    _rule.combineMeshesParameters.padding = EditorGUILayout.IntField(_rule.combineMeshesParameters.padding, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUI.indentLevel -= 2;
                EditorGUILayout.Space();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DisplayRemeshParam()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();
            EditorGUI.BeginDisabledGroup(_rule.isImposterActivated);

            bool combineState = EditorGUILayout.ToggleLeft("", _rule.isRemeshActivated || _rule.isRemeshFieldAlignedActivated, GUILayout.Width(_toggleSize));

            if (EditorGUI.EndChangeCheck())
            {
                if (!combineState)
                {
                    _rule.isRemeshActivated = false;
                    _rule.isRemeshFieldAlignedActivated = false;
                    isRemeshFoldout = false;
                }
                else
                {
                    _rule.isRemeshActivated = true;

                    _rule.isOcclusionActivated = false;
                    _rule.isImposterActivated = false;
                }
            }

            EditorGUI.EndDisabledGroup();

            isRemeshFoldout = EditorExtensions.CustomFoldout(isRemeshFoldout);

            EditorExtensions.DisplaySeparator("Retopolization", true);
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(!_rule.isRemeshActivated && !_rule.isRemeshFieldAlignedActivated);

            if (isRemeshFoldout)
            {
                EditorGUI.indentLevel += 2;
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", GUILayout.MaxWidth(_labelSize));
                int value = EditorGUILayout.Popup(_rule.isRemeshActivated ? 0 : 1, new GUIContent[] { new GUIContent("Standard"), new GUIContent("Field Aligned") }, GUILayout.Width(_popUpSize));
                EditorGUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck())
                {
                    _rule.isRemeshActivated = value == 0;
                    _rule.isRemeshFieldAlignedActivated = value == 1;
                }

                if (_rule.isRemeshActivated)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Feature size", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshParameters.featureSize = EditorGUILayout.DoubleField(_rule.remeshParameters.featureSize, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Keep animations", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshParameters.transferAnimations = EditorGUILayout.ToggleLeft("", _rule.remeshParameters.transferAnimations, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Bake Maps", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshParameters.bakeMaps = EditorGUILayout.ToggleLeft("", _rule.remeshParameters.bakeMaps, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginDisabledGroup(!_rule.remeshParameters.bakeMaps);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Maps resolution", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshParameters.mapsResolution = EditorGUILayout.IntField(_rule.remeshParameters.mapsResolution, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.EndDisabledGroup();
                }
                else if (_rule.isRemeshFieldAlignedActivated)
                {
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target type", GUILayout.MaxWidth(_labelSize));
                    int ratioSelect = EditorGUILayout.Popup(_rule.remeshFieldAlignedParameters.isTargetCount ? 0 : 1, new GUIContent[] { new GUIContent("Polycount"), new GUIContent("Ratio") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        _rule.remeshFieldAlignedParameters.isTargetCount = ratioSelect == 0;
                    }

                    if (_rule.remeshFieldAlignedParameters.isTargetCount)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Polycount", GUILayout.MaxWidth(_labelSize));
                        _rule.remeshFieldAlignedParameters.targetTriangleCount = EditorGUILayout.IntField(_rule.remeshFieldAlignedParameters.targetTriangleCount, GUILayout.Width(_fieldNumberSize));
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Ratio", GUILayout.MaxWidth(_labelSize));
                        _rule.remeshFieldAlignedParameters.targetRatio = EditorGUILayout.Slider((float)_rule.remeshFieldAlignedParameters.targetRatio * 100.0f, 0.0f, 100.0f, GUILayout.Width(_popUpSize)) / 100.0f;
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Feature size", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshFieldAlignedParameters.featureSize = EditorGUILayout.DoubleField(_rule.remeshFieldAlignedParameters.featureSize, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Full quad", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshFieldAlignedParameters.fullQuad = EditorGUILayout.ToggleLeft("", _rule.remeshFieldAlignedParameters.fullQuad, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Keep animations", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshFieldAlignedParameters.transferAnimations = EditorGUILayout.ToggleLeft("", _rule.remeshFieldAlignedParameters.transferAnimations, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Bake maps", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshFieldAlignedParameters.bakeMaps = EditorGUILayout.ToggleLeft("", _rule.remeshFieldAlignedParameters.bakeMaps, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.BeginDisabledGroup(!_rule.remeshFieldAlignedParameters.bakeMaps);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Maps resolution", GUILayout.MaxWidth(_labelSize));
                    _rule.remeshFieldAlignedParameters.mapsResolution = EditorGUILayout.IntField(_rule.remeshFieldAlignedParameters.mapsResolution, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUI.EndDisabledGroup();

                }
                EditorGUI.indentLevel -= 2;
                EditorGUILayout.Space();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DisplayDecimate()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            EditorGUI.BeginDisabledGroup(_rule.isImposterActivated);

            bool decimateState = EditorGUILayout.ToggleLeft("", _rule.isDecimateToQualityActivated || _rule.isDecimateToTargetActivated, GUILayout.Width(_toggleSize));

            if (EditorGUI.EndChangeCheck())
            {
                if (!decimateState)
                {
                    _rule.isDecimateToQualityActivated = false;
                    _rule.isDecimateToTargetActivated = false;
                    isDecimateFoldout = false;
                }
                else
                {
                    _rule.isDecimateToTargetActivated = true;
                    _rule.isImposterActivated = false;
                }
            }

            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginChangeCheck();

            bool foldout = EditorExtensions.CustomFoldout(isDecimateFoldout);

            if (EditorGUI.EndChangeCheck())
            {
                isDecimateFoldout = foldout;
            }

            EditorExtensions.DisplaySeparator("Decimation", true);
            EditorGUILayout.EndHorizontal();


            EditorGUI.BeginDisabledGroup(!_rule.isDecimateToQualityActivated && !_rule.isDecimateToTargetActivated);

            if (isDecimateFoldout)
            {
                EditorGUI.indentLevel += 2;
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", GUILayout.MaxWidth(_labelSize));
                int value = EditorGUILayout.Popup(_rule.isDecimateToTargetActivated ? 0 : 1, new GUIContent[] { new GUIContent("To Target"), new GUIContent("To Quality") }, GUILayout.Width(_popUpSize));
                EditorGUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck())
                {
                    _rule.isDecimateToTargetActivated = value == 0;
                    _rule.isDecimateToQualityActivated = value == 1;
                }

                if (_rule.isDecimateToTargetActivated)
                {
                    EditorGUI.BeginChangeCheck();
                    int vertexWeightScale = 0;

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Target type", GUILayout.MaxWidth(_labelSize));
                    int ratioSelect = EditorGUILayout.Popup(_rule.decimateToTarget.isTargetCount ? 0 : 1, new GUIContent[] { new GUIContent("Polycount"), new GUIContent("Ratio") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    if (_rule.decimateToTarget.isTargetCount)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Polycount", GUILayout.MaxWidth(_labelSize));
                        _rule.decimateToTarget.polycount = EditorGUILayout.IntField(_rule.decimateToTarget.polycount, GUILayout.Width(_fieldNumberSize));
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Ratio", GUILayout.MaxWidth(_labelSize));
                        _rule.decimateToTarget.ratio = EditorGUILayout.Slider((float)_rule.decimateToTarget.ratio * 100.0f, 0.0f, 100.0f, GUILayout.Width(150)) / 100.0f;
                        EditorGUILayout.EndHorizontal();
                    }

                    //EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Normal tolerance", GUILayout.MaxWidth(_labelSize));
                    //_rule.decimateToTarget.normalTolerance = EditorGUILayout.DoubleField(_rule.decimateToTarget.normalTolerance, GUILayout.Width(_fieldNumberSize));
                    //EditorGUILayout.EndHorizontal();

                    //EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("UV tolerance", GUILayout.MaxWidth(_labelSize));
                    //_rule.decimateToTarget.uvTolerance = EditorGUILayout.DoubleField(_rule.decimateToTarget.uvTolerance, GUILayout.Width(_fieldNumberSize));
                    //EditorGUILayout.EndHorizontal();

                    //EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("UV seam tolerance", GUILayout.MaxWidth(_labelSize));
                    //_rule.decimateToTarget.uvSeamTolerance = EditorGUILayout.DoubleField(_rule.decimateToTarget.uvSeamTolerance, GUILayout.Width(_fieldNumberSize));
                    //EditorGUILayout.EndHorizontal();

                    //EditorGUILayout.BeginHorizontal();
                    //EditorGUILayout.LabelField("Force target", GUILayout.MaxWidth(_labelSize));
                    //_rule.decimateToTarget.forceTarget = EditorGUILayout.ToggleLeft("", _rule.decimateToTarget.forceTarget, GUILayout.Width(_fieldToggleSize));
                    //EditorGUILayout.EndHorizontal();
                    

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Use vertex weights", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToTarget.useVertexWeights = EditorGUILayout.ToggleLeft("", _rule.decimateToTarget.useVertexWeights, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();
                    
                    if (_rule.decimateToTarget.useVertexWeights)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Vertex weight scale", GUILayout.MaxWidth(_labelSize));
                        vertexWeightScale = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToTarget.vertexWeightScale, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Boundary weight", GUILayout.MaxWidth(_labelSize));
                    int boundaryWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToTarget.boundaryWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Normal weight", GUILayout.MaxWidth(_labelSize));
                    int normalWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToTarget.normalWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("UV weight", GUILayout.MaxWidth(_labelSize));
                    int uvWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToTarget.uvWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Sharp normal weight", GUILayout.MaxWidth(_labelSize));
                    int sharpNormalWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToTarget.sharpNormalWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("UV seam weight", GUILayout.MaxWidth(_labelSize));
                    int uvSeamWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToTarget.uvSeamWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Forbid UV foldovers", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToTarget.forbidUVFoldovers = EditorGUILayout.ToggleLeft("", _rule.decimateToTarget.forbidUVFoldovers, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        _rule.decimateToTarget.isTargetCount = ratioSelect == 0;
                        
                        if (_rule.decimateToTarget.useVertexWeights)
                            _rule.decimateToTarget.vertexWeightScale = Mathf.Pow(10, vertexWeightScale);
                        
                        _rule.decimateToTarget.boundaryWeight = Mathf.Pow(10, boundaryWeight);
                        _rule.decimateToTarget.normalWeight = Mathf.Pow(10, normalWeight);
                        _rule.decimateToTarget.uvWeight = Mathf.Pow(10, uvWeight);
                        _rule.decimateToTarget.sharpNormalWeight = Mathf.Pow(10, sharpNormalWeight);
                        _rule.decimateToTarget.uvSeamWeight = Mathf.Pow(10, uvSeamWeight);
                    }
                }
                else if (_rule.isDecimateToQualityActivated)
                {
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Preset", GUILayout.MaxWidth(_labelSize));
                    int qualitySelect = EditorGUILayout.Popup((int)_rule.decimateToQualityParam.quality, new GUIContent[] { new GUIContent("High"), new GUIContent("Medium"), new GUIContent("Low"), new GUIContent("Custom") }, GUILayout.Width(150));
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        switch (qualitySelect)
                        {
                            case 0:
                                _rule.decimateToQualityParam.errorMax = DecimateToQualityParameters.High().errorMax;
                                _rule.decimateToQualityParam.normalTolerance = DecimateToQualityParameters.High().normalTolerance;
                                _rule.decimateToQualityParam.uvTolerance = DecimateToQualityParameters.High().uvTolerance;
                                break;
                            case 1:
                                _rule.decimateToQualityParam.errorMax = DecimateToQualityParameters.Medium().errorMax;
                                _rule.decimateToQualityParam.normalTolerance = DecimateToQualityParameters.Medium().normalTolerance;
                                _rule.decimateToQualityParam.uvTolerance = DecimateToQualityParameters.Medium().uvTolerance;
                                break;
                            case 2:
                                _rule.decimateToQualityParam.errorMax = DecimateToQualityParameters.Low().errorMax;
                                _rule.decimateToQualityParam.normalTolerance = DecimateToQualityParameters.Low().normalTolerance;
                                _rule.decimateToQualityParam.uvTolerance = DecimateToQualityParameters.Low().uvTolerance;
                                break;
                            default:
                                break;
                        }
                        _rule.decimateToQualityParam.quality = (DecimateQualityLevels)qualitySelect;
                    }

                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Surfacic tolerance", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToQualityParam.errorMax = EditorGUILayout.DoubleField(_rule.decimateToQualityParam.errorMax, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Normal tolerance", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToQualityParam.normalTolerance = EditorGUILayout.DoubleField(_rule.decimateToQualityParam.normalTolerance, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("UV tolerance", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToQualityParam.uvTolerance = EditorGUILayout.DoubleField(_rule.decimateToQualityParam.uvTolerance, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("UV seam tolerance", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToQualityParam.uvSeamTolerance = EditorGUILayout.DoubleField(_rule.decimateToQualityParam.uvSeamTolerance, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        _rule.decimateToQualityParam.quality = DecimateQualityLevels.Custom;
                    }
                    /*
                    int vertexWeightScale = 0;
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Use vertex weights", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToQualityParam.useVertexWeights = EditorGUILayout.ToggleLeft("", _rule.decimateToQualityParam.useVertexWeights, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    if (_rule.decimateToQualityParam.useVertexWeights)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Vertex weight scale", GUILayout.MaxWidth(_labelSize));
                        vertexWeightScale = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToQualityParam.vertexWeightScale, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Boundary weight", GUILayout.MaxWidth(_labelSize));
                    int boundaryWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToQualityParam.boundaryWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Normal weight", GUILayout.MaxWidth(_labelSize));
                    int normalWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToQualityParam.normalWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("UV weight", GUILayout.MaxWidth(_labelSize));
                    int uvWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToQualityParam.uvWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Sharp normal weight", GUILayout.MaxWidth(_labelSize));
                    int sharpNormalWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToQualityParam.sharpNormalWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("UV seam weight", GUILayout.MaxWidth(_labelSize));
                    int uvSeamWeight = EditorGUILayout.Popup((int)Mathf.Log((int)_rule.decimateToQualityParam.uvSeamWeight, 10), new GUIContent[] { new GUIContent("Low"), new GUIContent("Normal"), new GUIContent("Important"), new GUIContent("Very Important") }, GUILayout.Width(_popUpSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Forbid UV foldovers", GUILayout.MaxWidth(_labelSize));
                    _rule.decimateToQualityParam.forbidUVFoldovers = EditorGUILayout.ToggleLeft("", _rule.decimateToQualityParam.forbidUVFoldovers, GUILayout.Width(_fieldToggleSize));
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (_rule.decimateToQualityParam.useVertexWeights)
                            _rule.decimateToQualityParam.vertexWeightScale = Mathf.Pow(10, vertexWeightScale);

                        _rule.decimateToQualityParam.boundaryWeight = Mathf.Pow(10, boundaryWeight);
                        _rule.decimateToQualityParam.normalWeight = Mathf.Pow(10, normalWeight);
                        _rule.decimateToQualityParam.uvWeight = Mathf.Pow(10, uvWeight);
                        _rule.decimateToQualityParam.sharpNormalWeight = Mathf.Pow(10, sharpNormalWeight);
                        _rule.decimateToQualityParam.uvSeamWeight = Mathf.Pow(10, uvSeamWeight);
                    }*/
                }

                EditorGUI.indentLevel -= 2;
                EditorGUILayout.Space();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DisplayOcculusionParam()
        {
            EditorGUI.BeginDisabledGroup(_rule.isRemeshActivated || _rule.isRemeshFieldAlignedActivated || _rule.isImposterActivated);

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            _rule.isOcclusionActivated = EditorGUILayout.ToggleLeft("", _rule.isOcclusionActivated, GUILayout.Width(_toggleSize));

            if (EditorGUI.EndChangeCheck())
            {
                if (_rule.isOcclusionActivated)
                {
                    _rule.isImposterActivated = false;
                }
            }

            EditorGUI.EndDisabledGroup();

            isOcclusionFoldout = EditorExtensions.CustomFoldout(isOcclusionFoldout);

            EditorExtensions.DisplaySeparator("Hidden removal", true);
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(!_rule.isOcclusionActivated);

            EditorGUI.indentLevel += 2;

            if (_rule.isRemeshActivated || _rule.isRemeshFieldAlignedActivated)
            {
                EditorGUILayout.LabelField("The active retopolization operation already includes a hidden removal operation");
            }

            if (isOcclusionFoldout)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", GUILayout.Width(_labelSize));
                int occlusionMode = EditorGUILayout.Popup((int)_rule.occlusionParameters.mode, new GUIContent[] { new GUIContent("Standard"), new GUIContent("Advanced") }, GUILayout.Width(_popUpSize));
                EditorGUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck())
                {
                    _rule.occlusionParameters.mode = (OcclusionMode)occlusionMode;
                }

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Ignore transparency", GUILayout.MaxWidth(_labelSize));
                _rule.occlusionParameters.considerTransparentOpaque = EditorGUILayout.ToggleLeft("", _rule.occlusionParameters.considerTransparentOpaque, GUILayout.Width(_fieldToggleSize));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Neighbours preservation", GUILayout.MaxWidth(_labelSize));
                _rule.occlusionParameters.adjacencyDepth = EditorGUILayout.IntField(_rule.occlusionParameters.adjacencyDepth, GUILayout.Width(_fieldNumberSize));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Camera resolution", GUILayout.MaxWidth(_labelSize));
                _rule.occlusionParameters.cameraResolution = EditorGUILayout.IntField(_rule.occlusionParameters.cameraResolution, GUILayout.Width(_fieldNumberSize));
                EditorGUILayout.EndHorizontal();

                if (_rule.occlusionParameters.mode == OcclusionMode.Advanced)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Voxel size", GUILayout.MaxWidth(_labelSize));
                    _rule.occlusionParameters.voxelSize = EditorGUILayout.DoubleField(_rule.occlusionParameters.voxelSize, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Minimum cavity volume", GUILayout.MaxWidth(_labelSize));
                    _rule.occlusionParameters.minimumCavityVolume = EditorGUILayout.DoubleField(_rule.occlusionParameters.minimumCavityVolume, GUILayout.Width(_fieldNumberSize));
                    EditorGUILayout.EndHorizontal();

                }
                EditorGUILayout.Space();
            }
            EditorGUI.indentLevel -= 2;

            EditorGUI.EndDisabledGroup();
        }
    }
}