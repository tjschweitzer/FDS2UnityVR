using Pixyz.Commons.Utilities;
using Pixyz.OptimizeSDK.Utils;
using Pixyz.Commons.Extensions;
using Pixyz.Plugin4Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pixyz.OptimizeSDK.Editor.MeshProcessing
{
	public class MeshData
	{
		public List<Renderer> renderers = new List<Renderer>();
		public IList<Mesh> meshes = new List<Mesh>();
		public IList<UnityEngine.Material[]> materials = new List<UnityEngine.Material[]>();

		public MeshData()
		{

		}

		public MeshData(List<Renderer> renderers, IList<Mesh> meshes, IList<UnityEngine.Material[]> materials)
		{
			this.renderers = renderers;
			this.meshes = meshes;
			this.materials = materials;
		}
	}

	/// <summary>
	/// Describe how materials will be used with pixyz/unity
	/// - SyncFull : Materials are translated to pixyz materials and are translated back to unity
	/// - SyncSendOnly : Materials are translated to pixyz but unity materials that were sent will be re-applied
	/// - SyncNone : Input unity materials will be re-applied (if link is found) on the output. (Default material will be created in pixyz)
	/// </summary>
	public enum MaterialSyncType
    {
		SyncFull,
		SyncSendOnly,
		SyncNone
    }

	public class PixyzContext : IDisposable, ICloneable
	{
		public Native.Polygonal.MeshList pixyzMeshes;
		public Native.Geom.Matrix4List pixyzMatrices;

		/// <summary>
		/// Specify if this context should send materials data to pixyz or re-apply unity material
		/// </summary>
		private readonly MaterialSyncType _syncMaterials = MaterialSyncType.SyncFull;

		/// <summary>
		/// Keeps tracks of material already translated to pixyz from unity to avoid duplication
		/// </summary>
		private Dictionary<UnityEngine.Material, uint> _materialUnityToPixyz = null;

		/// <summary>
		/// Keeps tracks of material already translated to unity from pixyz to avoid duplication, can be share between multiple contexts
		/// </summary>
		private Dictionary<uint, UnityEngine.Material> _materialPixyzToUnity = null;

		/// <summary>
		/// Keeps track of material when material sync is off, to re-apply material when retrieving objects from pixyz
		/// </summary>
		private Dictionary<uint, Material[]> _unityMaterialsLookUp = null;

		/// <summary>
		/// Default line material to render lines
		/// </summary>
		private Material _lineMaterial = null;

		/// <summary>
		/// Default line material to render lines
		/// </summary>
		private Material LineMaterial
		{
			get
			{
				if (_lineMaterial)
				{
					_lineMaterial = new Material(ShaderUtilities.GetPixyzLineShader());
				}
				return _lineMaterial;
			}
		}

		/// <summary>
		/// Default Point material to render point cloud
		/// </summary>
		private Material _pointMaterial = null;

		/// <summary>
		/// Default Point material to render point cloud
		/// </summary>
		private Material PointMaterial
		{
			get
			{
				if (_pointMaterial)
				{
					_pointMaterial = new Material(ShaderUtilities.GetPixyzSplatsShader());
				}
				return _pointMaterial;
			}
		}

		/// <summary>
		/// Default material used when a material was not found
		/// </summary>
		private Material _missingMaterial = null;

		/// <summary>
		/// Default material used when a material was not found
		/// </summary>
		private Material MissingMaterial
		{
            get
			{
				if(_missingMaterial)
                {
					_missingMaterial = new Material(ShaderUtilities.GetDefaultShader());
					_missingMaterial.SetColor("_Color", Color.red);
					_missingMaterial.SetColor("_BaseColor", Color.red);
				}
				return _missingMaterial;
			}
		}

		/// <summary>
		/// Keeps tracks of textures already translated to pixyz from unity to avoid duplication
		/// </summary>
		private Dictionary<Texture2D, uint> _imagesUnityToPixyz = null;// new Dictionary<Texture2D, uint>();

		/// <summary>
		/// Keeps tracks of textures already translated to unity from pixyz to avoid duplication, can be share between multiple contexts
		/// </summary>
		private Dictionary<uint, Texture2D> _imagesPixyzToUnity = null;// new Dictionary<uint, Texture2D>();

		/// <summary>
		/// Keeps tracks of bones already translated to unity from pixyz to avoid duplication, can be share between multiple contexts
		/// </summary>
		private Dictionary<Transform, ulong> _bonesUnityToPixyz = new Dictionary<Transform, ulong>();

		/// <summary>
		/// Keep track of root bones to re-apply them as roots when translating from pixyz
		/// </summary>
		private Dictionary<uint, ulong> _rootBonesUnityToPixyz = new Dictionary<uint, ulong>();

		/// <summary>
		/// All uniques bones from unity
		/// </summary>
		private List<Transform> _unityBones = new List<Transform>();
		/// <summary>
		/// All uniques bones from pixyz
		/// </summary>
		private List<uint> _pixyzBones = new List<uint>();

		private Dictionary<uint, string> _meshDefinitionNames = new Dictionary<uint, string>();

		/// <summary>
		/// Keeps tracks of renderers already translated to unity from pixyz to avoid duplication, can be share between multiple contexts
		/// </summary>
		private Dictionary<uint, Renderer> _meshPixyzToUnity = new Dictionary<uint, Renderer>();

		/// <summary>
		/// Keeps tracks of meshes generated from pixyz by this context only
		/// </summary>
		private List<Mesh> _generatedMeshes = new List<Mesh>();

		/// <summary>
		/// List of all materials translated from pixyz to unity (usefull if they need to be saved outside the scene)
		/// </summary>
		public IEnumerable<UnityEngine.Material> GeneratedMaterials => _materialPixyzToUnity.Values.AsEnumerable();

		/// <summary>
		/// List of all textures translated from pixyz to unity (usefull if they need to be saved outside the scene)
		/// </summary>
		public IEnumerable<Texture2D> GeneratedTextures => _imagesPixyzToUnity.Values.AsEnumerable();

		/// <summary>
		/// List of all meshes translated from pixyz to unity (usefull if they need to be saved outside the scene)
		/// </summary>
		public IEnumerable<Mesh> GeneratedMesh => _generatedMeshes;

		public PixyzContext(MaterialSyncType syncMaterials = MaterialSyncType.SyncFull)
        {
			_syncMaterials = syncMaterials;
			switch(_syncMaterials)
            {
				case MaterialSyncType.SyncFull:
					{
						_imagesPixyzToUnity = new Dictionary<uint, Texture2D>();
						_imagesUnityToPixyz = new Dictionary<Texture2D, uint>();
						_materialPixyzToUnity = new Dictionary<uint, Material>();
						_materialUnityToPixyz = new Dictionary<Material, uint>();
					}
					break;
				case MaterialSyncType.SyncNone:
				case MaterialSyncType.SyncSendOnly:
					{
						_imagesUnityToPixyz = new Dictionary<Texture2D, uint>();
						_materialUnityToPixyz = new Dictionary<Material, uint>();
						_unityMaterialsLookUp = new Dictionary<uint, Material[]>();
					}
					break;
            }
        }

		// ------- Extract from Unity--------- //
		#region Unity => Pixyz

		/// <summary>
		/// Create meshes from input into pixyz. (pixyzMeshes & pixyzMatrices are auto filled)
		/// Handle instances
		/// </summary>
		/// <param name="renderers"></param>
		/// <param name="meshes"></param>
		/// <param name="materials"></param>
		/// <returns></returns>
		public uint[] UnityMeshesToPixyzCreate(List<Renderer> renderers, IList<Mesh> meshes, IList<UnityEngine.Material[]> materials)
		{
			return UnityMeshesToPixyzCreate(new MeshData(renderers, meshes, materials));
		}

		/// <summary>
		/// Create meshes from input into pixyz. (pixyzMeshes & pixyzMatrices are auto filled)
		/// Handle instances
		/// </summary>
		/// <param name="meshData"></param>
		/// <returns></returns>
		public uint[] UnityMeshesToPixyzCreate(in MeshData meshData)
		{
			List<Tuple<Renderer, int>> outRenderers = null;
			MeshData filteredData = FilteredRenderers(meshData, out outRenderers);

			int count = filteredData.renderers.Count;
			Native.Polygonal.MeshDefinition[] defs = new Native.Polygonal.MeshDefinition[count];
			pixyzMatrices = new Native.Geom.Matrix4List(count);

			for (int i = 0; i < filteredData.renderers.Count; ++i)
			{
				defs[i] = UnityMeshToPixyz(filteredData.renderers[i], filteredData.meshes[i], filteredData.materials[i]);
			}

			pixyzMeshes = new Native.Polygonal.MeshList(Native.NativeInterface.CreateMeshes(new Native.Polygonal.MeshDefinitionList(defs)));

			Native.Polygonal.MeshList meshList = new Native.Polygonal.MeshList(outRenderers.Count);
			Native.Geom.Matrix4List matrixList = new Native.Geom.Matrix4List(outRenderers.Count);

			for (int i = 0; i < outRenderers.Count; ++i)
			{
				Tuple<Renderer, int> pair = outRenderers[i];
				meshList[i] = pixyzMeshes[pair.Item2];
				matrixList[i] = Conversions.ConvertMatrix(pair.Item1.transform.localToWorldMatrix);
				
				if(_syncMaterials != MaterialSyncType.SyncFull)
					_unityMaterialsLookUp[pixyzMeshes[pair.Item2]] = pair.Item1.sharedMaterials;
			}

			pixyzMeshes = meshList;
			pixyzMatrices = matrixList;

			return pixyzMeshes.list;
		}
		/// <summary>
		/// Create a mesh from input into pixyz. (pixyzMeshes & pixyzMatrices are auto filled)
		/// </summary>
		/// <param name="renderers"></param>
		/// <param name="meshes"></param>
		/// <param name="materials"></param>
		/// <returns></returns>
		public uint UnityMeshToPixyzCreate(Renderer renderer, Mesh mesh, UnityEngine.Material[] materials)
		{
			Native.Polygonal.MeshDefinition def = UnityMeshToPixyz(renderer, mesh, materials);
            def.id = Native.NativeInterface.CreateMesh(def);

			pixyzMatrices = new Native.Geom.Matrix4List(new Native.Geom.Matrix4[] { Conversions.ConvertMatrix(renderer.transform.localToWorldMatrix) });
			pixyzMeshes = new Native.Polygonal.MeshList(new uint[] { def.id });

			if (_syncMaterials != MaterialSyncType.SyncFull)
				_unityMaterialsLookUp.Add(def.id, renderer.sharedMaterials);

			return def.id;
		}

		/// <summary>
		/// Create a mesh from input into pixyz without clearing pixyzMeshes & pixyzMatrices
		/// </summary>
		/// <param name="renderers"></param>
		/// <param name="meshes"></param>
		/// <param name="materials"></param>
		/// <returns></returns>
		public uint AddUnityMeshToPixyzCreate(Renderer renderer, Mesh mesh, UnityEngine.Material[] materials)
		{
			Native.Polygonal.MeshDefinition def = UnityMeshToPixyz(renderer, mesh, materials);
            def.id = Native.NativeInterface.CreateMesh(def);

			Native.Geom.Matrix4List matriceList = new Native.Geom.Matrix4List(pixyzMatrices.length + 1);
			Array.Copy(pixyzMatrices, matriceList, pixyzMatrices.length);
			matriceList[pixyzMatrices.length] = Conversions.ConvertMatrix(renderer.transform.localToWorldMatrix);

			Native.Polygonal.MeshList meshList = new Native.Polygonal.MeshList(pixyzMeshes.length + 1);
			Array.Copy(pixyzMatrices, matriceList, pixyzMatrices.length);
			meshList[pixyzMatrices.length] = def.id;

			if (_syncMaterials != MaterialSyncType.SyncFull)
				_unityMaterialsLookUp.Add(def.id, renderer.sharedMaterials);

			pixyzMatrices = matriceList;
			pixyzMeshes = meshList;

			return def.id;
		}

		/// <summary>
		/// Create meshes definition from input without creating into pixyz. (pixyzMeshes & pixyzMatrices are auto filled)
		/// Does not handle instancing.
		/// </summary>
		/// <param name="renderers"></param>
		/// <param name="meshes"></param>
		/// <param name="materials"></param>
		/// <returns></returns>
		public Native.Polygonal.MeshDefinition[] UnityMeshesToPixyz(IList<Renderer> renderers, IList<Mesh> meshes, IList<UnityEngine.Material[]> materials)
		{
			if (_syncMaterials != MaterialSyncType.SyncFull)
				Debug.LogWarning("[PixyzContext] This method only support MaterialSyncType.SyncFull");

			Native.Polygonal.MeshDefinition[] defs = new Native.Polygonal.MeshDefinition[renderers.Count];

			for (int i = 0; i < defs.Length; ++i)
			{
				defs[i] = UnityMeshToPixyz(renderers[i], meshes[i], materials[i]);
			}

			pixyzMatrices = Conversions.ConvertMatrices(renderers.Select(r => r.transform.localToWorldMatrix).ToList());

			return defs;
		}

		/// <summary>
		/// Create a mesh definition from input without creating into pixyz. (pixyzMeshes & pixyzMatrices are auto filled)
		/// </summary>
		/// <param name="renderers"></param>
		/// <param name="meshes"></param>
		/// <param name="materials"></param>
		/// <returns></returns>
		public Native.Polygonal.MeshDefinition UnityMeshToPixyz(Renderer renderer, Mesh mesh, UnityEngine.Material[] materials)
		{
			if(!mesh.isReadable)
			{
				throw new Exception("[PixyzContext] The mesh is not set as readable, please modify the import settings of the source Mesh and set Read/Write Enabled to true");
			}

			foreach(Material material in materials)
            {
				if (material == null)
					throw new Exception($"[PixyzContext] Missing material on renderer : {renderer.name}");
			}

			uint[] materialIds = GenerateMaterials(materials);
			Native.Polygonal.MeshDefinition def = Conversions.ConvertMesh(mesh, materialIds);

			SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
			def.joints = GenerateBones(skinnedMeshRenderer);

			return def;
		}

		/// <summary>
		/// Create a mesh definition from input without creating into pixyz and without clearing pixyzMeshes & pixyzMatrices
		/// </summary>
		/// <param name="renderers"></param>
		/// <param name="meshes"></param>
		/// <param name="materials"></param>
		/// <returns></returns>
		public Native.Polygonal.MeshDefinition AddUnityMeshToPixyz(Renderer renderer, Mesh mesh, UnityEngine.Material[] materials)
		{
			if (_syncMaterials != MaterialSyncType.SyncFull)
				Debug.LogWarning("[PixyzContext] This method only support MaterialSyncType.SyncFull");

			Native.Polygonal.MeshDefinition def = UnityMeshToPixyz(renderer, mesh, materials);
			Native.NativeInterface.CreateMesh(def);

			Native.Geom.Matrix4List matriceList = new Native.Geom.Matrix4List(pixyzMatrices.length + 1);
			Array.Copy(pixyzMatrices, matriceList, pixyzMatrices.length);
			matriceList[pixyzMatrices.length] = Conversions.ConvertMatrix(renderer.transform.localToWorldMatrix);

			pixyzMatrices = matriceList;

			return def;
		}

		private Native.Material.MaterialList GenerateMaterials(UnityEngine.Material[] materials)
		{
			List<Native.Material.MaterialDefinition> defs = new List<Native.Material.MaterialDefinition>();
			Native.Material.MaterialList materialsPixyz = new Native.Material.MaterialList(materials.Length);
			Dictionary<Texture2D, uint> textures = new Dictionary<Texture2D, uint>();
			Dictionary<Texture2D, string> texturesTypes = new Dictionary<Texture2D, string>();

			for (int i = 0; i < materials.Length; ++i)
			{
				UnityEngine.Material material = materials[i];

				if (_materialUnityToPixyz.ContainsKey(material))
				{
					materialsPixyz[i] = _materialUnityToPixyz[material];
					continue;
				}
				else
				{
					Native.Material.MaterialDefinition def = null;

					if(_syncMaterials == MaterialSyncType.SyncNone)
                    {
						def = Conversions.CreateStandardPixyzMaterial(material.name, Conversions.IntToColor((uint)Mathf.Abs(material.GetInstanceID())));
                    }
                    else
					{
						int propertyCount = ShaderUtil.GetPropertyCount(material.shader);
						Color albedoColor = Color.white;

						for (int j = 0; j < propertyCount; ++j)
						{
							ShaderUtil.ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(material.shader, j);
							string propertyName = ShaderUtil.GetPropertyName(material.shader, j);

							if (!IsValidProperty(propertyName))
								continue;

							if (propertyType == ShaderUtil.ShaderPropertyType.TexEnv)
							{
								Texture2D tex = material.GetTexture(propertyName) as Texture2D;

								if (tex != null && !textures.ContainsKey(tex))
								{
									texturesTypes.Add(tex, propertyName);
									textures.Add(tex, 0);
								}
							}
							else if (propertyType == ShaderUtil.ShaderPropertyType.Color && propertyName == "_Color")
							{
								albedoColor = material.GetColor("_Color");
							}
						}
						GenerateTextures(textures, texturesTypes, albedoColor);
						def = Conversions.ConvertMaterial(material, ref _imagesUnityToPixyz);
					}

					defs.Add(def);
					_materialUnityToPixyz.Add(material, 0);
				}
			}

			Native.Material.MaterialList list = Native.NativeInterface.CreateMaterials(new Native.Material.MaterialDefinitionList(defs.ToArray()));

			int generatedCount = 0;

			for (int i =0; i < materials.Length; ++i)
			{
				if(materialsPixyz[i] == 0)
				{
					UnityEngine.Material mat = materials[i];
					if(_materialUnityToPixyz[mat] == 0)
					{
						materialsPixyz[i] = list[generatedCount];
						_materialUnityToPixyz[mat] = list[generatedCount];
						++generatedCount;
					}
					else
					{
						materialsPixyz[i] = _materialUnityToPixyz[mat];
					}
				}
			}

			return materialsPixyz;
		}

		private void GenerateTextures(Dictionary<Texture2D, uint> textures, Dictionary<Texture2D, string> texturesTypes, Color colorCoeff)
		{
			List<Native.Material.ImageDefinition> defs = new List<Native.Material.ImageDefinition>();
			List<Texture2D> addedTextures = new List<Texture2D>();

			for(int i =0; i < textures.Keys.Count; ++i)
			{
				Texture2D tex = textures.Keys.ToList()[i];

				if (_imagesUnityToPixyz.ContainsKey(tex))
				{
					textures[tex] = _imagesUnityToPixyz[tex];
				}
				else
				{
					Native.Material.ImageDefinition def = Conversions.ConvertTexture(tex, texturesTypes[tex] == "_BumpMap");
					defs.Add(def);

					if(texturesTypes[tex] == "_MainTex" && colorCoeff != Color.white)
					{
						ApplyCoeff(def, colorCoeff);
					}
					addedTextures.Add(tex);
				}
			}

			Native.Material.ImageList list = Native.NativeInterface.CreateImages(new Native.Material.ImageDefinitionList(defs.ToArray()));

			for(int i = 0; i < list.list.Length; ++i)
			{
				Texture2D tex = addedTextures[i];
				textures[tex] = list[i];
				_imagesUnityToPixyz[tex] = list[i];
			}
		}

		private void ApplyCoeff(Native.Material.ImageDefinition image, Color coef)
		{
			byte[] srcColors = image.data.list;
			int srcComponentCount = image.componentsCount;
			if (coef.a != 1.0f && image.componentsCount != 4)
			{
				image.componentsCount = 4;
				image.data = new Native.Core.ByteList(4 * image.height * image.width);
			}

			for (int i = 0; i < srcColors.Length; i += srcComponentCount)
			{
				int colorCount = i / image.componentsCount;
				Color srcColor = new Color();
				srcColor.r = ((float)srcColors[i]) / 255.0f;

				if(image.componentsCount > 1)
					srcColor.g = ((float)srcColors[i + 1]) / 255.0f;
				if (image.componentsCount > 2)
					srcColor.b = ((float)srcColors[i + 2]) / 255.0f;
				if (image.componentsCount > 3)
					srcColor.a = ((float)srcColors[i + 3]) / 255.0f;

				srcColor *= coef;

				for (int j = 0; j < image.componentsCount; ++j)
				{
					switch(j)
					{
						case 0:
							image.data[colorCount * image.componentsCount + j] = (byte)(srcColor.r * 255.0f);
							break;
						case 1:
							image.data[colorCount * image.componentsCount + j] = (byte)(srcColor.g * 255.0f);
							break;
						case 2:
							image.data[colorCount * image.componentsCount + j] = (byte)(srcColor.b * 255.0f);
							break;
						case 3:
							image.data[colorCount * image.componentsCount + j] = (byte)(srcColor.a * 255.0f);
							break;
						default:
							throw new Exception("[PixyzContext] Color with more than 4 channels are not supported !");
					}
				}
			}
		}


		private MeshData FilteredRenderers(in MeshData inData, out List<Tuple<Renderer, int>> outRenderers)
		{
			Dictionary<long, int> lookupTable = new Dictionary<long, int>();
			HashSet<Renderer> uniqueRenderers = new HashSet<Renderer>();
			outRenderers = new List<Tuple<Renderer, int>>();
			MeshData data = new MeshData();

			for (int i = 0; i < inData.renderers.Count; ++i)
			{
				Renderer r = inData.renderers[i];

				if (uniqueRenderers.Contains(r))
					continue;

				long hash = GenerateRendererHash(r);
				uniqueRenderers.Add(r);

				if (lookupTable.ContainsKey(hash))
				{
					outRenderers.Add(new Tuple<Renderer, int>(r, lookupTable[hash]));
				}
				else
				{
					int index = data.meshes.Count;
					lookupTable.Add(hash, index);

					data.renderers.Add(r);
					data.meshes.Add(inData.meshes[i]);

					data.materials.Add(inData.materials[i]);
					outRenderers.Add(new Tuple<Renderer, int>(r, index));
				}
			}

			return data;
		}

		public static long GenerateRendererHash(Renderer r)
		{
			long hash = 1;

			if (r == null)
				return hash;

			Mesh mesh = null;

			if (r is MeshRenderer)
				mesh = r.GetComponent<MeshFilter>().sharedMesh;
			else if (r is SkinnedMeshRenderer)
				mesh = ((SkinnedMeshRenderer)r).sharedMesh;

			if (mesh == null)
				return hash;

			hash *= 0x1A02F035E56B3A72;
			//hash ^= mesh.normals.Length;
			//hash ^= mesh.uv.Length;
			//hash ^= mesh.uv2.Length;
			//hash ^= mesh.vertexCount;

			hash ^= (int)mesh.bounds.size.sqrMagnitude;

			if (r is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer skinnedRenderer = r as SkinnedMeshRenderer;

				foreach (Transform t in skinnedRenderer.bones)
				{
					hash *= 0x1B02B0AAE5DD3A00;
					hash += t.GetInstanceID();
				}
			}

			foreach (var material in r.sharedMaterials)
			{
				if (material == null)
					throw new Exception($"[PixyzContext] Missing material on renderer : {r.name}");

				hash *= 0x1EF31D238836D08C;
				hash += material.ComputeCRC();
			}

			hash ^= mesh.GetInstanceID();

			//hash ^= r.transform.localToWorldMatrix.GetHashCode();

			return hash;
		}

		private bool IsValidProperty(string propertyName)
		{
			switch (propertyName)
			{
				case "_Cutoff":
				case "_MainTex":
				case "_BumpMap":
				case "_Color":
				case "_MetallicGlossMap":
				case "_Metallic":
				case "_Glossiness":
				case "_OcclusionMap":
					return true;

				default: return false;
			}
		}

		private Native.Polygonal.JointList GenerateBones(SkinnedMeshRenderer renderer)
		{
			if(renderer == null)
			{
				return new Native.Polygonal.JointList();
			}

			Native.Polygonal.JointList joints = new Native.Polygonal.JointList(renderer.bones.Length);
			List<Transform> transformsAdd = new List<Transform>();
			List<int> transformsIndices = new List<int>();
			List<ulong> transformAddIds = new List<ulong>();
			List<Matrix4x4> transformAddMatrices = new List<Matrix4x4>();

			int count = _unityBones.Count();

			for(int i = 0; i < renderer.bones.Length; ++i)
			{
				Transform bone = renderer.bones[i];
				transformsAdd.Add(bone);

				if (_bonesUnityToPixyz.ContainsKey(bone))
				{
					int index = (int)_bonesUnityToPixyz[bone];
					if (_pixyzBones.Count > index)
						joints[i] = _pixyzBones[index];
				}
				else
				{
					transformsIndices.Add(i);
					transformAddIds.Add((ulong)count);
					transformAddMatrices.Add(bone.localToWorldMatrix);

					_bonesUnityToPixyz.Add(bone, 0);
					joints[i] = 0;

					++count;
				}
			}

			Native.Polygonal.PlaceholderJointList list = Native.NativeInterface.CreateJoints(new Native.Core.ULongList(transformAddIds.ToArray()), Conversions.ConvertMatrices(transformAddMatrices));

			int generatedCount = 0;
			for (int i = 0; i < transformsAdd.Count; ++i)
			{
				Transform transform = transformsAdd[i];
				if (joints[i] == 0)
				{
					if(_bonesUnityToPixyz[transform] == 0)
					{
						uint jointId = list.list[generatedCount];
						int index = transformsIndices[generatedCount];

						joints[index] = jointId;
						_unityBones.Add(transform);
						_bonesUnityToPixyz[transform] = transformAddIds[generatedCount];
						_pixyzBones.Add(jointId);
						++generatedCount;

						if (transform == renderer.rootBone)
							_rootBonesUnityToPixyz.Add(_pixyzBones[(int)_bonesUnityToPixyz[transform]], _bonesUnityToPixyz[transform]);
					}
					else
					{
						joints[i] = _pixyzBones[(int)_bonesUnityToPixyz[transform]];
					}
				}
			}

			return joints;
		}

		#endregion

		// ------- Extract from Pixyz--------- //
		#region Pixyz => Unity

		/// <summary>
		/// Retrieve a list of gameobject from pixyz meshs, transform are not applied.
		/// </summary>
		/// <param name="meshIds"></param>
		/// <returns></returns>
		public GameObject[] PixyzMeshToUnityObject(uint[] meshIds)
		{
            Native.Polygonal.MeshDefinition[] meshDefinitions = Native.NativeInterface.GetMeshes(new Native.Polygonal.MeshList(meshIds));
            GameObject[] gameObjects = new GameObject[meshDefinitions.Length];
			//TODO : add custom property
			//ExtractMeshDefinitionNames(meshIds);

			for (int i = 0; i < meshDefinitions.Length; i++)
                gameObjects[i] = PixyzMeshToUnityObject(meshDefinitions[i]);

            return gameObjects;
        }

		/// <summary>
		/// Retrieve a gameobject from a pixyz mesh, transform is not applied.
		/// </summary>
		/// <param name="meshIds"></param>
		/// <returns></returns>
		public GameObject PixyzMeshToUnityObject(uint meshId)
		{
			Native.Polygonal.MeshDefinition def = Native.NativeInterface.GetMesh(meshId);
			//TODO : add custom property
			//ExtractMeshDefinitionNames(new uint[]{ meshId});

			return PixyzMeshToUnityObject(def);
		}

		/// <summary>
		/// Retrieve a list of gameobject from pixyz meshs definitions, transform are not applied.
		/// </summary>
		/// <param name="meshIds"></param>
		/// <returns></returns>
		public GameObject[] PixyzMeshToUnityObject(IEnumerable<Native.Polygonal.MeshDefinition> sourceDefs)
		{
			uint[] meshIds = sourceDefs.Select(d => d.id).ToArray();

			Native.Polygonal.MeshDefinition[] meshDefinitions = Native.NativeInterface.GetMeshes(new Native.Polygonal.MeshList(meshIds));
            GameObject[] gameObjects = new GameObject[meshDefinitions.Length];
			//TODO : add custom property
			//ExtractMeshDefinitionNames(meshIds);

			for (int i = 0; i < meshDefinitions.Length; i++)
                gameObjects[i] = PixyzMeshToUnityObject(meshDefinitions[i]);

            return gameObjects;
        }
		
		/// <summary>
		/// Retrieve a list of renderers from pixyz meshs, transform are not applied.
		/// </summary>
		/// <param name="meshIds"></param>
		/// <returns></returns>
		public Renderer[] PixyzMeshToUnityRenderer(uint[] meshIds)
		{
			Native.Polygonal.MeshDefinition[] meshDefinitions = Native.NativeInterface.GetMeshes(new Native.Polygonal.MeshList(meshIds));
            Renderer[] renderers = new Renderer[meshIds.Length];
			//TODO : add custom property
			//ExtractMeshDefinitionNames(meshIds);

			for (int i = 0; i < meshDefinitions.Length; i++)
				renderers[i] = PixyzMeshToUnityRenderer(meshDefinitions[i]);

			return renderers;
		}

		/// <summary>
		/// Retrieve a gameobject from a pixyz mesh definition, transform is not applied.
		/// </summary>
		/// <param name="meshIds"></param>
		/// <returns></returns>
		public GameObject PixyzMeshToUnityObject(Native.Polygonal.MeshDefinition def)
		{
			return PixyzMeshToUnityRenderer(def).gameObject;
		}

		/// <summary>
		/// Retrieve a renderers from pixyz meshs, transform is not applied.
		/// </summary>
		/// <param name="meshIds"></param>
		/// <returns></returns>
		public Renderer PixyzMeshToUnityRenderer(Native.Polygonal.MeshDefinition def)
		{
			if (_meshPixyzToUnity.ContainsKey(def.id))
			{
				return GameObject.Instantiate<Renderer>(_meshPixyzToUnity[def.id]);
			}

			GameObject newGameObject = new GameObject();

			Mesh mesh = ExtractMesh(def);
			Renderer renderer = null;
			UnityEngine.Material[] materials = null;

			if (_syncMaterials != MaterialSyncType.SyncFull)
				materials = RetrieveMaterialsFromUnity(def);
			else
				materials = RetrieveMaterialsFromPixyz(def, mesh);

			if (def.joints.length > 0)
			{
				SkinnedMeshRenderer skinnedMesh = newGameObject.AddComponent<SkinnedMeshRenderer>();
				skinnedMesh.sharedMaterials = materials;
				skinnedMesh.sharedMesh = mesh;
				renderer = skinnedMesh;

				ExtractBones(skinnedMesh, def.joints);
			}
			else
			{
				MeshRenderer meshRenderer = newGameObject.AddComponent<MeshRenderer>();
				MeshFilter filter = newGameObject.AddComponent<MeshFilter>();

				meshRenderer.sharedMaterials = materials;
				filter.sharedMesh = mesh;
				renderer = meshRenderer;
			}

			mesh.MarkModified();
			mesh.UploadMeshData(false);

			_meshPixyzToUnity.Add(def.id, renderer);

			return renderer;
		}

		private Material[] RetrieveMaterialsFromUnity(Native.Polygonal.MeshDefinition meshDefinition)
        {
			UnityEngine.Material[] materials = new Material[meshDefinition.dressedPolys.length + meshDefinition.lines.length + (meshDefinition.points.length > 0 ? 1 : 0)];
			UnityEngine.Material[] oldMaterials = null;

			if (_unityMaterialsLookUp.ContainsKey(meshDefinition.id))
				oldMaterials = _unityMaterialsLookUp[meshDefinition.id];

			int usedDressedPoly = 0;
			int usedStylizedLines = 0;

			for(int i = 0; i < materials.Length; ++i)
			{
				if (usedDressedPoly < meshDefinition.dressedPolys.length && usedStylizedLines < meshDefinition.lines.length)
				{
					if (meshDefinition.lines[usedStylizedLines].externalId < meshDefinition.dressedPolys[usedDressedPoly].externalId)
					{
						if(oldMaterials != null && meshDefinition.lines[usedStylizedLines].externalId != 0)
                        {
							materials[i] = oldMaterials[meshDefinition.lines[usedStylizedLines].externalId - 1];
                        }
                        else
                        {
							materials[i] = MissingMaterial;
						}
						++usedStylizedLines;
					}
					else
					{
						if (oldMaterials != null && meshDefinition.dressedPolys[usedDressedPoly].externalId != 0)
						{
							materials[i] = oldMaterials[meshDefinition.dressedPolys[usedDressedPoly].externalId - 1];
						}
						else
						{
							materials[i] = MissingMaterial;
						}
						++usedDressedPoly;
					}
				}
				else if (usedStylizedLines < meshDefinition.lines.length)
				{
					if (oldMaterials != null && meshDefinition.lines[usedStylizedLines].externalId != 0)
					{
						materials[i] = oldMaterials[meshDefinition.lines[usedStylizedLines].externalId - 1];
					}
					else
					{
						materials[i] = MissingMaterial;
					}
					++usedStylizedLines;
				}
				else if (usedDressedPoly < meshDefinition.dressedPolys.length)
				{
					if (oldMaterials != null && meshDefinition.dressedPolys[usedDressedPoly].externalId != 0)
					{
						materials[i] = oldMaterials[meshDefinition.dressedPolys[usedDressedPoly].externalId - 1];
					}
					else
					{
						materials[i] = MissingMaterial;
					}
					++usedDressedPoly;
				}
				else
				{
					materials[i] = PointMaterial;
				}
			}

			return materials;
		}
		private Material[] RetrieveMaterialsFromPixyz(Native.Polygonal.MeshDefinition definition, Mesh mesh)
		{
			UnityEngine.Material[] materials = ExtractMaterials(definition.dressedPolys);

			if (definition.lines.length > 0 || definition.points.length > 0)
			{
				UnityEngine.Material[] tmp = new Material[mesh.subMeshCount];
				uint assignedDressedPolyMaterial = 0;

				for (int i = 0; i < mesh.subMeshCount; ++i)
				{
					switch (mesh.GetTopology(i))
					{
						case MeshTopology.Lines:
						case MeshTopology.LineStrip:
							{
								tmp[i] = LineMaterial;
							}
							break;
						case MeshTopology.Points:
							{
								tmp[i] = PointMaterial;
							}
							break;
						case MeshTopology.Quads:
						case MeshTopology.Triangles:
							{
								tmp[i] = materials[assignedDressedPolyMaterial];
								++assignedDressedPolyMaterial;
							}
							break;
					}

				}
				materials = tmp;
			}

			return materials;
		}

		private Mesh ExtractMesh(Native.Polygonal.MeshDefinition definition)
		{
			Mesh mesh = new Mesh();

			if (_meshDefinitionNames.ContainsKey(definition.id))
				mesh.name = _meshDefinitionNames[definition.id];
			else
				mesh.name = "Pixyz_Mesh";

			Conversions.ConvertMeshDefinition(definition, mesh);
			_generatedMeshes.Add(mesh);

			return mesh;
		}

		private UnityEngine.Material[] ExtractMaterials(Native.Polygonal.DressedPolyList subMesh)
		{
			UnityEngine.Material[] unityMaterials = new UnityEngine.Material[subMesh.list.Length];
			List<uint> materialsId = new List<uint>();

			for(int i = 0; i < subMesh.list.Length; ++i)
			{
				uint materialId = subMesh[i].material;
				
				if (_materialPixyzToUnity.ContainsKey(materialId))
				{
					unityMaterials[i] = _materialPixyzToUnity[materialId];
				}
				else
				{
					materialsId.Add(materialId);
					_materialPixyzToUnity.Add(materialId, null);
				}
			}

			Native.Material.MaterialDefinitionList materialDefs = Native.NativeInterface.GetMaterials(new Native.Material.MaterialList(materialsId.ToArray()));

			int generatedCount = 0;

			for(int i = 0; i < unityMaterials.Length; ++i)
			{
				if(unityMaterials[i] == null)
				{
					uint materialId = subMesh[i].material;
					if (_materialPixyzToUnity[materialId] == null)
					{
						UnityEngine.Material mat = Conversions.ConvertMaterialExtract(materialDefs[generatedCount], ref _imagesPixyzToUnity);
						_materialPixyzToUnity[materialId] = mat;

						unityMaterials[i] = mat;

						++generatedCount;
					}
					else
					{
						unityMaterials[i] = _materialPixyzToUnity[materialId];
					}
				}
			}

			return unityMaterials;
		}

		private void ExtractBones(SkinnedMeshRenderer renderer, Native.Polygonal.JointList joints)
		{
			Transform[] bones = new Transform[joints.length];

			Native.Core.ULongList jointsList = Native.NativeInterface.GetJoints(new Native.Polygonal.PlaceholderJointList(joints.list));

			for(int i = 0; i < joints.length; ++i)
			{
				bones[i] = _unityBones[(int)jointsList[i]];

				if(_rootBonesUnityToPixyz.ContainsKey(joints[i]))
					renderer.rootBone = _unityBones[(int)_rootBonesUnityToPixyz[joints[i]]];
			}

			renderer.bones = bones;
		}

		private void ExtractMeshDefinitionNames(uint[] meshIds)
        {
			uint[] meshes = meshIds.Distinct().ToArray();
			Native.Core.StringList meshNames = Native.NativeInterface.GetProperties(new Native.Core.EntityList(meshes), "meshName", "");
			for(int i = 0; i < meshes.Length; ++i)
            {
				if (meshNames[i] != "")
					_meshDefinitionNames.Add(meshes[i], meshNames[i]);
            }
		}

		#endregion

		#region Utilities
		/// <summary>
		/// Dispose this context and all contexts, pixyz has currently only one way to remove all unused assets. All ids will be invalid
		/// </summary>
		public void Dispose()
		{
			Native.NativeInterface.Clear();
		}

		/// <summary>
		/// Clone this context unity side but also pixyz side by clone meshes.
		/// /!\ pixyzMeshes & pixyzMatrices will be overriden
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			PixyzContext newContext = new PixyzContext();
			newContext.pixyzMeshes = new Native.Polygonal.MeshList(pixyzMeshes.length);
			newContext.pixyzMatrices = new Native.Geom.Matrix4List(pixyzMatrices.length);

			Dictionary<uint, uint> lookUpNewId = new Dictionary<uint, uint>();

			for (int i = 0; i < pixyzMeshes.length; ++i)
			{
				if(lookUpNewId.ContainsKey(pixyzMeshes.list[i]))
				{
					newContext.pixyzMeshes[i] = lookUpNewId[pixyzMeshes.list[i]];
				}
				else
				{
					newContext.pixyzMeshes[i] = Native.NativeInterface.CloneEntity(pixyzMeshes.list[i]);
					lookUpNewId.Add(pixyzMeshes.list[i], newContext.pixyzMeshes[i]);
				}

				newContext.pixyzMatrices[i] = Conversions.Identity();

				newContext.pixyzMatrices[i][0][0] = pixyzMatrices[i][0][0];
				newContext.pixyzMatrices[i][0][1] = pixyzMatrices[i][0][1];
				newContext.pixyzMatrices[i][0][2] = pixyzMatrices[i][0][2];
				newContext.pixyzMatrices[i][0][3] = pixyzMatrices[i][0][3];

				newContext.pixyzMatrices[i][1][0] = pixyzMatrices[i][1][0];
				newContext.pixyzMatrices[i][1][1] = pixyzMatrices[i][1][1];
				newContext.pixyzMatrices[i][1][2] = pixyzMatrices[i][1][2];
				newContext.pixyzMatrices[i][1][3] = pixyzMatrices[i][1][3];

				newContext.pixyzMatrices[i][2][0] = pixyzMatrices[i][2][0];
				newContext.pixyzMatrices[i][2][1] = pixyzMatrices[i][2][1];
				newContext.pixyzMatrices[i][2][2] = pixyzMatrices[i][2][2];
				newContext.pixyzMatrices[i][2][3] = pixyzMatrices[i][2][3];

				newContext.pixyzMatrices[i][3][0] = pixyzMatrices[i][3][0];
				newContext.pixyzMatrices[i][3][1] = pixyzMatrices[i][3][1];
				newContext.pixyzMatrices[i][3][2] = pixyzMatrices[i][3][2];
				newContext.pixyzMatrices[i][3][3] = pixyzMatrices[i][3][3];
			}
			
			newContext._bonesUnityToPixyz = _bonesUnityToPixyz;
			newContext._rootBonesUnityToPixyz = _rootBonesUnityToPixyz;
			newContext._pixyzBones = _pixyzBones;
			newContext._unityBones = _unityBones;

			return newContext;
		}

		/// <summary>
		/// Allow this context to re-use already translated assets from pixyz to unity (used for shared materials, meshes, texture)
		/// </summary>
		/// <param name="context"></param>
		public void LinkContext(PixyzContext context)
		{
			context._imagesPixyzToUnity = _imagesPixyzToUnity;
			context._materialPixyzToUnity = _materialPixyzToUnity;
			context._meshPixyzToUnity = _meshPixyzToUnity;
		}

		#endregion
	}
}