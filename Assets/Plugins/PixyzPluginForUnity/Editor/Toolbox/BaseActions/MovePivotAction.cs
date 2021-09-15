using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Pixyz.Commons.Extensions;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Toolbox.Editor
{
    public class MovePivotAction : ActionInOut<IList<GameObject>, IList<GameObject>>
    {
        public override int id => 99541656;
        public override int order => 12;
        public override string menuPathRuleEngine => "Pivot/Move Pivot";
        public override string menuPathToolbox => "Pivot/Move Pivot";
        public override string tooltip => ToolboxTooltips.movePivotAction;

        public enum MovePivotOption
        {
            ToCenterOfBoundingBox,
            ToMininumOfBoundingBox,
            ToMaximumOfBoundingBox,
            ToCustom,
        }

        [UserParameter]
        public MovePivotOption target;

        [UserParameter("isPivotCustom")]
        public Vector3 customPosition;

        [UserParameter("isPivotCustom")]
        public bool worldSpace = false;

        private bool isPivotCustom()
        {
            return target == MovePivotOption.ToCustom;
        }

        public override IList<GameObject> run(IList<GameObject> input)
        {
            OptimizeSDK.Native.NativeInterface.PushAnalytic("MovePivot", "");
            var selectedGameObjects = new HashSet<GameObject>(input);
            var highestSelectedAncestors = new HashSet<GameObject>();
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
                Vector3 center = new Vector3();
                switch (target)
                {
                    case MovePivotOption.ToCenterOfBoundingBox:
                        center = gameObject.GetBoundsWorldSpace(true).center;
                        break;
                    case MovePivotOption.ToMaximumOfBoundingBox:
                        center = gameObject.GetBoundsWorldSpace(true).max;
                        break;
                    case MovePivotOption.ToMininumOfBoundingBox:
                        center = gameObject.GetBoundsWorldSpace(true).min;
                        break;
                    case MovePivotOption.ToCustom:
                        center = customPosition;
                        if (!worldSpace && gameObject.transform.parent != null)
                        {
                            center = gameObject.transform.parent.TransformPoint(center);
                            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            sphere.transform.position = center;
                        }
                        break;
                }

                // Move transforms 
                Vector3 delta = gameObject.transform.position - center;
                Vector3 localDelta = gameObject.transform.InverseTransformVector(delta);
                gameObject.transform.position = center;
                foreach (Transform child in gameObject.transform)
                {
                    child.position += delta;
                }

                // Move mesh
                MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                if (meshFilter)
                {
                    Mesh mesh = Mesh.Instantiate(meshFilter.sharedMesh);
                    mesh.name = meshFilter.sharedMesh.name;
                    var vertices = mesh.vertices;
                    for (int j = 0; j < vertices.Length; j++)
                    {
                        vertices[j] += localDelta;
                    }
                    mesh.vertices = vertices;
                    mesh.RecalculateBounds();
                    meshFilter.sharedMesh = mesh;
                }

                // Move collider
                BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
                if (boxCollider)
                {
                    boxCollider.center += localDelta;
                }
                SphereCollider sphereCollider = gameObject.GetComponent<SphereCollider>();
                if (sphereCollider)
                {
                    sphereCollider.center += localDelta;
                }
                CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
                if (capsuleCollider)
                {
                    capsuleCollider.center += localDelta;
                }
                MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
                if (meshCollider)
                {
                    Mesh mesh = Mesh.Instantiate(meshCollider.sharedMesh);
                    mesh.name = meshCollider.sharedMesh.name;
                    var vertices = mesh.vertices;
                    for (int j = 0; j < vertices.Length; j++)
                    {
                        vertices[j] += localDelta;
                    }
                    mesh.vertices = vertices;
                    mesh.RecalculateBounds();
                    meshCollider.sharedMesh = mesh;
                }
            }

            return highestSelectedAncestors.ToArray();
        }
    }
}