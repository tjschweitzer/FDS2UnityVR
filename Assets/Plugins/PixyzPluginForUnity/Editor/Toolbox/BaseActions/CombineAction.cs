using System.Collections.Generic;
using UnityEngine;
using Pixyz.OptimizeSDK;
using Pixyz.OptimizeSDK.Native;
using Pixyz.OptimizeSDK.Native.Polygonal;
using Pixyz.OptimizeSDK.Native.Geom;
using Pixyz.OptimizeSDK.Native.Algo;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public class CombineAction : PixyzFunction
    {
        public override int id => 57243405;
        public override int order => 9;
        public override string menuPathRuleEngine => "Hierarchy/Combine";
        public override string menuPathToolbox => "Hierarchy/Combine";
        public override string tooltip => ToolboxTooltips.combineAction;

        [UserParameter(tooltip: ToolboxTooltips.combineMapResolution)]
        public MapDimensions mapsResolution = MapDimensions._1024;

        [UserParameter(displayName: "Recreate UV", tooltip: ToolboxTooltips.combineUVGen)]
        public bool forceUVGeneration = false;
        private bool isCustom() { return mapsResolution == MapDimensions.Custom; }

        [UserParameter("isCustom", tooltip:"Output maps resolution")]
        public int resolution = 1024;

        protected override void process()
        {
            try
            {
                OptimizeSDK.Native.NativeInterface.PushAnalytic("Combine", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);
                UpdateProgressBar(0.43f);
                uint outputMesh = NativeInterface.CombineMeshes(Context.pixyzMeshes, Context.pixyzMatrices, forceUVGeneration, mapsResolution == MapDimensions.Custom ? resolution : (int)mapsResolution, 1, BakingMethod.RayOnly);

                Context.pixyzMeshes = new MeshList(new uint[] { outputMesh });
                Context.pixyzMatrices = new Matrix4List(new Matrix4[] { Conversions.Identity() });

                UpdateProgressBar(1f);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} /n {e.StackTrace}");
            }
        }

        protected override void postProcess()
        {
            _output = Context.PixyzMeshToUnityObject(Context.pixyzMeshes);
            IList<GameObject> gosInput = (IList<GameObject>)Input;

            string rootName = gosInput[gosInput.Count - 1].name;
            foreach (GameObject go in (IList<GameObject>)Output)
            {
                go.name = rootName;
            }
            DisableInput();
        }

        public override IList<string> getErrors()
        {
            var errors = new List<string>();
            if (isCustom())
            {
                if (resolution < 64)
                {
                    errors.Add("Maps resolution is too low ! (must be between 64 and 8192)");
                }
                else if (resolution > 8192)
                {
                    errors.Add("Maps resolution is too high ! (must be between 64 and 8192)");
                }
            }
            return errors.ToArray();
        }
    }
}