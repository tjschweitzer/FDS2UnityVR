using System.Collections.Generic;
using UnityEngine;
using Pixyz.OptimizeSDK.Native;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;

namespace Pixyz.Toolbox.Editor
{
    public class DecimateToQualityAction : PixyzFunction
    {
        public override int id => 15654565;
        public override int order => 3;
        public override string menuPathRuleEngine => "Mesh/Decimate To Quality";
        public override string menuPathToolbox => "Mesh/Decimate To Quality";
        public override string tooltip => ToolboxTooltips.decimQualityAction;
        protected override MaterialSyncType SyncMaterials => MaterialSyncType.SyncNone;

        public enum DecimationStrategy
        {
            VertexRemoval,
            EdgeCollapse
        }

        public enum DecimationQuality {
            Custom = 0,
            High = 1,
            Medium = 2,
            Low = 3,
        }

        public enum Weight
        {
            Low = 1,
            Normal = 10,
            Important = 100,
            VeryImportant = 1000
        }

        private struct DecimateToQualityVertexRemoval
        {
            public double surfacicTolerance;
            public double lineicTolerance;
            public double normalTolerance;
            public double uvTolerance;

            public DecimateToQualityVertexRemoval(double s, double l, double n, double t) {
                surfacicTolerance = s;
                lineicTolerance = l;
                normalTolerance = n;
                uvTolerance = t;
            }
        }
        private Dictionary<DecimationQuality, DecimateToQualityVertexRemoval> qualityVertexRemovalPresets = new Dictionary<DecimationQuality, DecimateToQualityVertexRemoval>()
        {
            {DecimationQuality.High, new DecimateToQualityVertexRemoval(0.0005, 0.0001, 1, -1) },
            {DecimationQuality.Medium, new DecimateToQualityVertexRemoval(0.001, -1, 8, -1) },
            {DecimationQuality.Low, new DecimateToQualityVertexRemoval(0.003, -1, 15, -1) }
        };

        private struct DecimateToQuality
        {
            public double errorMax;
            public double normalMaxDeviation;
            public double uvMaxDeviation;

            public DecimateToQuality(double s, double n, double t)
            {
                errorMax = s;
                normalMaxDeviation = n;
                uvMaxDeviation = t;
            }
        }
        private Dictionary<DecimationQuality, DecimateToQuality> qualityPresets = new Dictionary<DecimationQuality, DecimateToQuality>()
        {
            {DecimationQuality.High, new DecimateToQuality(0.0005, -1, -1) },
            {DecimationQuality.Medium, new DecimateToQuality(0.001, -1, -1) },
            {DecimationQuality.Low, new DecimateToQuality(0.003, -1, -1) }
        };
                public bool isEdgeCollapse() => strategy == DecimationStrategy.EdgeCollapse;
        public bool isVertexRemoval() => strategy == DecimationStrategy.VertexRemoval;

        [UserParameter(tooltip: ToolboxTooltips.decimQualityStrategy)]
        public DecimationStrategy strategy = DecimationStrategy.VertexRemoval;

        [UserParameter("isVertexRemoval", displayName: "Quality Preset", tooltip: ToolboxTooltips.decimQualityPreset)]
        public DecimationQuality vertexRemovalQuality = DecimationQuality.Medium;

        [UserParameter("isEdgeCollapse", displayName: "Quality Preset", tooltip: ToolboxTooltips.decimQualityPreset)]
        public DecimationQuality quality = DecimationQuality.Medium;

        public DecimationQuality prevQuality;

        [UserParameter("isEdgeCollapse", displayName:"Surfacic tolerance", tooltip: ToolboxTooltips.decimQualitySurfacic)]
        public double errorMax = 0.001;

        //[UserParameter("isEdgeCollapse", tooltip:"Use vertex weights if any. Vertex weights are computed from red vertices colors")]
        public bool useVertexWeight = false;

        //[UserParameter("useVertexWeights", tooltip:"Give more importance to colored vertices preservation" )]
        public Weight vertexWeightScale = Weight.Normal;

        //[UserParameter("isEdgeCollapse", tooltip: "Preserve the edges defining the mesh boundaries (free edges) from being distorted")]
        public Weight boundaryWeight = Weight.Low;

        //[UserParameter("isEdgeCollapse", tooltip:"Preserve mesh smoothing from being damaged")]
        public Weight normalWeight = Weight.Low;

        //[UserParameter("isEdgeCollapse", displayName: "UV weight", tooltip:"Preserve UVs from being distorted")]
        public Weight uvWeight = Weight.Low;

        //[UserParameter("isEdgeCollapse", tooltip:"Preserve sharp edges (or hard edges) from being distorted")]
        public Weight sharpNormalWeight = Weight.Low;

