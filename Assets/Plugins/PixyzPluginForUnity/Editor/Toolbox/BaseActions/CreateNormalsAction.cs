using System.Collections.Generic;
using UnityEngine;
using Pixyz.OptimizeSDK.Native;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;

namespace Pixyz.Toolbox.Editor
{
    public class CreateNormals : PixyzFunction
    {
        public override int id => 219151512;
        public override int order => 14;
        public override string menuPathRuleEngine => "Normals/Create Normals";
        public override string menuPathToolbox => "Normals/Create Normals";
        public override string tooltip => ToolboxTooltips.createNormalsAction;
        protected override MaterialSyncType SyncMaterials => MaterialSyncType.SyncNone;

        [UserParameter]
        public double smoothingAngle = 25;

        [UserParameter]
        public bool areaWeighting = true;


        protected override void process()
        {
            try
            {
                UpdateProgressBar(0.25f, "Creating normals");
                NativeInterface.PushAnalytic("CreateNormals", "");
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                NativeInterface.CreateNormals(Context.pixyzMeshes, smoothingAngle, areaWeighting, true);
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
            if (smoothingAngle < 0)
            {
                errors.Add("Smoothing angle is too low ! (must be between 0 and 180)");
            }
            if (smoothingAngle > 180)
            {
                errors.Add("Smoothing angle is too high ! (must be between 0 and 180)");
            }
            return errors;
        }
    }
}
