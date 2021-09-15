using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using Pixyz.LODTools;
using Pixyz.LODTools.Editor;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;
using Pixyz.Commons.Extensions.Editor;

namespace Pixyz.OptimizeSDK
{
    public class CustomModelPostProcessor : AssetPostprocessor
    {
        private HashSet<string> UniqueIdentifiers = new HashSet<string>();
        private string GetUniqueName(string identifier)
        {
            int i = 1;
            string uniqueIdentifier = identifier;
            while (!UniqueIdentifiers.Add(uniqueIdentifier))
            {
                uniqueIdentifier = identifier + ' ' + i;
                i++;
            }
            return uniqueIdentifier;
        }

        private void OnPostprocessModel(GameObject gameObject)
        {
            var splt = (assetImporter as ModelImporter)?.userData.Split('_');
            if (splt == null)
                return;

            if (splt.Length != 2)
                return;

            LODProcess lodProcess = (LODProcess)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(splt[0]), typeof(LODProcess));

            if (!lodProcess)
                return;

            CreateLODs(gameObject, lodProcess);

            EditorUtility.SetDirty(gameObject);
        }

        private void CreateLODs(GameObject root, LODProcess lodProcess)
        {
            PixyzContext context = new PixyzContext();
            MeshData data = GetMeshDataFromRoot(root);
            context.UnityMeshesToPixyzCreate(data);
            
            LODBuilder builder = new LODBuilder();
            var task = builder.BuildLOD(context, lodProcess, false);
            task.Wait();

            ApplyGenerationToRoot(root, builder, data.renderers);
            AddAssetsToMainAsset(root, root.GetNonPersistentDependenciesRecursive());
        }

        private void ApplyGenerationToRoot(GameObject root, LODBuilder builder, List<Renderer> renderers)
        {
            LODGroup lodGroup = root.GetComponent<LODGroup>();
            LODGenerationData generatedData = root.GetComponent<LODGenerationData>();

            UnityEngine.LOD[] lods = new UnityEngine.LOD[builder.Contexts.Length];

            if (lodGroup == null)
            {
                lodGroup = root.AddComponent<LODGroup>();
                generatedData = root.AddComponent<LODGenerationData>();
            }
            else
            {
                if(generatedData == null)
                    generatedData = root.AddComponent<LODGenerationData>();

                if (lodGroup.lodCount > 0)
                {
                    UnityEngine.LOD[] srcLods = lodGroup.GetLODs();

                    if (srcLods.Length > lods.Length)
                    {
                        lods = new UnityEngine.LOD[srcLods.Length];
                    }

                    Array.Copy(srcLods, lods, srcLods.Length);
                }
            }

            UnityEngine.LOD lod0 = lods[0];
            lod0.renderers = renderers.ToArray();
            lod0.screenRelativeTransitionHeight = 1f - 1f / builder.Contexts.Length + 0.1f;
            lods[0] = lod0;

            int count = 0;

            for (int i = 1; i < builder.Contexts.Length; ++i)
            {
                count += i;

                PixyzContext context = builder.Contexts[i];
                Renderer[] generatedRenderer = context.PixyzMeshToUnityRenderer(context.pixyzMeshes);

                UnityEngine.LOD lod = lods[i];

                if (lod.renderers != null)
                {
                    foreach (Renderer r in lod.renderers)
                    {
                        if (r == null)
                            continue;
                        GameObject.DestroyImmediate(r.gameObject);
                    }
                }

                if (lod.screenRelativeTransitionHeight == 0)
                {
                    lod.screenRelativeTransitionHeight = 1f - 1f / builder.Contexts.Length * i;
                }

                if (lod.screenRelativeTransitionHeight >= lods[i - 1].screenRelativeTransitionHeight)
                    lod.screenRelativeTransitionHeight = lods[i - 1].screenRelativeTransitionHeight * 0.5f;

                for (int j = 0; j < generatedRenderer.Length; ++j)
                {
                    count += j;

                    Renderer r = generatedRenderer[j];
                    r.gameObject.name = $"Generated LOD{i} object - {count}";

                    Matrix4x4 transform = Conversions.ConvertMatrix(context.pixyzMatrices[j]);
                    r.transform.position = transform.GetColumn(3);
                    r.transform.rotation = transform.rotation;
                    r.transform.localScale = new Vector3(transform.GetColumn(0).magnitude, transform.GetColumn(1).magnitude, transform.GetColumn(2).magnitude);
                    r.transform.parent = generatedData.transform;
                }
                lod.renderers = generatedRenderer;

                lods[i] = lod;
            }

            lodGroup.SetLODs(lods);

            builder.Contexts[0].Dispose();
        }

        private void AddAssetsToMainAsset(GameObject root, UnityEngine.Object[] dependencies)
        {
            context.AddObjectToAsset("main", root);
            context.SetMainObject(root);

            // Add created dependencies (Meshes, Textures, Materials, ...)
            foreach (var dependency in dependencies)
                context.AddObjectToAsset(GetUniqueName(dependency.name), dependency);

            // It seems like dependencies are properly added on the object
            var objs = new List<UnityEngine.Object>();
            context.GetObjects(objs);
            foreach (var obj in objs)
                Debug.Log("Found dep : " + obj);

            // Attempt force save
            // It looks like it doesn't help
            AssetDatabase.SaveAssets();
        }

        private MeshData GetMeshDataFromRoot(GameObject root)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            MeshData data = new MeshData();
            data.renderers = new List<Renderer>(renderers.Length);
            data.meshes = new List<Mesh>(renderers.Length);
            data.materials = new List<Material[]>(renderers.Length);

            for (int i = 0; i < renderers.Length; ++i)
            {
                Renderer r = renderers[i];
                if (r is SkinnedMeshRenderer)
                {
                    data.meshes.Add(((SkinnedMeshRenderer)r).sharedMesh);
                }
                else if (r is MeshRenderer)
                {
                    data.meshes.Add(r.GetComponent<MeshFilter>().sharedMesh);
                }
                else
                {
                    continue;
                }
                data.renderers.Add(renderers[i]);
                data.materials.Add(r.sharedMaterials);
            }

            return data;
        }
    }
}