using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Pixyz.LODTools
{
	/// <summary>
	/// Class used to store infos about previous LOD generation du quickly re-apply previous settings, sources
	/// also it can give a hints if the LODGroup is "dirty" (renderers moved, mesh changed, materials changed, ect...)
	/// </summary>
	[RequireComponent(typeof(LODGroup))]
	public class LODGenerationData : MonoBehaviour
	{
		[SerializeField]
		private LODProcess _processUsed = null;

		[SerializeField]
		private long _generationHash = 1;

		[SerializeField]
		private List<Renderer> _source = new List<Renderer>();

		[SerializeField]
		private List<long> _sourceHash = null;

		/// <summary>
		/// Renderers used as source for the last generation
		/// </summary>
		public ReadOnlyCollection<Renderer> SourceRenderers => _source.AsReadOnly();
		
		/// <summary>
		/// The full process used to generate the last generation
		/// </summary>
		public LODProcess ProcessUsed => _processUsed;

		/// <summary>
		/// The hash of the process when used for the last generation
		/// </summary>
		public long GenerationProcessHash => _generationHash;

		/// <summary>
		/// Change the process used to generate this LODGroup, it will automatically be set as dirty
		/// </summary>
		/// <param name="process"></param>
		public void SetNewProcess(LODProcess process)
		{
			_generationHash = process != _processUsed ? 1 : _generationHash;
			_processUsed = process;
		}

		/// <summary>
		/// Register info from a new LOD generation and generate corresponding hashes
		/// </summary>
		/// <param name="process"></param>
		/// <param name="source"></param>
		public void SetNewGeneration(LODProcess process, List<Renderer> source)
		{
			_processUsed = process;
			_generationHash = process.ComputeHash();
			_sourceHash = new List<long>(source.Count);
			_source = new List<Renderer>(source);

			foreach (Renderer src in source)
			{
				_sourceHash.Add(GenerateRendererHash(src));
			}
		} 

		/// <summary>
		/// Is the current state of the LODGroup representative of the last generation ?
		/// </summary>
		/// <returns></returns>
		public bool IsDirty()
		{
			if (_processUsed == null)
				return false;

			if (_processUsed.ComputeHash() != _generationHash)
				return true;

			for(int i = 0; i < _source.Count; ++i)
			{
				Renderer r = _source[i];
				if (r == null)
					return true;

				if (GenerateRendererHash(r) != _sourceHash[i])
					return true; 
			}

			return false;
		}

		/// <summary>
		/// Generate a hash for the provided renderer (MeshRenderers & SkinnedRenderers only)
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
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
			hash ^= (int)mesh.bounds.size.sqrMagnitude;

			if (r is SkinnedMeshRenderer)
			{
				SkinnedMeshRenderer skinnedRenderer = r as SkinnedMeshRenderer;

				foreach(Transform t in skinnedRenderer.bones)
				{
					hash *= 0x1B02B0AAE5DD3A00;
					hash ^= t.localToWorldMatrix.GetHashCode();
				}
			}

			hash ^= mesh.normals.Length;
			hash ^= mesh.uv.Length;
			hash ^= mesh.uv2.Length;
			hash ^= mesh.vertexCount;

			foreach (var material in r.sharedMaterials)
			{
				hash *= 0x1EF31D238836D08C;
				hash += material.ComputeCRC();
			}

			hash ^= r.transform.localToWorldMatrix.GetHashCode();

			return hash;
		}
	}
}