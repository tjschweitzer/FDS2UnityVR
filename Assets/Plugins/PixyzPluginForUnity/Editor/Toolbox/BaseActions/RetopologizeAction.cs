using System.Collections.Generic;
using UnityEngine;
using Pixyz.Commons;
using Pixyz.Commons.Extensions;
using Pixyz.OptimizeSDK;
using Pixyz.OptimizeSDK.Native;
using Pixyz.OptimizeSDK.Native.Polygonal;
using Pixyz.OptimizeSDK.Native.Geom;
using Pixyz.OptimizeSDK.Native.Algo;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public enum MapDimensions
    {
        _8192 = 8192,
        _4096 = 4096,
        _2048 = 2048,
        _1024 = 1024,
        _512 = 512,
        _256 = 256,
        Custom = 0
    }

    public enum GridResolutionPreset
    {
        Custom = 0,
        Poor = 10,
        Low = 25,
        Medium = 50,
        High = 100,
        VeryHigh = 200
    }

    public enum VertexTarget
    {
        TriangleCount,
        Ratio
    }
    public enum RetopologizeType
    {
        Standard,
        FieldAligned
    }

    public class RetopologizeAction : PixyzFunction
    {
        public override int id => 57249905;
        public override int order => 6;
        public override string menuPathRuleEngine => "Remeshing/Retopologize";
        public override string menuPathToolbox => "Remeshing/Retopologize";
        public override string tooltip => ToolboxTooltips.retopologizeAction;
        private bool isFieldAligned() { return type == RetopologizeType.FieldAligned; }
        private bool isStandard() { return type == RetopologizeType.Standard; }
        private bool isCustom() { return mapsResolution == MapDimensions.Custom && isBakingMaps(); }
        private bool isPolycount() => criterion == VertexTarget.TriangleCount && isFieldAligned();
        private bool isPolycountRatio() => criterion == VertexTarget.Ratio && isFieldAligned();
        private bool isBakingMaps() => bakeMaps;
        private bool isFeatureSizeActive() => isFieldAligned() && useFeatureSize;

        [UserParameter(tooltip: ToolboxTooltips.retopologizeType)]
        public RetopologizeType type = RetopologizeType.Standard;

        [UserParameter("isFieldAligned", displayName:"Strategy", tooltip: ToolboxTooltips.retopologizeStrategy)]
        public VertexTarget criterion = VertexTarget.Ratio;

        [UserParameter("isPolycount", displayName:"Triangles", tooltip: ToolboxTooltips.retopologizeTriangles)]
        public int targetTriangleCount = 10000;

        [UserParameter("isPolycountRatio", displayName:"Ratio", tooltip: ToolboxTooltips.retopologizeRatio)]
        public Range targetRatio = (Range)10f;

        [UserParameter("isStandard", displayName:"Quality preset", tooltip: ToolboxTooltips.retopologizeQuality)]
        public GridResolutionPreset gridResolutionPreset = GridResolutionPreset.Medium;

        [UserParameter("isStandard", displayName: "Quality value", tooltip: ToolboxTooltips.retopologizeQualityValue)]
        public int gridResolution = (int)GridResolutionPreset.Medium;

        [UserParameter("isFieldAligned", tooltip: ToolboxTooltips.retopologizeUseFeature)]
        public bool useFeatureSize = false;

        [UserParameter("isFeatureSizeActive", tooltip: ToolboxTooltips.retopologizeFeatureSize)]
        public float featureSize = 0.1f;

        [UserParameter(tooltip: ToolboxTooltips.retopologizeBake)]
        public bool bakeMaps = true;

        [UserParameter("isBakingMaps", tooltip: ToolboxTooltips.retopologizeMapResolution)]
        public MapDimensions mapsResolution = MapDimensions._1024;

        [UserParameter("isCustom", tooltip: ToolboxTooltips.retopologizeMapResolution)]
        public int resolution = 1024;

        [UserParameter("isStandard", tooltip: ToolboxTooltips.retopologizePtCloud)]
        public bool isPointCloud = false;

        private GridResolutionPreset _prevGridResolutionPreset = GridResolutionPreset.Medium;
        private int _prevGridResolution = (int)GridResolutionPreset.Medium;

        public override IList<string> getWarnings()
        {
            var warnings = new List<string>();
            if (isStandard())
            {
                if (gridResolution >= 1000)
                    warnings.Add("Quality value is too high! (The execution can take a lot of time)");
            }
            return warnings.ToArray();
        }
        public override IList<string> getErrors()
        {
            var errors = new List<string>();
            if (isStandard())
            {
                if (gridResolution <= 0)
                    errors.Add("Quality value is too low! (must be higher than 0)");
            }
            if(isFieldAligned())
            {
                if (featureSize <= 0 && useFeatureSize)
                    errors.Add("Feature size is too low ! (must be higher than 0)");
            }
            if (isCustom())
            {
                if (resolution < 64)
                {
                    errors.Add("Maps resolution is too low ! (must be between 64 and 8192)");
                }
                if (resolution > 8192)
                {
                    errors.Add("Maps resolution is too high ! (must be between 64 and 8192)");
                }
            }
            return errors.ToArray();
        }
        public override void onBeforeDraw()
        {
            base.onBeforeDraw();

            if (_prevGridResolutionPreset != gridResolutionPreset)
            {
                gridResolution = (int)gridResolutionPreset;
                _prevGridResolutionPreset = gridResolutionPreset;
                _prevGridResolution = gridResolution;
            }
            else if (gridResolution != _prevGridResolution)
            {
                switch ((GridResolutionPreset)gridResolution)
                {
                    case GridResolutionPreset.VeryHigh:
                        gridResolutionPreset = GridResolutionPreset.VeryHigh;
                        break;
                    case GridResolutionPreset.High:
                        gridResolutionPreset = GridResolutionPreset.High;
                        break;
                    case GridResolutionPreset.Medium:
                        gridResolutionPreset = GridResolutionPreset.Medium;
                        break;
                    case GridResolutionPreset.Low:
                        gridResolutionPreset = GridResolutionPreset.Low;
                        break;
                    case GridResolutionPreset.Poor:
                        gridResolutionPreset = GridResolutionPreset.Poor;
                        break;
                    default:
                        gridResolutionPreset = GridResolutionPreset.Custom;
                        break;
                }
                _prevGridResolutionPreset = gridResolutionPreset;
                _prevGridResolution = gridResolution;
            }
        }

        protected override void process()
        {
            try
            {
                NativeInterface.PushAnalytic("Retopologize", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                UpdateProgressBar(0.35f, "Remeshing...");
                uint newMesh = 0;
                
                AABB aabb = NativeInterface.GetAABB(Context.pixyzMeshes, Context.pixyzMatrices);
                float featureSize = Mathf.Max((float)(aabb.high.x - aabb.low.x), (float)(aabb.high.y - aabb.low.y), (float)(aabb.high.z - aabb.low.z)) / gridResolution;

                if (type == RetopologizeType.FieldAligned)
                {
                    if (isPolycount())
                        newMesh = NativeInterface.RemeshFieldAligned(Context.pixyzMeshes, targetTriangleCount, Context.pixyzMatrices, true, useFeatureSize ? featureSize : -1, true);
                    else if (isPolycountRatio())
                        newMesh = NativeInterface.RemeshFieldAlignedToRatio(Context.pixyzMeshes, (double)targetRatio.value / 100.0, Context.pixyzMatrices, true, useFeatureSize ? featureSize : -1, true);
                }
                else if (type == RetopologizeType.Standard)
                {
                    newMesh = NativeInterface.Remesh(Context.pixyzMeshes, featureSize, isPointCloud, Context.pixyzMatrices, true);
                    NativeInterface.DecimateToQualityVertexRemoval(new MeshList(new uint[] { newMesh }), 0.001, 0.0005, -1, -1, new Matrix4List(new Matrix4[] { Conversions.Identity() }));
                }
                
                MeshList newMeshes = new MeshList(new uint[] { newMesh });

                if (bakeMaps)
                {
                    UpdateProgressBar(0.65f, "Baking...");

                    MapTypeList mapsToBake = null;
                    if (isPointCloud)
                    {
                        mapsToBake = new MapTypeList(new MapType[] { MapType.Diffuse });
                    }
                    else
                    {
                        mapsToBake = new MapTypeList(new MapType[] { MapType.Diffuse, MapType.Normal, MapType.Metallic, MapType.Roughness, MapType.AO });
                    }
                    NativeInterface.BakeMaterials(Context.pixyzMeshes, newMesh, mapsResolution == MapDimensions.Custom ? resolution : (int)mapsResolution, 1, true, isPointCloud ? BakingMethod.ProjOnly : BakingMethod.RayOnly, mapsToBake, Context.pixyzMatrices, Conversions.Identity());
                }

                Context.pixyzMeshes = newMeshes;
                Context.pixyzMatrices = new Matrix4List(new Matrix4[] { Conversions.Identity() });

                UpdateProgressBar(1f);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} \n {e.StackTrace}");
            }
        }

        protected override void postProcess()
        {
            _output = Context.PixyzMeshToUnityObject(Context.pixyzMeshes);
            DisableInput();

            int polyCount = _output.GetMeshes().GetPolyCount();
            foreach (var go in (IList<GameObject>)Output) go.name = "Retopo-" + polyCount;
        }
    }
}