using Pixyz.Plugin4Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pixyz.Commons.Extensions
{
    public static class MeshExtensions
    {
        public static uint GetVerticesCount(this Mesh mesh, int submesh)
        {
            if (submesh < mesh.subMeshCount - 1)
            {
                return mesh.GetBaseVertex(submesh + 1) - mesh.GetBaseVertex(submesh);
            }
            else
            {
                return (uint)mesh.vertexCount - mesh.GetBaseVertex(submesh);
            }
        }

        /// <summary>
        /// Returns the polycount of a given mesh.
        /// Only counts triangles.
        /// </summary>
        /// <param name="mesh"></param>
        /// <returns></returns>
        public static int GetPolycount(this Mesh mesh)
        {
            if (mesh == null)
                return 0;
            uint polycount = 0;
            for (int s = 0; s < mesh.subMeshCount; s++)
            {
                if (mesh.GetTopology(s) == MeshTopology.Triangles)
                {
                    polycount += mesh.GetIndexCount(s) / 3;
                }
            }
            return (int)polycount;
        }

        public static int GetPolyCount(this Mesh[] meshes)
        {
            int polyCount = 0;
            foreach (var mesh in meshes)
            {
                polyCount += mesh.GetPolycount();
            }
            return polyCount;
        }

        public static Mesh GetSubmesh(this Mesh aMesh, int aSubMeshIndex)
        {
            if (aSubMeshIndex < 0 || aSubMeshIndex >= aMesh.subMeshCount)
                return null;

            int[] indices = aMesh.GetIndices(aSubMeshIndex);
            MeshTopology topology = aMesh.GetTopology(aSubMeshIndex);
            //int[] indices = aMesh.GetTriangles(aSubMeshIndex);
            VerticesData source = new VerticesData(aMesh);
            VerticesData dest = new VerticesData();
            Dictionary<int, int> map = new Dictionary<int, int>();
            int[] newIndices = new int[indices.Length];
            for (int i = 0; i < indices.Length; i++)
            {
                int o = indices[i];
                int n;
                if (!map.TryGetValue(o, out n))
                {
                    n = dest.Add(source, o);
                    map.Add(o, n);
                }
                newIndices[i] = n;
            }
            Mesh m = new Mesh();
            dest.AssignTo(m);
            m.SetIndices(newIndices, topology, 0);
            return m;
        }

        public static List<GameObject> ExplodeSubmeshes(this GameObject gameObject, bool firstSubmeshAsParent)
        {

            List<GameObject> newGameObjects = new List<GameObject>();

            MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>(); ;
            if (!meshFilter)
                return newGameObjects;

            Mesh mesh = meshFilter.sharedMesh;
            if (!mesh)
                return newGameObjects;

            Material[] materials;
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer)
            {
                materials = renderer.sharedMaterials;
                Array.Resize(ref materials, mesh.subMeshCount);
            }
            else
            {
                materials = new Material[mesh.subMeshCount];
            }

            if (firstSubmeshAsParent)
            {
                for (int s = 0; s < mesh.subMeshCount; s++)
                {
                    Mesh submesh = mesh.GetSubmesh(s);
                    if (s == 0)
                    {
                        meshFilter.sharedMesh = submesh;
                        renderer.sharedMaterials = new Material[] { materials[s] };
                    }
                    else
                    {
                        var child = new GameObject(string.IsNullOrEmpty(materials[s].name) ? "Unamed Material" : materials[s].name);
                        child.transform.SetParent(gameObject.transform);
                        child.transform.localPosition = Vector3.zero;
                        child.transform.localRotation = Quaternion.identity;
                        child.transform.localScale = Vector3.one;
                        child.GetOrAddComponent<MeshFilter>().sharedMesh = submesh;
                        child.GetOrAddComponent<MeshRenderer>().sharedMaterials = new Material[] { materials[s] };
                        newGameObjects.Add(child);
                    }
                }
            }
            else
            {
                for (int s = 0; s < mesh.subMeshCount; s++)
                {
                    Mesh submesh = mesh.GetSubmesh(s);
                    var child = new GameObject(string.IsNullOrEmpty(materials[s].name) ? "Unamed Material" : materials[s].name);
                    child.transform.SetParent(gameObject.transform);
                    child.transform.localPosition = Vector3.zero;
                    child.transform.localRotation = Quaternion.identity;
                    child.transform.localScale = Vector3.one;
                    child.GetOrAddComponent<MeshFilter>().sharedMesh = submesh;
                    child.GetOrAddComponent<MeshRenderer>().sharedMaterials = new Material[] { materials[s] };
                    newGameObjects.Add(child);
                }
                GameObject.DestroyImmediate(renderer);
                GameObject.DestroyImmediate(meshFilter);
            }

            return newGameObjects;
        }
    }

}