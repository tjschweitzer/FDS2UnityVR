using System.Collections.Generic;
using UnityEngine;
using Pixyz.OptimizeSDK.Native;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;

namespace Pixyz.Toolbox.Editor
{
    public class RemoveHiddenAction : PixyzFunction
    {
        private bool _isTransparentDetected = false;
        public override int id => 79878745;
        public override int order => 5;
        public override string menuPathRuleEngine => "Mesh/Remove Hidden";
        public override string menuPathToolbox => "Mesh/Remove Hidden";
        public override string tooltip => ToolboxTooltips.removeHiddenAction;
        protected override MaterialSyncType SyncMaterials => MaterialSyncType.SyncSendOnly;

        public enum CullingStrategy
        {
            Standard = 0,
            Advanced = 1
        }

        public enum SelectionLevel
        {
            GameObject = 0,
            SubMesh = 1,
            Triangles = 2,
        }

        public enum DensityEnum
        {
            High = 50,
            Medium = 20,
            Low = 3
        }

        public enum PrecisonPreset : int
        {
            High = 2048,
            Medium = 1024,
            Low = 256,
            Custom = 0
        }

        private bool isStandardStrategy() => strategy == CullingStrategy.Standard;
        private bool isSmartStrategy() => strategy == CullingStrategy.Advanced;
        private bool isTriangle() => selectionLevel == SelectionLevel.Triangles;
        private bool isDetectingCavities() => preserveCavities && isSmartStrategy();

        [UserParameter(tooltip: ToolboxTooltips.removeHiddenStrategy)]
        public CullingStrategy strategy = CullingStrategy.Standard;

        [UserParameter("isSmartStrategy", displayName:"Precision", tooltip: ToolboxTooltips.removeHiddenPrecision)]
        public DensityEnum density = DensityEnum.Medium;

        [UserParameter("isStandardStrategy", tooltip:ToolboxTooltips.removeHiddenPrecisionPreset)]
        public PrecisonPreset precisionPreset = PrecisonPreset.Medium;

        [UserParameter("isStandardStrategy", tooltip: ToolboxTooltips.removeHiddenPrecision)]
        public int precision = (int)PrecisonPreset.Medium;

        [UserParameter(displayName: "Level", tooltip: ToolboxTooltips.removeHiddenLevel)]
        public SelectionLevel selectionLevel = SelectionLevel.Triangles;

        [UserParameter("isTriangle", tooltip: ToolboxTooltips.removeHiddenNeighboors)]
        public int neighborsPreservation = 1;

        [UserParameter(tooltip: ToolboxTooltips.removeHiddenTransparent)]
        public bool considerTransparencyOpaque = false;

        [UserParameter("isSmartStrategy", tooltip: ToolboxTooltips.removeHiddenCavities)]
        public bool preserveCavities = false;

        [UserParameter("isDetectingCavities", tooltip: ToolboxTooltips.removeHiddenCavitiesMinimum)]
        public float minimumCavitiesToPreserve = 1f;

        private Bounds bounds = new Bounds();
        private bool skinnedMesh = false;
        private PrecisonPreset _prevPrecisionPreset = PrecisonPreset.Medium;
        private int _prevPrecision = 1024;

        private bool[] hiddenList;

        public override void onSelectionChanged(IList<GameObject> selection)
        {
            base.onSelectionChanged(selection);
            skinnedMesh = false;
            _isTransparentDetected = false;

            foreach (var go in selection)
            {
                Renderer r = go.GetComponent<Renderer>();
                if (r == null)
                    continue;

                if (r is SkinnedMeshRenderer && !skinnedMesh)
                {
                    skinnedMesh = true;
                }

                if(!_isTransparentDetected)
                {
                    Material[] materials = r.sharedMaterials;

                    foreach (Material mat in materials)
                    {
                        if (mat != null && mat.HasProperty("_Mode") && mat.GetFloat("_Mode") != 0)
                        {
                            _isTransparentDetected = true;
                            break;
                        }
                    }
                }

                if (_isTransparentDetected && skinnedMesh)
                    break;
            }
        }

