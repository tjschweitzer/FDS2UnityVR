using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Pixyz.OptimizeSDK.Utils;
using Pixyz.Commons.Extensions.Editor;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{

    public class Merge : ActionInOut<IList<GameObject>, IList<GameObject>>
    {
        public override int id => 511763496;
        public override int order => 8;
        public override string menuPathRuleEngine => "Hierarchy/Merge";
        public override string menuPathToolbox => "Hierarchy/Merge";
        public override string tooltip => ToolboxTooltips.mergeAction;

        [UserParameter(tooltip: ToolboxTooltips.mergeKeepParent)]
        public bool keepParent = false;

        private bool skinnedMesh = false;

        public override void onSelectionChanged(IList<GameObject> selection)
        {
            base.onSelectionChanged(selection);
            skinnedMesh = false;

            foreach (var go in selection)
            {
                Renderer r = go.GetComponent<Renderer>();
                if (r == null)
                    continue;

                if (r is SkinnedMeshRenderer && !skinnedMesh)
                {
                    skinnedMesh = true;
                }
            }
        }

        public override IList<GameObject> run(IList<GameObject> input)
        {
            OptimizeSDK.Native.NativeInterface.PushAnalytic("Merge", "");
            Regex regex = new Regex("_LOD[1-9]$");

            if (keepParent)
            {
                HashSet<GameObject> selectedGameObjects = new HashSet<GameObject>(input);
                HashSet<GameObject> highestSelectedAncestors = new HashSet<GameObject>();
                for (int i = 0; i < input.Count; i++)
                {
                    Transform current = input[i].transform;
                    GameObject highestSelectedAncestor = null;
                    while (current)
                    {
                        if (selectedGameObjects.Contains(current.gameObject))
                        {
                            highestSelectedAncestor = current.gameObject;
                        }
                        current = current.parent;
                    }
                    highestSelectedAncestors.Add(highestSelectedAncestor);
                }

                foreach (GameObject gameObject in highestSelectedAncestors)
                {
                    gameObject.MergeChildren();
                }

                return highestSelectedAncestors.ToArray();

            }
            else
            {

                GameObject highestSelectedAncestor = SceneExtensions.GetHighestAncestor(input);

                MergingContainer meshTransfer = new MergingContainer();

                for (int i = 0; i < input.Count; i++)
                {

                    if (!input[i])
                        continue;

                    if (!highestSelectedAncestor)
                        continue;

                    if (!regex.IsMatch(input[i].name))
                    { // Don't merge LODs lower than 0
                        meshTransfer.addGameObject(input[i], highestSelectedAncestor.transform);
                    }

                    if (input[i] == highestSelectedAncestor)
                        continue;

                    foreach (Transform child in input[i].transform.GetEnumerator().ToEnumerable<Transform>().ToArray())
                    {
                        if (input[i].transform.parent)
                        {
                            SceneExtensionsEditor.SetParentSafe(child, input[i].transform.parent, true);
                        }
                        else
                        {
                            SceneExtensionsEditor.SetParentSafe(child, null, true);
                        }
                    }
                }

                if (meshTransfer.vertexCount > 0)
                {
                    highestSelectedAncestor.GetOrAddComponent<MeshFilter>().sharedMesh = meshTransfer.getMesh();
                    highestSelectedAncestor.GetOrAddComponent<MeshRenderer>().sharedMaterials = meshTransfer.sharedMaterials;
                }

                for (int i = 0; i < input.Count; i++)
                {
                    if (input[i] != highestSelectedAncestor)
                    {
                        SceneExtensionsEditor.DestroyImmediateSafe(input[i]);
                    }
                }

                return new GameObject[] { highestSelectedAncestor };
            }
        }

        public override IList<string> getWarnings()
        {
            var warnings = new List<string>();
            if (skinnedMesh)
            {
                warnings.Add("Selection contains Skinned Mesh Renderer.\nMerge is not possible with SkinnedMesh.");
            }

            return warnings;
        }
    }
}