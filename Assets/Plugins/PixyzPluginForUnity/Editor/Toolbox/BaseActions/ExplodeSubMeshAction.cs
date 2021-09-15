using System.Collections.Generic;
using UnityEngine;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public class ExplodeSubmeshesAction : ActionInOut<IList<GameObject>, IList<GameObject>>
    {
        public override int id => 91540003;
        public override int order => 11;
        public override string menuPathRuleEngine => "Hierarchy/Explode";
        public override string menuPathToolbox => "Hierarchy/Explode";
        public override string tooltip => ToolboxTooltips.explodeAction;

        public override IList<GameObject> run(IList<GameObject> input)
        {
            OptimizeSDK.Native.NativeInterface.PushAnalytic("ExplodeMeshes", "");
            UpdateProgressBar(0.5f, "Exploding meshes..");

            List<GameObject> output = new List<GameObject>();

            foreach (GameObject gameObject in input)
            {
                output.Add(gameObject);
                output.AddRange(MeshExtensions.ExplodeSubmeshes(gameObject, false));
            }
            return output;
        }
    }
}
