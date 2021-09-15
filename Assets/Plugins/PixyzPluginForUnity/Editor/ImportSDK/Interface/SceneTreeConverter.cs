using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Pixyz.Commons.Utilities;
using Pixyz.ImportSDK.Native.Scene;
using System.Globalization;
using Pixyz.Plugin4Unity;
using Pixyz.ImportSDK.Native;
using Pixyz.ImportSDK.Native.Core;
using System;
using Pixyz.ImportSDK.Native.Polygonal;
using Pixyz.ImportSDK.Native.Material;
using System.Net.NetworkInformation;
using Pixyz.Commons.Extensions;
using UnityEditor.UIElements;
using System.Linq;

namespace Pixyz.ImportSDK
{
    public class SceneTreeConverter
    {
        // workaround for bug :
        // https://issuetracker.unity3d.com/issues/objects-get-rendered-black-slash-dark-color-when-their-scale-is-ridiculously-small
        private const bool applyScaleOnVertices = true;

        private const string LOD_NUMBER_ATTR = "PXZ_LOD_No";
        public GameObject gameObject { get; private set; }

        private int polyCount;
        public int PolyCount => polyCount;

        private int objectCount;
        public int ObjectCount => objectCount;

        private ImportSettings settings;
        private Dictionary<uint, Material> uniqueMaterials = new Dictionary<uint, Material>();
        private Dictionary<UnityEngine.Color, Material> uniqueLineColor = new Dictionary<UnityEngine.Color, Material>();
        private Dictionary<uint, Texture2D> textures = new Dictionary<uint, Texture2D>();
        private MeshDefinitionList meshDefinitions;
        private Mesh[] meshes;
        private MetadataDefinitionList metadataByOccurrences;

        private VoidHandler conversionCallback;
        private bool isCompleted = false;
        private bool isReadyForCompletion = false;
        private bool isAsynchronous;

        public PackedTree scene;
        private string _file;
        private int _meshDefConverterProcessedCount = 0;
        List<GameObject> gameObjects = new List<GameObject>();
        private List<LODGroup> lodGroups = new List<LODGroup>();
        public SceneTreeConverter(PackedTree packedTree, string file, ImportSettings settings, VoidHandler conversionCallback = null, bool isAsynchronous = false)
        {
            this.settings = settings;
            this.scene = packedTree;
            this.conversionCallback = conversionCallback;
            this.isAsynchronous = isAsynchronous;
            _file = file;
        }

        public void convert()
        {
            var metadatas = NativeInterface.GetComponentByOccurrence(scene.occurrences, ComponentType.Metadata, true);
            MetadataList metadataList = new MetadataList(metadatas.list);
            
            metadataByOccurrences =  NativeInterface.GetMetadatasDefinitions(metadataList);
            createTextures();
            createMaterials();
            createMeshes();
            createTree(scene);
            setRootTransform();

            isReadyForCompletion = true;
            checkCompletion();
        }

        private void setRootTransform()
        {
            if(gameObject)
            {
                Matrix4x4 transform = Matrix4x4.identity;
                if(this.settings.scaleFactor != 1 && !applyScaleOnVertices)
                {
                    Matrix4x4 scale = Matrix4x4.identity;
                    scale.m00 = scale.m11 = scale.m22 = this.settings.scaleFactor;
                    transform = transform * scale;
                }

                if (!this.settings.isLeftHanded)
                {
                    Matrix4x4 symm = Matrix4x4.identity;
                    symm.m00 = -1.0f;
                    transform = transform * symm;
                }

                if (this.settings.isZUp)
                {
                    Matrix4x4 rotate = Matrix4x4.identity;
                    rotate.m11 = rotate.m22 = 0.0f;
                    rotate.m12 = 1.0f;
                    rotate.m21 = -1.0f;
                    transform = transform * rotate;
                }

                Matrix4x4 matrix = MathExtensions.GetLocalMatrix(gameObject.transform);
                MathExtensions.SetFromLocalMatrix(gameObject.transform, matrix * transform);
            }
        }

        private void meshDefConverterProcessed(MeshDefinitionToMesh meshDefinitionConverter)
        {
            _meshDefConverterProcessedCount++;
            checkCompletion();
        }

        private void checkCompletion()
        {
            if (isReadyForCompletion && !isCompleted && _meshDefConverterProcessedCount >= meshes.Length)
            {
                if (conversionCallback != null)
                {
                    isCompleted = true;
                    conversionCallback?.Invoke();
                    clear();
                }
            }
        }

