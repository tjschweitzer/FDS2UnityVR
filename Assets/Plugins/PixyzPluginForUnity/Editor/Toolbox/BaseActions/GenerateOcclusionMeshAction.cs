using System.Collections.Generic;
using UnityEngine;
using Pixyz.Commons.Extensions;
using Pixyz.OptimizeSDK.Utils;
using Pixyz.OptimizeSDK.Native;
using Pixyz.OptimizeSDK.Native.Polygonal;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;

namespace Pixyz.Toolbox.Editor
{
    public class GenerateOcclusionMeshAction : PixyzFunction
    {
        public override int id => 14009905;
        public override int order => 6;
        //public override string menuPathRuleEngine => "Optimize/Generate Occlusion Mesh";
        //public override string menuPathToolbox => "Generate Occlusion Mesh";
        //public override string tooltip => "Generate occlusion meshes";
        protected override MaterialSyncType SyncMaterials => MaterialSyncType.SyncNone;

        [UserParameter]
        public CreateOccluder type = CreateOccluder.Occludee;
        private bool isOccludee() => type == CreateOccluder.Occludee;
        private bool isOccluder() => type == CreateOccluder.Occluder;

        [UserParameter(tooltip:"Minimum feature size to preserve")]
        public double featureSize = 0.010;

        [UserParameter("isOccluder", tooltip:"Number of erosion iterations")]
        public int erosionPassCount = 3;

        [UserParameter("isOccludee", tooltip:"Number of dilation iterations")]
        public int dilationPassCount = 3;

        protected override void process()
        {
            try {
                NativeInterface.PushAnalytic("CreateOcclusionMesh", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                UpdateProgressBar(0.35f);
                
                uint outputMesh = NativeInterface.CreateOcclusionMesh(Context.pixyzMeshes, type, featureSize, isOccluder() ? erosionPassCount : dilationPassCount, Context.pixyzMatrices, true);
                Context.pixyzMeshes = new MeshList(new uint[] { outputMesh });

                UpdateProgressBar(0.9f, "Post processing...");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} /n {e.StackTrace}");
            }
        }

        protected override void postProcess()
        {
            _output = Context.PixyzMeshToUnityObject(Context.pixyzMeshes);
            int polyCount = _output.GetMeshes().GetPolyCount();
            foreach (var go in _output)
            {
                go.name = "OcclusionMesh-" + polyCount;
            }
        }

        public override IList<string> getErrors()
        {
            var errors = new List<string>();
            if (featureSize <= 0)
            {
                errors.Add("Feature size is too low ! (must be higher than 0)");
            }
            if (isOccluder() && erosionPassCount < 0)
            {
                errors.Add("Erosion pass count is too low ! (must be higher than 0)");
            }
            if (isOccludee() && dilationPassCount < 0)
            {
                errors.Add("Dilation pass count is too low ! (must be higher than 0)");
            }
            return errors.ToArray();
        }
    }
}