        public override bool preProcess(IList<GameObject> input)
        {
            bounds = input.GetBoundsWorldSpace();
            return base.preProcess(input);
        }

        public override void onBeforeDraw()
        {
            base.onBeforeDraw();

            if(precisionPreset != _prevPrecisionPreset)
            {
                precision = (int)precisionPreset;
                _prevPrecisionPreset = precisionPreset;
                _prevPrecision = precision;
            }
            else if(precision != _prevPrecision)
            {
                switch((PrecisonPreset)precision)
                {
                    case PrecisonPreset.High:
                        precisionPreset = PrecisonPreset.High;
                        break;
                    case PrecisonPreset.Medium:
                        precisionPreset = PrecisonPreset.Medium;
                        break;
                    case PrecisonPreset.Low:
                        precisionPreset = PrecisonPreset.Low;
                        break;
                    default:
                        precisionPreset = PrecisonPreset.Custom;
                        break;
                }
                _prevPrecisionPreset = precisionPreset;
                _prevPrecision = precision;
            }
        }
        protected override void process()
        {
            try {
                NativeInterface.PushAnalytic("RemoveHiddenAdvanced", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                UpdateProgressBar(0.55f);

                if (strategy == CullingStrategy.Standard)
                {
                    hiddenList = NativeInterface.RemoveHidden(Context.pixyzMeshes, Context.pixyzMatrices, (OptimizeSDK.Native.Algo.SelectionLevel)selectionLevel, considerTransparencyOpaque, neighborsPreservation, precision);
                }
                else if (strategy == CullingStrategy.Advanced)
                {
                    double size = bounds.extents.magnitude * 2;

                    // compute bounds volume
                    var minimumCavityVolume = preserveCavities ? minimumCavitiesToPreserve * Mathf.Pow(10, -9) : -1f;
                    hiddenList = NativeInterface.RemoveHiddenAdvanced(Context.pixyzMeshes, Context.pixyzMatrices, (OptimizeSDK.Native.Algo.SelectionLevel)selectionLevel, size / (double)density, minimumCavityVolume, considerTransparencyOpaque, neighborsPreservation, 256);
                }
                
                UpdateProgressBar(1f);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} \n {e.StackTrace}");
            }
        }

        protected override void postProcess()
        {
            if(selectionLevel == SelectionLevel.Triangles)
            {
                GameObject[] outputParts = Context.PixyzMeshToUnityObject(Context.pixyzMeshes);
                ReplaceInHierarchy(InputParts, outputParts);
            }
            else
            {
                if (hiddenList.Length == InputParts.Count)
                {
                    int displacement = 0;
                    for (int i = 0; i < hiddenList.Length; i++)
                    {
                        if (hiddenList[i] == false)
                        {
                            GameObject.DestroyImmediate(InputParts[i - displacement]);
                            InputParts.RemoveAt(i - displacement);
                            displacement++;
                        }

                    }
                }
            }

        }

        public override IList<string> getErrors()
        {
            var errors = new List<string>();
            if (neighborsPreservation < 0)
            {
                errors.Add("Neighbours preservation is too low ! (must be higher than 0)");
            }

            if (precision % 2 != 0)
                errors.Add("Precison must be a power of 2");

            return errors.ToArray();
        }

        public override IList<string> getWarnings()
        {
            var warnings = new List<string>();
            if (skinnedMesh)
            {
                warnings.Add("Selection contains Skinned Mesh Renderer.\nPolygons only visible during animations can be removed.");
            }

            if (_isTransparentDetected && !considerTransparencyOpaque)
                warnings.Add("Some materials are transparent, the mesh will not be fully optimized since \"Consider transparency opaque\" is false");

            return warnings;
        }
    }
}