        void createTree(PackedTree sceneTree)
        {
            for (int i = 0; i < sceneTree.occurrences.length; i++)
            {

                //Create an object
                GameObject newGameObject = extractObject(sceneTree, i);
                gameObjects.Add(newGameObject);

                //Set Parent 
                if (i != 0)
                {
                    int parentIndex = sceneTree.parents[i];
                    newGameObject.transform.SetParent(gameObjects[parentIndex].transform, false);

                }
                else
                {
                    if (!gameObject)
                        gameObject = newGameObject;

                }

                // LOD
                // Check if GameObject name is of type "XXXXXXXXX_LODN"
                // If that's the case, we register this LOD to a LOD group, on its parent or on the root object (depending on settings)
                if (settings.lodCount > 1)
                {
                    string[] split = newGameObject.name.Split(new[] { "_LOD" }, StringSplitOptions.None);
                    int lodn;
                    Renderer renderer = newGameObject.GetComponent<MeshRenderer>();
                    GameObject lodGroupHolder = (settings.lodsMode == LODTools.LodGroupPlacement.ROOT) ? gameObject : newGameObject.transform.parent?.gameObject;
                    if (split.Length > 1 && int.TryParse(split[split.Length - 1], out lodn))
                    {
                        // If GameObject has a renderer to register as a LOD and if there is a GameObject to register the LOD on, we do it.
                        if (renderer && lodGroupHolder)
                        {
                            LODGroup lodGroup;
                            lodGroup = lodGroupHolder.GetComponent<LODGroup>();
                            if (!lodGroup)
                            {
                                lodGroup = lodGroupHolder.AddComponent<LODGroup>();
                                lodGroups.Add(lodGroup);
                                lodGroup.SetLODs(new LOD[1]);
                            }
                            var lods = lodGroup.GetLODs();
                            if (lodn > lods.Length - 1)
                            {
                                Array.Resize(ref lods, lodn + 1);
                            }
                            float threshold = (float)settings.qualities.lods[lodn].threshold;
                            //threshold = (float)settings.qualities.lods[lodn].threshold;
                            Metadata metadata = newGameObject.GetComponent<Metadata>();
                            if (metadata && metadata.containsProperty("PXZ_LOD_TRANSITION"))
                            {
                                threshold = Mathf.Clamp(float.Parse(metadata.getProperty("PXZ_LOD_TRANSITION"), CultureInfo.InvariantCulture), 0f, 0.99f);
                            }
                            else
                            {
                                threshold = (float)settings.qualities.lods[lodn].threshold;
                            }
                            lods[lodn].screenRelativeTransitionHeight = threshold;

                            if (lods[lodn].renderers == null)
                                lods[lodn].renderers = new Renderer[0];
                            //Append new renderer
                            var renderList = lods[lodn].renderers.ToList();
                            renderList.Add(renderer);
                            lods[lodn].renderers = renderList.ToArray();

                            lodGroup.SetLODs(lods);
                        }
                    }

                }
                //end LOD
            }

            string filename = Path.GetFileName(_file);
            string extension = Path.GetExtension(filename).ToLower();

            if (extension == ".pxz")
                gameObject.name = filename;
        }

        #region Materials

        private Dictionary<string, Material> _materialsInResources;
        private Dictionary<string, Material> materialsInResources
        {
            get
            {
                if (_materialsInResources == null)
                {
                    _materialsInResources = new Dictionary<string, Material>();
                    foreach (var material in Resources.LoadAll<Material>(""))
                    {
                        if (_materialsInResources.ContainsKey(material.name))
                        {
                            Debug.LogWarning($"There are multiple materials with the name '{material.name}' in your resources.");
                        }
                        else
                        {
                            _materialsInResources.Add(material.name, material);
                        }
                    }
                }
                return _materialsInResources;
            }
        }
        void createMaterials()
        {
            var allMaterials = NativeInterface.GetAllMaterials();
            HashSet<uint> uniqueMats = new HashSet<uint>();

            foreach(uint matId in allMaterials.list)
            {
                if(!uniqueMaterials.ContainsKey(matId))
                {
                    uniqueMats.Add(matId);
                }
            }

            var materialDefList = NativeInterface.GetMaterialDefinitions(new MaterialList(uniqueMats.ToArray()));
            var materials = Conversions.ConvertMaterialExtracts(materialDefList, ref textures, settings.shader);

            for (int i=0; i< materialDefList.length; i++)
            {
                uniqueMaterials.Add(materialDefList[i].id, materials[i]);
            }
        }

