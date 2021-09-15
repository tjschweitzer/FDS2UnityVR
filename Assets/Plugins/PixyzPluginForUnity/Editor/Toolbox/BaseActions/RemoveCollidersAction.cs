using System.Collections.Generic;
using UnityEngine;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public class RemoveCollidersAction : ActionInOut<IList<GameObject>, IList<GameObject>>
    {
        public override int id => 142499879;
        public override int order => 1;
        public override string menuPathRuleEngine => "Colliders/Remove Colliders";
        public override string menuPathToolbox => "Colliders/Remove Colliders";
        public override string tooltip => ToolboxTooltips.removeColliderAction;

        public override IList<GameObject> run(IList<GameObject> input)
        {
            OptimizeSDK.Native.NativeInterface.PushAnalytic("RemoveColliders", "");
            float progressCount = input.Count;
            for (int i = 0; i < input.Count; i++)
            {
                var colliders = input[i].GetComponents<Collider>();
                foreach (var collider in colliders)
                {
                    Collider.DestroyImmediate(collider);
                }
                UpdateProgressBar(i / progressCount);
            }

            return input;
        }
    }
}
