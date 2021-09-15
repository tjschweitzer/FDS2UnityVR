using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;
using Pixyz.Toolbox.Editor;
using Pixyz.Commons.UI.Editor;

namespace Pixyz.Commons.Extensions.Editor
{
    public static class SceneExtensionsEditor
    {
        /// <summary>
        /// Collect all source data (Renderers, meshs, materials)
        /// </summary>
        public static MeshData GetMeshData(this IList<GameObject> input)
        {
            var data = new MeshData();

            foreach (GameObject go in input)
            {
                if (go.GetMeshData(out Renderer renderer, out Mesh mesh, out Material[] materials))
                {
                    data.renderers.Add(renderer);
                    data.meshes.Add(mesh);
                    data.materials.Add(materials);
                }
            }
            return data;
        }

        public static bool GetMeshData(this GameObject gameObject, out Renderer renderer, out Mesh mesh, out Material[] materials)
        {
            mesh = null;
            materials = null;

            renderer = gameObject.GetComponent<Renderer>();

            switch (renderer)
            {
                case MeshRenderer meshRenderer:
                    materials = meshRenderer.sharedMaterials;
                    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                    if (meshFilter == null)
                        return false;
                    mesh = meshFilter.sharedMesh;
                    return true;
                case SkinnedMeshRenderer skinnedMeshRenderer:
                    mesh = skinnedMeshRenderer.sharedMesh;
                    materials = skinnedMeshRenderer.sharedMaterials;
                    return true;
                default:
                    // Non handled Renderer type
                    return false;
                case null:
                    // No renderer on this GameObject
                    return false;
            }
        }

        public static bool AutoUnpack = false;
        private static bool dontShowUnpackAgain = false;

        private static bool ShowUnpackDialog()
        {
            if (dontShowUnpackAgain || AutoUnpack) return true;

            int value = UnityEditor.EditorUtility.DisplayDialogComplex("Invalid prefab operation", "Pixyz has detected that at least one of the GameObjects is linked to a prefab. However, the running action will break the prefab instance.\n\n Would you like to unpack the prefab and continue ?", "Yes, unpack and continue", "No, abort", "Yes, unpack, continue and don't show again");
            dontShowUnpackAgain = value == 2;
            return value != 1;
        }

        /// <summary>
        /// Change Tranform's parent, but ensures it is properly unpack if transform is part of a prefab (if editor)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="parent"></param>
        /// <param name="worldPositionStays"></param>
        public static void SetParentSafe(this Transform transform, Transform parent, bool worldPositionStays = true)
        {
            if (!transform)
                throw new NullReferenceException();
            if (transform == parent)
                return;

            var transformPrefabStatus = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(transform);
            if (transformPrefabStatus == UnityEditor.PrefabInstanceStatus.Connected || transformPrefabStatus == UnityEditor.PrefabInstanceStatus.MissingAsset)
            {
                if (ShowUnpackDialog())
                {
                    UnityEditor.PrefabUtility.UnpackPrefabInstance(UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(transform.gameObject), UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);
                }
                else
                {
                    throw new PrefabModificationException();
                }
            }
            var parentPrefabStatus = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(transform);
            if (parentPrefabStatus == UnityEditor.PrefabInstanceStatus.Connected || parentPrefabStatus == UnityEditor.PrefabInstanceStatus.MissingAsset)
            {
                if (ShowUnpackDialog())
                {
                    UnityEditor.PrefabUtility.UnpackPrefabInstance(UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(parent.gameObject), UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);
                }
                else
                {
                    throw new PrefabModificationException();
                }
            }

            transform.SetParent(parent, worldPositionStays);
        }

        /// <summary>
        /// Destroys GameObject, but ensure that it is properly unpacked if gameobject is part of a prefab (if editor)
        /// </summary>
        /// <param name="gameObject"></param>
        public static void DestroyImmediateSafe(this GameObject gameObject)
        {
            if (!gameObject)
                return;

            var prefabStatus = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(gameObject);
            if (prefabStatus == UnityEditor.PrefabInstanceStatus.Connected || prefabStatus == UnityEditor.PrefabInstanceStatus.MissingAsset)
            {
                if (ShowUnpackDialog())
                {
                    UnityEditor.PrefabUtility.UnpackPrefabInstance(UnityEditor.PrefabUtility.GetNearestPrefabInstanceRoot(gameObject), UnityEditor.PrefabUnpackMode.Completely, UnityEditor.InteractionMode.AutomatedAction);
                }
                else
                {
                    throw new PrefabModificationException();
                }
            }

            GameObject.DestroyImmediate(gameObject);
        }

        /// <summary>
        /// Merge children (recursively) of given GameObject.
        /// If given GameObject has no mesh or renderer, this method will create it with the merged content inside.
        /// </summary>
        /// <param name="gameObject"></param>
        public static void MergeChildren(this GameObject gameObject)
        {
            Regex regex = new Regex("_LOD[1-9]$");

            MergingContainer meshTransfer = new MergingContainer();
            meshTransfer.addGameObject(gameObject);
            var gameObjects = gameObject.GetChildren(true, false);
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (!regex.IsMatch(gameObjects[i].name))
                { // Don't merge LODs lower than 0
                    meshTransfer.addGameObject(gameObjects[i], gameObject.transform);
                }
            }

            if (meshTransfer.vertexCount > 0)
            {
                gameObject.GetOrAddComponent<MeshFilter>().sharedMesh = meshTransfer.getMesh();
                gameObject.GetOrAddComponent<MeshRenderer>().sharedMaterials = meshTransfer.sharedMaterials;
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                DestroyImmediateSafe(gameObjects[i]);
            }
        }
    }
}