        private Material getMaterial(uint materialId)
        {
            Material material;
            if(uniqueMaterials.TryGetValue(materialId, out material))
            {
                return material;
            }
            else
            {
                return getDefaultMaterial();
            }
        }

        private Material getLineMaterial(UnityEngine.Color color)
        {
            if (uniqueLineColor.ContainsKey(color))
            {
                return uniqueLineColor[color];
            }
            else
            {
                Shader shader = ShaderUtilities.GetPixyzLineShader();
                if (shader == null)
                    throw new Exception("Shader 'Pixyz/Simple Lines' not found. Make sure it is in your project and included in your build if you are running from a build.");
                Material material = new Material(shader);
                material.name = "Line #" + ColorUtility.ToHtmlStringRGB(color);
                material.color = color;
                uniqueLineColor.Add(color, material);
                return material;
            }
        }

        private Material _defaultMaterial;
        private Material getDefaultMaterial()
        {
            if (_defaultMaterial == null)
            {
                _defaultMaterial = new Material(settings.shader ?? ShaderUtilities.GetDefaultShader());
                _defaultMaterial.name = "DefaultMaterial";
                _defaultMaterial.color = new UnityEngine.Color(0.5f, 0.5f, 0.5f, 1f);
                uniqueMaterials.Add(0, _defaultMaterial);
            }
            return _defaultMaterial;
        }

        private Material _pointMaterial;
        private Material getPointMaterial()
        {
            if (_pointMaterial == null)
            {
                var shader = Shader.Find("Pixyz/Splats");
                if (shader == null)
                    throw new Exception("Shader 'Pixyz/Splats' not found. Make sure it is in your project and included in your build if you are running from a build.");
                _pointMaterial = new Material(shader);
                _pointMaterial.name = "Point Unlit";
            }
            return _pointMaterial;
        }
        
        private void createTextures()
        {
            var allImages = NativeInterface.GetAllImages();
            
            for (int i = 0; i < allImages.length; i++)
            {
                if (allImages[i] == 0)
                    continue;
                var imageDef = NativeInterface.GetImageDefinition(allImages[i]);
                if (!textures.ContainsKey(imageDef.id))
                    textures.Add(
                        imageDef.id,
                        Conversions.ConvertImageDefinition(imageDef));
                
            }
        }
        #endregion

        #region Meshes
        void createMeshes()
        {
            var parts = NativeInterface.GetComponentByOccurrence(scene.occurrences, ComponentType.Part, true);
            PartList partList = new PartList(parts.list);
            MeshList meshList = NativeInterface.GetPartsMeshes(partList);
            Dictionary<uint, Mesh> convertesMesh = new Dictionary<uint, Mesh>();

            meshDefinitions = NativeInterface.GetMeshDefinitions(meshList);
            //meshDefConverters = new MeshDefinitionToMesh[meshDefinitions.length];
            meshes = new Mesh[meshDefinitions.length];
            for(int i = 0; i < meshDefinitions.length; i++)
            {
                if (convertesMesh.ContainsKey(meshDefinitions[i].id))
                {
                    meshes[i] = convertesMesh[meshDefinitions[i].id];
                    continue;
                }

                Mesh mesh = new Mesh();
                mesh.name = "Mesh_" + meshDefinitions[i].id;

                this.meshes[i] = mesh;
                //TODO: ignore empty meshes
                Conversions.ConvertMeshDefinition(meshDefinitions[i], mesh, applyScaleOnVertices ? settings.scaleFactor : 1.0f);
                convertesMesh.Add(meshDefinitions[i].id, mesh);
                //this.meshes[i].name = "Mesh_" + (uint)meshList[i].GetInstanceID();//i;
            }
        }

        #endregion

        

