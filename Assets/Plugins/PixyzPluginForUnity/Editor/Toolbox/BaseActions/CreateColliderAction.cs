using System.Collections.Generic;
using System;
using UnityEngine;
using Pixyz.Commons;
using Pixyz.Commons.Extensions;
using Pixyz.OptimizeSDK;
using Pixyz.OptimizeSDK.Native;
using Pixyz.OptimizeSDK.Native.Polygonal;
using Pixyz.OptimizeSDK.Native.Geom;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;

namespace Pixyz.Toolbox.Editor
{
    public class CreateColliderAction : PixyzFunction
    {
        public override int id => 14249905;
        public override int order => 0;
        public override string menuPathRuleEngine => "Colliders/Create Collider";
        public override string menuPathToolbox => "Colliders/Create Collider";
        public override string tooltip => ToolboxTooltips.createColliderAction;
        protected override MaterialSyncType SyncMaterials => MaterialSyncType.SyncNone;

        public enum ProxyStrategy
        {
            Retopology,
            ConvexDecomposition,
            AxisAlignedBoundingBox,
            OriginalMesh
        }

        private bool asyncCheck() 
        {
            switch (strategy)
            {
                case ProxyStrategy.AxisAlignedBoundingBox:
                case ProxyStrategy.OriginalMesh:
                    _isAsync = false;
                    break;
                default:
                    _isAsync = true;
                    break;
            }
            return true;
        }

        private bool isConvexDecomposition() => strategy == ProxyStrategy.ConvexDecomposition;
        private bool isRetopology() => strategy == ProxyStrategy.Retopology;
        private bool isOriginalMesh() => strategy == ProxyStrategy.OriginalMesh;
        private bool isAABB() => strategy == ProxyStrategy.AxisAlignedBoundingBox;
        private bool isFeatureSizeActive() => isFieldAligned() && useFeatureSize;

        [UserParameter("asyncCheck", displayName:"Strategy", tooltip:ToolboxTooltips.createColliderStrategy)]
        public ProxyStrategy strategy = ProxyStrategy.Retopology;

        #region ConvexDecomposition Parameters

        [UserParameter("isConvexDecomposition", tooltip: ToolboxTooltips.createColliderMaxDecompo)]
        public int maxDecompositionPerMesh = 3;

        [UserParameter("isConvexDecomposition", tooltip: ToolboxTooltips.createColliderTriangles)]
        public int maxTrianglesPerMesh = 200;

        [UserParameter("isConvexDecomposition", tooltip: ToolboxTooltips.createColliderResolution)]
        public Range resolution = (Range)50f;

        #endregion

        #region Retopoligize Parameters

        [UserParameter("isRetopology", displayName: "Type", tooltip: ToolboxTooltips.createColliderStrategy)]
        public RetopologizeType type = RetopologizeType.Standard;

        [UserParameter("isFieldAligned", displayName:"Strategy", tooltip: ToolboxTooltips.retopologizeStrategy)]
        public VertexTarget criterion = VertexTarget.Ratio;

        [UserParameter("isPolycount", displayName: "Triangles Count" ,tooltip: ToolboxTooltips.retopologizeTriangles)]
        public int targetTriangleCount = 10000;

        [UserParameter("isPolycountRatio", displayName:"Ratio" ,tooltip: ToolboxTooltips.retopologizeRatio)]
        public Range targetRatio = (Range)10f;

        [UserParameter("isFieldAligned", tooltip: ToolboxTooltips.retopologizeUseFeature)]
        public bool useFeatureSize = false;

        [UserParameter("isFeatureSizeActive", tooltip: ToolboxTooltips.retopologizeFeatureSize)]
        public float featureSize = 0.1f;

        [UserParameter("isStandard", displayName:"Mesh quality preset", tooltip: ToolboxTooltips.retopologizeQuality)]
        public GridResolutionPreset gridResolutionPreset = GridResolutionPreset.Medium;

        [UserParameter("isStandard", displayName: "Quality value", tooltip: ToolboxTooltips.retopologizeQualityValue)]
        public int gridResolution = (int)GridResolutionPreset.Medium;

        [UserParameter("isStandard", tooltip: ToolboxTooltips.retopologizePtCloud)]
        public bool isPointCloud = false;

        private bool isFieldAligned() { return type == RetopologizeType.FieldAligned && isRetopology(); }
        private bool isStandard() { return type == RetopologizeType.Standard && isRetopology(); }
        private bool isPolycount() => criterion == VertexTarget.TriangleCount && isFieldAligned() && isRetopology();
        private bool isPolycountRatio() => criterion == VertexTarget.Ratio && isFieldAligned() && isRetopology();

