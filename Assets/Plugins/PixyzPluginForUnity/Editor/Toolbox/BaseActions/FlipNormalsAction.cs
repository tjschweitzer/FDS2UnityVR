using System.Collections.Generic;
using UnityEngine;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public class FlipNormals : ActionInOut<IList<GameObject>, IList<GameObject>>
    {
        public override int id => 10191513;
        public override int order => 13;
        public override string menuPathRuleEngine => "Normals/Flip Normals";
        public override string menuPathToolbox => "Normals/Flip Normals";
        public override string tooltip => ToolboxTooltips.flipNormalsAction;

        [UserParameter]
        public bool includeTriangleNormals = true;

        public override IList<GameObject> run(IList<GameObject> input)
        {
            OptimizeSDK.Native.NativeInterface.PushAnalytic("FlipNormals", "");
            UpdateProgressBar(0.5f, "Flipping Normals..");

            foreach (Mesh mesh in input.GetMeshesUnique())
            {
                if (includeTriangleNormals)
                {
                    for (int s = 0; s < mesh.subMeshCount; s++)
                    {
                        if (mesh.GetTopology(s) != MeshTopology.Triangles)
                            continue;
                        int[] triangles = mesh.GetTriangles(s);
                        for (int i = 0; i < triangles.Length; i += 3)
                        {
                            int x = triangles[i];
                            triangles[i] = triangles[i + 1];
                            triangles[i + 1] = x;
                        }
                        mesh.SetTriangles(triangles, s);
                    }

                }
                Vector3[] normals = mesh.normals;
                for (int n = 0; n < normals.Length; n++)
                {
                    normals[n] = -normals[n];
                }
                mesh.normals = normals;
            }
            return input;
        }
    }
}