        GameObject extractObject(PackedTree sceneTree, int occurrenceIndex)
        {
            //uint occurrence = sceneTree.occurrences.list[occurrenceIndex];
            var gameObject = new GameObject(sceneTree.names[occurrenceIndex]);

            //Transformation
            int matrixIndex = sceneTree.transformIndices[occurrenceIndex];
            
            if(matrixIndex != -1)
            {
                Matrix4x4 matrix = Conversions.ConvertMatrix(sceneTree.transformMatrices[matrixIndex]);
                    //matrix = sceneTree.transformMatrices[matrixIndex].ToUnityObject();
                if(settings.scaleFactor != 1.0f && applyScaleOnVertices)
                {
                    matrix.m03 *= settings.scaleFactor;
                    matrix.m13 *= settings.scaleFactor;
                    matrix.m23 *= settings.scaleFactor;
                }
                gameObject.transform.SetFromLocalMatrix(matrix);
            }

            //Geometry
            bool hasPart = meshDefinitions[occurrenceIndex].id != 0; // meshDefConverters[occurrenceIndex].meshDefinition.id != 0;
            if(hasPart)
            {
                //MeshDefinitionToMesh meshConverter = meshDefConverters[occurrenceIndex];
                Mesh mesh = this.meshes[occurrenceIndex];
                if (this.meshes[occurrenceIndex].vertexCount != 0)
                {
                    MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
                    MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
                    meshFilter.sharedMesh = mesh;
                    polyCount += mesh.GetPolycount();
                    objectCount++;

                    //Assign materials
                    uint matRef = scene.materials.list[occurrenceIndex];

                    Material[] materials;
                    materials = new Material[mesh.subMeshCount];
                    for(int i=0; i<materials.Length;i++)
                    {
                        if(mesh.GetSubMesh(i).topology == MeshTopology.Lines)
                        {
                            //if (i < mesh.subMeshCount - mesh.lines.length)
                            //    return UnityEngine.Color.black;
                            //ImportSDK.Native.Core.ColorAlpha color = meshDefinition.lines[submesh - (subMeshCount - meshDefinition.lines.length)].color;
                            //c = new UnityEngine.Color((float)color.r, (float)color.g, (float)color.b, (float)color.a);
                            materials[i] = getLineMaterial(UnityEngine.Color.black);
                        }
                        else if(mesh.GetSubMesh(i).topology == MeshTopology.Points)
                        {
                            materials[i] = getPointMaterial();
                        }
                        else
                        {
                            if (matRef <= 0) //Not having material on part
                            {
                                if (i > meshDefinitions[occurrenceIndex].dressedPolys.length - 1)
                                {
                                    materials[i] = getLineMaterial(UnityEngine.Color.black);
                                }
                                else
                                {
                                    uint subMeshMatId = meshDefinitions[occurrenceIndex].dressedPolys[i].material;
                                    Material newMaterial = getMaterial(subMeshMatId);
                                    materials[i] = getMaterial(subMeshMatId);

                                }

                            }
                            else //Having material on part
                            {
                                materials[i] = getMaterial(matRef);
                            }
                        }
                    }
                    meshRenderer.sharedMaterials = materials;
                }
            }

            //Metadata 

            Metadata metadata = null;
            if (metadataByOccurrences.list[occurrenceIndex]._base.length > 0)
            {
                PropertyValueList valueList = metadataByOccurrences.list[occurrenceIndex]._base;
                string[] names = new string[metadataByOccurrences.list[occurrenceIndex]._base.length];
                string[] values = new string[metadataByOccurrences.list[occurrenceIndex]._base.length];
                for (int i = 0; i < valueList.length; i++)
                {
                    names[i] = valueList.list[i].name;
                    values[i] = valueList.list[i].value;
                }
                metadata = gameObject.AddComponent<Metadata>();
                metadata.type = "Metadata";

                metadata.setProperties(new Properties(names, values));
            }

            // Custom properties
            if (sceneTree.customProperties[occurrenceIndex].length > 0)
            {
                if (metadata == null)
                {
                    metadata = gameObject.AddComponent<Metadata>();
                    metadata.type = "Metadata";
                }

                for (int i = 0; i < sceneTree.customProperties[occurrenceIndex].length; i++)
                {
                    string name = sceneTree.customProperties[occurrenceIndex][i].key;
                    string value = sceneTree.customProperties[occurrenceIndex][i].value;
                    metadata.addOrSetProperty(name, value);
                }
            }


            _meshDefConverterProcessedCount++;

            return gameObject;
        }

        private int getLODNumber(int occurrence)
        {
            int length = metadataByOccurrences[occurrence]._base.list.Length;
            for (int i = 0; i < length; ++i)
            {
                if (metadataByOccurrences[occurrence]._base.list[i].name == LOD_NUMBER_ATTR)
                {
                    int num = -1;
                    int.TryParse(metadataByOccurrences[occurrence]._base.list[i].value, out num);
                    return num;
                }
            }
            return -1;
        }

        

        private void clear()
        {
            foreach (LODGroup lodGroup in lodGroups)
            {
                lodGroup.RecalculateBounds();
            }
            try
            {
                ImportSDK.Native.NativeInterface.ResetSession();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }
}

