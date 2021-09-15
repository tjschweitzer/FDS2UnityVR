using System.Collections.Generic;
using UnityEngine;
using Pixyz.Commons;
using Pixyz.OptimizeSDK.Native;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;

namespace Pixyz.Toolbox.Editor
{
    public class DecimateToTargetAction : PixyzFunction
    {
        public override int id => 15654564;
        public override int order => 4;
        public override string menuPathRuleEngine => "Mesh/Decimate To Target";
        public override string menuPathToolbox => "Mesh/Decimate To Target";
        protected override MaterialSyncType SyncMaterials => MaterialSyncType.SyncNone;
        public override string tooltip => ToolboxTooltips.decimTargetAction;

        public enum Weight
        {
            Low = 1,
            Normal = 10,
            Important = 100,
            VeryImportant = 1000
        }

        public enum DecimationStrategy
        {
            TriangleCount,
            Ratio
        }
        private bool isPolycount() { return strategy == DecimationStrategy.TriangleCount; }
        private bool isRatio() { return strategy == DecimationStrategy.Ratio; }
        private bool useVertexWeights() { return useVertexWeight; }

        [UserParameter(tooltip: ToolboxTooltips.decimTargetStrategy)]
        public DecimationStrategy strategy = DecimationStrategy.Ratio;

        [UserParameter("isPolycount", displayName:"Triangle count", tooltip: ToolboxTooltips.decimTargetCount)]
        public int polycount = 5000;

        [UserParameter("isRatio", displayName:"Ratio", tooltip: ToolboxTooltips.decimTargetRatio)]
        public Range targetRatio = (Range)50f;

        [UserParameter(tooltip: ToolboxTooltips.decimTargetVertex)]
        public bool useVertexWeight = false;

        [UserParameter("useVertexWeights", tooltip: ToolboxTooltips.decimTargetWeightScale)]
        public Weight vertexWeightScale = Weight.Normal;

        [UserParameter(tooltip: ToolboxTooltips.decimTargetBoundary)]
        public Weight boundaryWeight = Weight.Low;

        [UserParameter(tooltip: ToolboxTooltips.decimTargetNormal)]
        public Weight normalWeight = Weight.Low;

        [UserParameter(displayName:"UV weight", tooltip: ToolboxTooltips.decimTargetUV)]
        public Weight uvWeight = Weight.Low;

        [UserParameter(tooltip: ToolboxTooltips.decimTargetSharpNormal)]
        public Weight sharpNormalWeight = Weight.Low;

        [UserParameter(displayName:"UV seam weight", tooltip: ToolboxTooltips.decimTargetUVSeam)]
        public Weight uvSeamWeight = Weight.Normal;

        //[UserParameter(tooltip: "Constraint the maximum normals deviation (angle threshold)")]
        //public double normalTolerance = -1;

        //[UserParameter(displayName: "UV tolerance", tooltip: "Constaint the maximum UV deviation (displacement)")]
        //public double uvTolerance = -1;

        //[UserParameter(displayName: "UV seam tolerance", tooltip: "Constraint the maximum UV seams deviation (displacement)")]
        //public double uvSeamTolerance = -1;

        [UserParameter(displayName: "Forbid UV foldovers", tooltip: ToolboxTooltips.decimTargetUVFoldovers)]
        public bool forbidUvFoldovers = true;

        protected override void process()
        {
            try
            {
                NativeInterface.PushAnalytic("DecimateToTarget", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                UpdateProgressBar(0.5f);

                switch (strategy)
                {
                    case DecimationStrategy.TriangleCount:
                        NativeInterface.DecimateToPolycount(Context.pixyzMeshes, polycount, Context.pixyzMatrices, useVertexWeight, 1.0, (double)vertexWeightScale, (double)boundaryWeight, (double)normalWeight, (double)uvWeight, (double)sharpNormalWeight, (double)uvSeamWeight, forbidUvFoldovers, true);
                        break;
                    case DecimationStrategy.Ratio:
                        NativeInterface.DecimateToRatio(Context.pixyzMeshes, (double)targetRatio.value / 100.0, Context.pixyzMatrices, useVertexWeight, 1.0, (double)vertexWeightScale, (double)boundaryWeight, (double)normalWeight, (double)uvWeight, (double)sharpNormalWeight, (double)uvSeamWeight, forbidUvFoldovers, true);
                        break;
                    default:
                        break;
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
            GameObject[] outputParts = Context.PixyzMeshToUnityObject(Context.pixyzMeshes);            
            ReplaceInHierarchy(InputParts, outputParts);
        }

        public override IList<string> getErrors()
        {
            var errors = new List<string>();
            if (isPolycount())
            {
                if (polycount <= 0)
                {
                    errors.Add("Target polycount is too low ! (must be higher than 0)");
                }
            }
            return errors.ToArray();
        }
    }
}