        //[UserParameter("isEdgeCollapse", displayName: "UV seam weight", tooltip:"Preserve UV seams (UV islands contours) from being distorted")]
        public Weight uvSeamWeight = Weight.Normal;

        [UserParameter("isEdgeCollapse", displayName:"Normal tolerance", tooltip: ToolboxTooltips.decimQualityNormalDeviation)]
        public double normalMaxDeviation = -1;

        [UserParameter("isVertexRemoval", tooltip: ToolboxTooltips.decimQualityNormalTolerance)]
        public double normalTolerance;

        [UserParameter("isEdgeCollapse", displayName: "UV tolerance", tooltip: ToolboxTooltips.decimQualityUV)]
        public double uvMaxDeviation = -1;

        [UserParameter("isEdgeCollapse", displayName: "UV seam tolerance", tooltip: ToolboxTooltips.decimQualityUVSeam)]
        public double uvSeamTolerance = -1;

        [UserParameter("isEdgeCollapse", displayName: "Forbid UV foldovers", tooltip: ToolboxTooltips.decimQualityUVFoldovers)]
        public bool forbidUvFoldovers = true;
        public DecimationQuality prevVertRemQuality;

        [UserParameter("isVertexRemoval", tooltip: ToolboxTooltips.decimQualitySurfacic)]
        public double surfacicTolerance;

        [UserParameter("isVertexRemoval", tooltip: ToolboxTooltips.decimQualityLineic)]
        public double lineicTolerance;

        [UserParameter("isVertexRemoval", displayName:"UV tolerance", tooltip: ToolboxTooltips.decimQualityUV)]
        public double uvTolerance;

        public override void onBeforeDraw()
        {
            base.onBeforeDraw();
            if (isVertexRemoval())
            {
                BaseExtensionsEditor.MatchEnumWithCustomValue(ref vertexRemovalQuality, prevVertRemQuality, ref surfacicTolerance, nameof(surfacicTolerance), qualityVertexRemovalPresets);
                BaseExtensionsEditor.MatchEnumWithCustomValue(ref vertexRemovalQuality, prevVertRemQuality, ref lineicTolerance, nameof(lineicTolerance), qualityVertexRemovalPresets);
                BaseExtensionsEditor.MatchEnumWithCustomValue(ref vertexRemovalQuality, prevVertRemQuality, ref normalTolerance, nameof(normalTolerance), qualityVertexRemovalPresets);
                BaseExtensionsEditor.MatchEnumWithCustomValue(ref vertexRemovalQuality, prevVertRemQuality, ref uvTolerance, nameof(uvTolerance), qualityVertexRemovalPresets);
                prevVertRemQuality = vertexRemovalQuality;
            }
            else if (isEdgeCollapse())
            {
                BaseExtensionsEditor.MatchEnumWithCustomValue(ref quality, prevQuality, ref errorMax, nameof(errorMax), qualityPresets);
                BaseExtensionsEditor.MatchEnumWithCustomValue(ref quality, prevQuality, ref normalMaxDeviation, nameof(normalMaxDeviation), qualityPresets);
                BaseExtensionsEditor.MatchEnumWithCustomValue(ref quality, prevQuality, ref uvMaxDeviation, nameof(uvMaxDeviation), qualityPresets);
                prevQuality = quality;
            }
        }

        protected override void process()
        {
            try
            {
                NativeInterface.PushAnalytic("DecimateToQuality", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                UpdateProgressBar(0.5f);
                if (isVertexRemoval())
                {
                    NativeInterface.DecimateToQualityVertexRemoval(Context.pixyzMeshes, surfacicTolerance, lineicTolerance, normalTolerance, uvTolerance, Context.pixyzMatrices);
                }
                else if (isEdgeCollapse())
                {
                    NativeInterface.DecimateToQuality(Context.pixyzMeshes, errorMax, Context.pixyzMatrices, useVertexWeight, 1.0, (double)vertexWeightScale, (double)boundaryWeight, (double)normalWeight, (double)uvWeight, (double)sharpNormalWeight, (double)uvSeamWeight, forbidUvFoldovers, normalMaxDeviation, uvMaxDeviation, uvSeamTolerance, true);
                }
                UpdateProgressBar(0.75f);
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
            if (isEdgeCollapse())
            {
                if (errorMax <= 0)
                {
                    errors.Add("Surfacic tolerance is too low ! (must be higher than 0)");
                }
            } else if (isVertexRemoval())
            {
                if (surfacicTolerance <= 0)
                {
                    errors.Add("Surfacic tolerance is too low ! (must be higher than 0)");
                }
            }
            return errors.ToArray();
        }
    }
}