        #endregion

        private List<uint> outputMeshes = new List<uint>();
        private List<uint[]> outputDecompo = new List<uint[]>();

        private bool skinnedMesh = false;
        private GridResolutionPreset _prevGridResolutionPreset = GridResolutionPreset.Medium;
        private int _prevGridResolution = (int)GridResolutionPreset.Medium;

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

        public override void onSelectionChanged(IList<GameObject> selection)
        {
            base.onSelectionChanged(selection);
            skinnedMesh = false;
            if (!isConvexDecomposition()) { return; }
            foreach (var go in selection)
            {
                skinnedMesh = go.GetComponent<SkinnedMeshRenderer>() != null;
                if (skinnedMesh) break;
            }
        }

        public override bool preProcess(IList<GameObject> input)
        {
            switch(strategy)
            {
                case ProxyStrategy.AxisAlignedBoundingBox:
                case ProxyStrategy.OriginalMesh:
                    _input = input;
                    _output = _input;
                    break;
                default:
                    return base.preProcess(input);
            }

            return true;
        }

        protected override void process()
        {
            if (strategy == ProxyStrategy.AxisAlignedBoundingBox || strategy == ProxyStrategy.OriginalMesh)
                return;

            outputMeshes.Clear();
            outputDecompo.Clear();
            try {
                OptimizeSDK.Native.NativeInterface.PushAnalytic("CreateCollider", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                UpdateProgressBar(0.35f);
                switch (strategy)
                {
                    case ProxyStrategy.ConvexDecomposition:
                        ConvexDecompositionProcess();
                        break;
                    case ProxyStrategy.Retopology:
                        RetopologizeProcess();
                        break;
                    default:
                        break;
                }
                UpdateProgressBar(0.9f, "Post processing...");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} \n {e.StackTrace}");
            }
        }

        private void RetopologizeProcess()
        {
            uint newMesh = 0;
            AABB aabb = NativeInterface.GetAABB(Context.pixyzMeshes, Context.pixyzMatrices);

            if (type == RetopologizeType.FieldAligned)
            {
                if (isPolycount())
                {
                    newMesh = NativeInterface.RemeshFieldAligned(Context.pixyzMeshes, targetTriangleCount, Context.pixyzMatrices, true, useFeatureSize ? featureSize : -1, true);
                }
                else if (isPolycountRatio())
                {
                    newMesh = NativeInterface.RemeshFieldAlignedToRatio(Context.pixyzMeshes, (double)targetRatio.value / 100.0, Context.pixyzMatrices, true, useFeatureSize ? featureSize : -1, true);
                }
            }
            else if (type == RetopologizeType.Standard)
            {
                float featureSize = Mathf.Max((float)(aabb.high.x - aabb.low.x), (float)(aabb.high.y - aabb.low.y), (float)(aabb.high.z - aabb.low.z)) / gridResolution;
                newMesh = NativeInterface.Remesh(Context.pixyzMeshes, featureSize, isPointCloud, Context.pixyzMatrices, true);
                NativeInterface.DecimateToQualityVertexRemoval(new MeshList(new uint[] { newMesh }), 0.001, 0.0005, -1, -1, new Matrix4List(new Matrix4[] { Conversions.Identity() }));
            }
            MeshList newMeshes = new MeshList(new uint[] { newMesh });

            Context.pixyzMeshes = newMeshes;
            Context.pixyzMatrices = new Matrix4List(new Matrix4[] { Conversions.Identity() });
        }

        private void ConvexDecompositionProcess()
        {
            int vertexCount = 2 + maxTrianglesPerMesh / 2; //Euler characteristic
            int res = (int)convertRange(resolution, 10000, 64000000);
            Dictionary<uint, int> decompoDone = new Dictionary<uint, int>();

            foreach (uint mesh in Context.pixyzMeshes.list)
            {
                if(decompoDone.ContainsKey(mesh))
                {
                    outputDecompo.Add(outputDecompo[decompoDone[mesh]]);
                }
                else
                {
                    uint[] decompositions = NativeInterface.DecomposeConvex(mesh, maxDecompositionPerMesh, vertexCount, res, 0.001);
                    decompoDone.Add(mesh, outputDecompo.Count);
                    outputDecompo.Add(decompositions);
                }
            }
        }


