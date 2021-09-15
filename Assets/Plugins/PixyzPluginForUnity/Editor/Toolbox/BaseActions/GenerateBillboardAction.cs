using System.Collections.Generic;
using UnityEngine;
using Pixyz.OptimizeSDK.Native;
using Pixyz.OptimizeSDK.Native.Polygonal;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public class GenerateBillboardAction : PixyzFunction
    {
        public override int id => 57241105;
        public override int order => 7;
        public override string menuPathRuleEngine => "Remeshing/Create Billboard";
        public override string menuPathToolbox => "Remeshing/Create Billboard";
        public override string tooltip => ToolboxTooltips.billboardAction;

        [UserParameter(tooltip: ToolboxTooltips.billboardResolution)]
        public MapDimensions mapsResolution = MapDimensions._1024;
        private bool isCustom() { return mapsResolution == MapDimensions.Custom; }

        [UserParameter("isCustom", tooltip:"Output maps resolution")]
        public int resolution = 1024;

        [UserParameter]
        public bool xPositive = true;

        [UserParameter]
        public bool xNegative = true;

        [UserParameter]
        public bool yPositive = true;

        [UserParameter]
        public bool yNegative = true;

        [UserParameter]
        public bool zPositive = true;

        [UserParameter]
        public bool zNegative = true;

        protected override void process()
        {
            try
            {
                NativeInterface.PushAnalytic("GenerateBillboard", "");
                UpdateProgressBar(0.25f);
                NativeInterface.WeldVertices(Context.pixyzMeshes, 0.0000001, Context.pixyzMatrices);

                uint outputMesh = NativeInterface.CreateBillboard(Context.pixyzMeshes, Context.pixyzMatrices, mapsResolution == MapDimensions.Custom ? resolution : (int)mapsResolution, xPositive, xNegative, yPositive, yNegative, zPositive, zNegative, true);
                Context.pixyzMeshes = new MeshList(new uint[] { outputMesh });

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
            
            foreach (GameObject go in _output)
            {
                go.name = "Imposter";
                Material material = go.GetComponent<MeshRenderer>().sharedMaterial;
                material.SetFloat("_Mode", 2);
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;

            }
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
                if (resolution > 8192)
                {
                    errors.Add("Maps resolution is too high ! (must be between 64 and 8192)");
                }
            }
            return errors.ToArray();
        }
    }
}