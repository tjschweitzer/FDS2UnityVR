using System.Collections.Generic;
using UnityEngine;
using Pixyz.OptimizeSDK.Native;
using Pixyz.Commons.UI.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;

namespace Pixyz.Toolbox.Editor
{
    public class RepairMeshAction : PixyzFunction
    {
        public override int id => 1455705;
        public override int order => 2;
        public override string menuPathRuleEngine => "Mesh/Repair";
        public override string menuPathToolbox => "Mesh/Repair";
        public override string tooltip => ToolboxTooltips.repairAction;
        protected override MaterialSyncType SyncMaterials => MaterialSyncType.SyncNone;

        [UserParameter(tooltip: ToolboxTooltips.repairDistanceTolerance)]
        public double distanceTolerance = 0.0001;

        [UserParameter(tooltip: ToolboxTooltips.repairOrientFaces)]
        public bool orientFaces = false;

        protected override void process()
        {
            try
            {
                UpdateProgressBar(0.25f, "Repairing meshes..");
                NativeInterface.RepairMeshes(Context.pixyzMeshes, distanceTolerance, true, orientFaces, Context.pixyzMatrices);
                OptimizeSDK.Native.NativeInterface.PushAnalytic("RepairMeshes", $"{Context.pixyzMeshes}, {distanceTolerance}, true, false, {Context.pixyzMatrices}");
                UpdateProgressBar(1f);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} /n {e.StackTrace}");
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
            if (distanceTolerance <= 0)
            {
                errors.Add("Distance tolerance is too low ! (must be higher than 0)");
            }
            return errors.ToArray();
        }
    }
}