        protected override void postProcess()
        {
            switch(strategy)
            {
                case ProxyStrategy.ConvexDecomposition:
                        PostProcessConvexDecomposition();
                    break;
                case ProxyStrategy.AxisAlignedBoundingBox:
                        PostProcessAxisAlignedBoundingBox();
                    break;
                case ProxyStrategy.OriginalMesh:
                        PostProcessOriginalMesh();
                    break;
                case ProxyStrategy.Retopology:
                        PostProcessRetopology();
                    break;
            }
        }

        private void PostProcessRetopology()
        {
            _output = Context.PixyzMeshToUnityObject(Context.pixyzMeshes);

            int polyCount = _output.GetMeshes().GetPolyCount();
            foreach (var go in (IList<GameObject>)Output)
            {
                go.name = "Retopo-" + polyCount;
                MeshCollider collider = go.AddComponent<MeshCollider>();
                MeshFilter filter = go.GetComponent<MeshFilter>();
                collider.sharedMesh = filter.sharedMesh;
                GameObject.DestroyImmediate(go.GetComponent<Renderer>());
                GameObject.DestroyImmediate(filter);
            }
        }

        private void PostProcessOriginalMesh()
        {
            foreach(GameObject gameObject in _input)
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (!meshFilter)
                    continue;
                Mesh mesh = meshFilter.sharedMesh;
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (!mesh || !meshRenderer)
                    continue;

                MeshCollider meshCollider = gameObject.GetOrAddComponent<MeshCollider>();
                meshCollider.sharedMesh = mesh;
            }
        }

        private void PostProcessAxisAlignedBoundingBox()
        {
            foreach (GameObject gameObject in _input)
            {
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (!meshFilter)
                    continue;
                Mesh mesh = meshFilter.sharedMesh;
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (!mesh || !meshRenderer)
                    continue;
                BoxCollider boxCollider = gameObject.GetOrAddComponent<BoxCollider>();
                boxCollider.center = mesh.bounds.center;
                boxCollider.size = mesh.bounds.size;
            }
        }

        private void PostProcessConvexDecomposition()
        {
            _output = (IList<GameObject>)Input;
            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < outputDecompo.Count; i++)
            {
                uint[] decomposition = outputDecompo[i];
                GameObject current = InputParts[i];
                var decompoObjects = Context.PixyzMeshToUnityObject(new MeshList(decomposition));
                var meshes = decompoObjects.GetMeshes();

                // Destroys previous colliders
                foreach (var oldCollider in current.GetComponents<MeshCollider>())
                {
                    UnityEngine.Object.DestroyImmediate(oldCollider);
                }

                // Adds new colliders
                foreach (var mesh in meshes)
                {
                    MeshCollider meshCollider = current.AddComponent<MeshCollider>();
                    meshCollider.sharedMesh = mesh;
                    meshCollider.convex = true;
                }
                gameObjects.AddRange(decompoObjects);
            }

            foreach (var go in gameObjects)
                GameObject.DestroyImmediate(go);
        }

        public override IList<string> getErrors()
        {
            var errors = new List<string>();
            if (isConvexDecomposition())
            {
                if (maxDecompositionPerMesh <= 0)
                {
                    errors.Add("Max decomposition is too low ! (must be higher than 0)");
                }
                if (maxTrianglesPerMesh <= 0)
                {
                    errors.Add("Max triangles is too low ! (must be higher than 0)");
                }
                if (maxTrianglesPerMesh > 255)
                {
                    errors.Add("Max triangles is too high ! (must be lower than 264)");
                }
            }

            if (isStandard())
            {
                if (featureSize <= 0 && useFeatureSize)
                    errors.Add("Feature size is too low ! (must be higher than 0)");
            }
            return errors.ToArray();
        }

        public override IList<string> getWarnings()
        {
            var warnings = new List<string>();
            if (isConvexDecomposition() && skinnedMesh)
            {
                warnings.Add("Selection contains Skinned Mesh Renderer but the output convex decomposition won't be animated.");
            }

            if (isStandard())
            {
                if (gridResolution >= 1000)
                    warnings.Add("Quality value is too high! (The execution can take a lot of time)");
                if (gridResolution <= 0)
                    warnings.Add("Quality value is too low! (must be higher than 0)");
            }
            return warnings;
        }

        private double convertRange(double range, double minv, double maxv)
        {
            double minp = 0;
            double maxp = 100;

            double minlog = Math.Log(minv);
            double maxlog = Math.Log(maxv);

            // calculate adjustment factor
            double scale = (maxlog - minlog) / (maxp - minp);
            return Math.Exp(minlog + scale * (range - minp));
        }

    }
}
