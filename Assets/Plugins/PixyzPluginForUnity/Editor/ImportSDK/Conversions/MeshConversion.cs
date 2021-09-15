using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Pixyz.Commons.Extensions;
using System.Linq;

namespace Pixyz.ImportSDK
{
	public static partial class Conversions
	{
		#region MeshDefinition <> Mesh

		public static Native.Geom.Point3List ExtractVerticesFromMesh(in Mesh mesh)
		{
			Vector3[] uvertices = mesh.vertices;
			Native.Geom.Point3List ivertices = new Native.Geom.Point3List(uvertices.Length);
			for (int i = 0; i < uvertices.Length; i++)
			{
				ivertices[i] = new Native.Geom.Point3 { x = uvertices[i].x, y = uvertices[i].y, z = uvertices[i].z };
			}

			return ivertices;
		}

		public static Native.Core.ColorAlphaList ExtractColorsFromMesh(in Mesh mesh)
		{
			Color[] ucolors = mesh.colors;
			Native.Core.ColorAlphaList icolors = new Native.Core.ColorAlphaList(ucolors.Length);
			for (int i = 0; i < ucolors.Length; i++)
			{
				icolors[i] = new Native.Core.ColorAlpha { r = ucolors[i].r, g = ucolors[i].g, b = ucolors[i].b, a = ucolors[i].a };
			}

			return icolors;
		}

		public static Native.Geom.Vector3List ExtractNormalsFromMesh(in Mesh mesh)
		{
			Vector3[] unormals = mesh.normals;
			Native.Geom.Vector3List inormals = new Native.Geom.Vector3List(unormals.Length);
			for (int i = 0; i < unormals.Length; i++)
			{
				unormals[i].Normalize();
				inormals[i] = new Native.Geom.Point3 { x = unormals[i].x, y = unormals[i].y, z = unormals[i].z };
			}

			return inormals;
		}

		public static Native.Geom.Vector4List ExtractTangentsFromMesh(in Mesh mesh)
		{
			Vector4[] utangents = mesh.tangents;
			Native.Geom.Vector4List itangents = new Native.Geom.Vector4List(utangents.Length);
			for (int i = 0; i < utangents.Length; i++)
			{
				itangents[i] = new Native.Geom.Point4 { x = utangents[i].x, y = utangents[i].y, z = utangents[i].z, w = utangents[i].w };
			}

			return itangents;
		}

		public static void ExtractUVsFromMesh(in Mesh mesh, out Native.Geom.Point2ListList uvsPixyz, out Native.Core.IntList channelsPixyz)
		{
			uvsPixyz = new Native.Geom.Point2ListList(8);
			var channels = new List<int>();
			for (int j = 0; j < 8; j++)
			{
				var uvs = new List<Vector2>();
				mesh.GetUVs(j, uvs);
				uvsPixyz[j] = new Native.Geom.Point2List(uvs.Count);
				if (uvs.Count > 0)
				{
					channels.Add(j);
					for (int i = 0; i < uvs.Count; i++)
					{
						uvsPixyz[j].list[i] = new Native.Geom.Point2 { x = uvs[i].x, y = uvs[i].y };
					}
				}
			}

			channelsPixyz = new Native.Core.IntList(channels.ToArray());
		}

		public static void ExtractTriangles(in Mesh mesh, int subMeshIndex, ref List<int> triangles, ref Native.Polygonal.DressedPoly dressedPoly)
		{
			var utriangles = mesh.GetTriangles(subMeshIndex);
			int firstTriInt = triangles.Count;
			triangles.AddRange(utriangles);
			dressedPoly.firstTri = firstTriInt / 3;
			dressedPoly.firstQuad = -1;
			int triCountInt = (int)mesh.GetIndexCount(subMeshIndex);
			dressedPoly.triCount = triCountInt / 3;
			dressedPoly.quadCount = 0;

			for (int i = 0; i < triCountInt; i += 3)
			{
				triangles[firstTriInt + i] = utriangles[i];
				triangles[firstTriInt + i + 1] = utriangles[i + 1];
				triangles[firstTriInt + i + 2] = utriangles[i + 2];
			}
		}

		public static List<Native.Geom.Point3> ExtractLines(in Mesh mesh, int subMeshIndex, ref Native.Polygonal.StylizedLine line)
		{
			var lineVertices = new List<Native.Geom.Point3>();

			SubMeshDescriptor subMeshDescriptor = mesh.GetSubMesh(subMeshIndex);
			Vector3[] uvertices = mesh.vertices;
			int[] uindices = mesh.GetIndices(subMeshIndex);

			line.color = new Native.Core.ColorAlpha();
			// todo: handle colors properly
			line.color.r = 0f; //_materials[s].albedo.color.r;
			line.color.g = 0f; //_materials[s].albedo.color.g;
			line.color.b = 0f; //_materials[s].albedo.color.b;
			line.color.a = 1;
			line.lines = new Native.Core.IntList(subMeshDescriptor.indexCount);

			Dictionary<int, int> indicesMap = new Dictionary<int, int>();

			for (int i = 0; i < subMeshDescriptor.indexCount; ++i)
			{
				int indice = uindices[i];
				int indiceMaped = 0;
				if (!indicesMap.ContainsKey(indice))
				{
					indiceMaped = lineVertices.Count;
					lineVertices.Add(uvertices[indice].ToPoint3());
					indicesMap.Add(indice, indiceMaped);
				}
				else
				{
					indiceMaped = indicesMap[indice];
				}
				line.lines[i] = indiceMaped;
			}
			return lineVertices;
		}

		public static void ExtractGeomFromMesh(in Mesh mesh, uint[] matIds, out Native.Core.IntList triangleList, out Native.Polygonal.DressedPolyList dressedPoliesList, out Native.Geom.Point3List lineVerticesList, out Native.Polygonal.StylizedLineList linesList, out Native.Geom.Point3List pointsList, out Native.Geom.Vector3List pointsColorList)
		{
			var triangles = new List<int>();
			var dressedPolies = new List<Native.Polygonal.DressedPoly>();
			var lineVertices = new List<Native.Geom.Point3>();
			var lines = new List<Native.Polygonal.StylizedLine>();
			var points = new List<Native.Geom.Point3>();
			var pointsColor = new List<Native.Geom.Vector3>();

			for (int s = 0; s < mesh.subMeshCount; s++)
			{
				MeshTopology topology = mesh.GetTopology(s);

				switch (topology)
				{
					case MeshTopology.Triangles:
						{
							Native.Polygonal.DressedPoly dressedPoly = new Native.Polygonal.DressedPoly();
							ExtractTriangles(mesh, s, ref triangles, ref dressedPoly);

							if (matIds == null)
							{
								// 0 means no material (default)
								dressedPoly.material = 0;
							}
							else
							{
								dressedPoly.material = matIds[s];
							}
							dressedPoly.externalId = (uint)s + 1; // Incremental, starting from 1
							dressedPolies.Add(dressedPoly);
						}
						break;
					case MeshTopology.Lines:
						{
							var submesh = mesh.GetSubmesh(s);
							var line = new Native.Polygonal.StylizedLine();
							line.lines = new Native.Core.IntList(submesh.GetIndices(0));
							lineVertices.AddRange(ExtractLines(mesh, s, ref line));
							line.externalId = (uint)s + 1; // Incremental, starting from 1
							lines.Add(line);
						}
						break;
					case MeshTopology.Points:
						{
							SubMeshDescriptor subMeshDescriptor = mesh.GetSubMesh(s);

							Vector3[] uvertices = new Vector3[subMeshDescriptor.vertexCount];
							Color[] ucolors = new Color[subMeshDescriptor.vertexCount];
							Array.Copy(mesh.vertices, subMeshDescriptor.firstVertex, uvertices, 0, subMeshDescriptor.vertexCount);
							Array.Copy(mesh.colors, subMeshDescriptor.firstVertex, ucolors, 0, subMeshDescriptor.vertexCount);

							Native.Geom.Point3[] tmpPoints = new Native.Geom.Point3[subMeshDescriptor.indexCount];
							Native.Geom.Vector3[] tmpColorPoints = new Native.Geom.Vector3[subMeshDescriptor.indexCount];

							for (int i = 0; i < tmpPoints.Length; ++i)
							{
								tmpPoints[i] = uvertices[subMeshDescriptor.firstVertex + i].ToPoint3();
								Color c = ucolors[subMeshDescriptor.firstVertex + i];
								tmpColorPoints[i] = new Native.Geom.Point3() { x = c.r, y = c.g, z = c.b };
							}
							points.AddRange(tmpPoints);
							pointsColor.AddRange(tmpColorPoints);
						}
						break;
				}
			}

			triangleList = new Native.Core.IntList(triangles.ToArray());
			dressedPoliesList = new Native.Polygonal.DressedPolyList(dressedPolies.ToArray());
			lineVerticesList = new Native.Geom.Point3List(lineVertices.ToArray());
			linesList = new Native.Polygonal.StylizedLineList(lines.ToArray());
			pointsList = new Native.Geom.Point3List(points.ToArray());
			pointsColorList = new Native.Geom.Vector3List(pointsColor.ToArray());
		}

		public static void ExtractJointInfosFromMesh(in Mesh mesh, out Native.Geom.Vector4List weightsList, out Native.Geom.Vector4IList indicesList)
		{
			BoneWeight[] bones = mesh.boneWeights;
			weightsList = new Native.Geom.Vector4List(bones.Length);
			indicesList = new Native.Geom.Vector4IList(bones.Length);

			for (int i = 0; i < bones.Length; ++i)
			{
				Native.Geom.Point4 weights = new Native.Geom.Point4();
				weights.x = bones[i].weight0;
				weights.y = bones[i].weight1;
				weights.z = bones[i].weight2;
				weights.w = bones[i].weight3;
				weightsList[i] = weights;

				Native.Geom.Vector4I joins = new Native.Geom.Vector4I();
				joins.x = bones[i].boneIndex0;
				joins.y = bones[i].boneIndex1;
				joins.z = bones[i].boneIndex2;
				joins.w = bones[i].boneIndex3;
				indicesList[i] = joins;
			}
		}

		public static Native.Geom.Matrix4List ExtractInverseBindPoseFromMesh(in Mesh mesh)
		{
			Native.Geom.Matrix4List inverseBindMatrices = new Native.Geom.Matrix4List(mesh.bindposes.Length);

			for (int i = 0; i < mesh.bindposes.Length; ++i)
			{
				inverseBindMatrices[i] = ConvertMatrix(mesh.bindposes[i]);
			}
			return inverseBindMatrices;
		}

		public static Native.Polygonal.MeshDefinition ConvertMesh(in Mesh mesh, uint[] matIds = null)
		{
			Native.Polygonal.MeshDefinition meshDefinition = new Native.Polygonal.MeshDefinition();

			meshDefinition.vertices = ExtractVerticesFromMesh(mesh);

			meshDefinition.linesVertices = new Native.Geom.Point3List(0);
			meshDefinition.lines = new Native.Polygonal.StylizedLineList(0);
			meshDefinition.triangles = new Native.Core.IntList(0);
			meshDefinition.dressedPolys = new Native.Polygonal.DressedPolyList(0);
			meshDefinition.uvs = new Native.Geom.Point2ListList(0);

			ExtractGeomFromMesh(mesh, matIds, out meshDefinition.triangles, out meshDefinition.dressedPolys, out meshDefinition.linesVertices, out meshDefinition.lines, out meshDefinition.points, out meshDefinition.pointsColors);
			ExtractUVsFromMesh(mesh, out meshDefinition.uvs, out meshDefinition.uvChannels);
			ExtractJointInfosFromMesh(mesh, out meshDefinition.jointWeights, out meshDefinition.jointIndices);

			meshDefinition.normals = ExtractNormalsFromMesh(mesh);
			meshDefinition.tangents = ExtractTangentsFromMesh(mesh);
			meshDefinition.vertexColors = ExtractColorsFromMesh(mesh);
			meshDefinition.inverseBindMatrices = ExtractInverseBindPoseFromMesh(mesh);

			return meshDefinition;
		}

		public static Color[] ExtractColorFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition)
		{
			Color[] vertexColors = new Color[meshDefinition.vertices.length + meshDefinition.linesVertices.length + meshDefinition.points.length];

			int offset = 0;
			// Convert Mesh Vertex Colors
			for (int i = 0; i < meshDefinition.vertexColors.length; i++)
			{
				vertexColors[i] = meshDefinition.vertexColors[i].ToUnityColor();
			}
			offset += meshDefinition.vertices.length;
			// Convert Lines Vertex Colors
			/// No code here (there are not such 'line vertex color yet')
			offset += meshDefinition.linesVertices.length;
			// Convert Free Vertex Colors
			for (int i = 0; i < meshDefinition.pointsColors.length; i++)
			{
				Native.Geom.Point3 color = meshDefinition.pointsColors[i];
				vertexColors[i + offset] = new Color((float)color.x, (float)color.y, (float)color.z, 1f);
			}

			return vertexColors;
		}

		public static Vector3[] ExtractVerticesFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition, float scaleFactor = 1.0f)
		{
			List<Native.Geom.Point3> ivertices = new List<Native.Geom.Point3>(meshDefinition.vertices.list);
			ivertices.AddRange(meshDefinition.linesVertices.list);
			ivertices.AddRange(meshDefinition.points.list);

			Vector3[] vertices = new Vector3[ivertices.Count];

			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = new Vector3((float)ivertices[i].x * scaleFactor, (float)ivertices[i].y * scaleFactor, (float)ivertices[i].z * scaleFactor);
			}

			return vertices;
		}

		public static Vector3[] ExtractNormalsFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition)
		{
			// Prepare Normals
			Native.Geom.Vector3List inormals = meshDefinition.normals;
			int normalsCount = inormals.length + ((inormals.length == 0) ? 0 : meshDefinition.linesVertices.length);
			var normals = new Vector3[normalsCount];
			for (int i = 0; i < inormals.length; i++)
			{
				normals[i] = new Vector3((float)inormals[i]._base.x, (float)inormals[i]._base.y, (float)inormals[i]._base.z);
			}

			// Fill non defined normals with zeros
			for (int i = inormals.length; i < normalsCount; i++)
			{
				normals[i] = Vector3.zero;
			}

			return normals;
		}

		public static Vector4[] ExtractTangentsFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition)
		{
			// Prepare Tangents
			Native.Geom.Vector4List itangents = meshDefinition.tangents;
			int tangentsCount = itangents.length + ((itangents.length == 0) ? 0 : meshDefinition.linesVertices.length);
			Vector4[] tangents = new Vector4[tangentsCount];
			for (int i = 0; i < itangents.length; i++)
			{
				tangents[i] = new Vector4((float)itangents[i]._base.x, (float)itangents[i]._base.y, (float)itangents[i]._base.z, (float)itangents[i]._base.w);
			}

			// Fill non defined tangents with zeros
			for (int i = itangents.length; i < tangentsCount; i++)
			{
				tangents[i] = Vector4.zero;
			}

			return tangents;
		}

		public static Vector2[][] ExtractUVsFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition, Mesh mesh)
		{
			Vector2[][] uvsc = new Vector2[meshDefinition.uvChannels.length][];
			for (int j = 0; j < uvsc.Length; j++)
			{
				Native.Geom.Point2List iuvs = meshDefinition.uvs[meshDefinition.uvChannels[j]];
				Vector2[] uvsj = uvsc[j] = new Vector2[meshDefinition.vertices.list.Length + meshDefinition.linesVertices.list.Length];
				int uvLength = iuvs.length;
				for (int i = 0; i < uvsj.Length; i++)
				{
					if (i < uvLength)
						uvsj[i] = new Vector2((float)iuvs[i].x, (float)iuvs[i].y);
					else
					{
						// To ensure that we have uv size = vertices size.
						// This is useful when having indices without uvs such as lines.
						// This only works if vertices without uvs are at the end of the vertex list.
						uvsj[i] = Vector3.zero;
					}
				}
			}

			return uvsc;
		}

		public static int[][] ExtractTrianglesFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition)
		{
			// Initializing subMeshCount with dressedPoly merged by materials
			int[][] triangles = new int[meshDefinition.dressedPolys.length][];

			Native.Core.IntList iquads = meshDefinition.quadrangles;
			Native.Core.IntList itris = meshDefinition.triangles;

			for (int s = 0; s < triangles.Length; s++)
			{
				// Prepare triangles from quadrangles
				int offset = meshDefinition.dressedPolys[s].firstQuad * 4;
				int tcount = 0;

				tcount += meshDefinition.dressedPolys[s].triCount * 3;
				tcount += meshDefinition.dressedPolys[s].quadCount * 6;

				int triCount = meshDefinition.dressedPolys[s].triCount * 3;
				int quadCount = meshDefinition.dressedPolys[s].quadCount;

				var striangles = triangles[s] = new int[tcount];
				for (int t = 0; t < quadCount; t++)
				{
					int offsetTri = t * 6;
					int offsetQuad = t * 4;
					striangles[offsetTri] = iquads[offset + offsetQuad];
					striangles[offsetTri + 1] = iquads[offset + offsetQuad + 1];
					striangles[offsetTri + 2] = iquads[offset + offsetQuad + 3];

					striangles[offsetTri + 3] = iquads[offset + offsetQuad + 3];
					striangles[offsetTri + 4] = iquads[offset + offsetQuad + 1];
					striangles[offsetTri + 5] = iquads[offset + offsetQuad + 2];
				}

				//Prepare Triangles
				offset = meshDefinition.dressedPolys[s].firstTri * 3;
				for (int t = 0; t < triCount; t += 3)
				{
					striangles[t + quadCount * 6] = itris[offset + t];
					striangles[t + 1 + quadCount * 6] = itris[offset + t + 1];
					striangles[t + 2 + quadCount * 6] = itris[offset + t + 2];
				}
			}
			
			return triangles;
		}

		public static int[][] ExtractLinesFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition)
		{
			List<int[]> edges = new List<int[]>();
			// Prepare lines
			foreach (var line in meshDefinition.lines.list)
			{

				var indices = line.lines;
				if (meshDefinition.vertices.length != 0)
				{

					for (int i = 0; i < line.lines.length; ++i)
						indices[i] += meshDefinition.vertices.length;
				}
				edges.Add(indices);
			};

			return edges.ToArray();
		}

		public static BoneWeight[] ExtractBoneWeightsFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition)
		{
			BoneWeight[] bones = new BoneWeight[meshDefinition.jointWeights.length];

			for (int i = 0; i < meshDefinition.jointWeights.length; ++i)
			{
				Native.Geom.Vector4I joints = meshDefinition.jointIndices[i];
				Native.Geom.Point4 weights = meshDefinition.jointWeights[i];

				bones[i].boneIndex0 = joints.x;
				bones[i].boneIndex1 = joints.y;
				bones[i].boneIndex2 = joints.z;
				bones[i].boneIndex3 = joints.w;

				bones[i].weight0 = (float)weights.x;
				bones[i].weight1 = (float)weights.y;
				bones[i].weight2 = (float)weights.z;
				bones[i].weight3 = (float)weights.w;
			}

			return bones;
		}

		public static Matrix4x4[] ExtractBindPoseFromDefinition(in Native.Polygonal.MeshDefinition meshDefinition)
		{
			Matrix4x4[] bindposes = new Matrix4x4[meshDefinition.inverseBindMatrices.length];

			for (int i = 0; i < meshDefinition.inverseBindMatrices.length; ++i)
			{
				bindposes[i] = ConvertMatrix(meshDefinition.inverseBindMatrices[i]);
			}

			return bindposes;
		}

		public static void ConvertMeshDefinition(in Native.Polygonal.MeshDefinition meshDefinition, Mesh mesh, float scaleFactor = 1.0f)
		{
			if (meshDefinition.dressedPolys.length > 0)
			{
				meshDefinition.dressedPolys.list = meshDefinition.dressedPolys.list.OrderBy(x => x.externalId).ToArray();
			}

			if (meshDefinition.lines.length > 0)
			{
				meshDefinition.lines.list = meshDefinition.lines.list.OrderBy(x => x.externalId).ToArray();
			}

			Vector2[][] uvs = ExtractUVsFromDefinition(meshDefinition, mesh);
			int[][] triangles = ExtractTrianglesFromDefinition(meshDefinition);
			int[][] lines = ExtractLinesFromDefinition(meshDefinition);
			BoneWeight[] weights = ExtractBoneWeightsFromDefinition(meshDefinition);
			Matrix4x4[] bindposes = ExtractBindPoseFromDefinition(meshDefinition);

			// Settings attributes to mesh
			mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
			mesh.vertices = ExtractVerticesFromDefinition(meshDefinition, scaleFactor);
			mesh.normals = ExtractNormalsFromDefinition(meshDefinition);
			mesh.tangents = ExtractTangentsFromDefinition(meshDefinition);
			mesh.colors = ExtractColorFromDefinition(meshDefinition);
			mesh.subMeshCount = triangles.Length + meshDefinition.lines.length + (meshDefinition.points.length > 0 ? 1 : 0);

			for (int j = 0; j < uvs.Length; j++)
			{
				try
				{
					switch (j)
					{
						case -1:
							// Ignore
							break;
						case 0:
							mesh.uv = uvs[j];
							break;
						case 1:
							mesh.uv2 = uvs[j];
							break;
						case 2:
							mesh.uv3 = uvs[j];
							break;
						case 3:
							mesh.uv4 = uvs[j];
							break;
						case 4:
							mesh.uv5 = uvs[j];
							break;
						case 5:
							mesh.uv6 = uvs[j];
							break;
						case 6:
							mesh.uv7 = uvs[j];
							break;
						case 7:
							mesh.uv8 = uvs[j];
							break;
						default:
							Debug.LogError("Mesh doesn't have an UV set n°" + j);
							break;
					}
				}
				catch (Exception exception)
				{
					Debug.LogError(exception);
				}
			}

			int subMeshCount = triangles.Length + lines.Length + (meshDefinition.points.length > 0 ? 1 : 0);
			int usedDressedPoly = 0;
			int usedStylizedLines = 0;

			for (int i = 0; i < subMeshCount; ++i)
			{
				if (usedDressedPoly < meshDefinition.dressedPolys.length && usedStylizedLines < meshDefinition.lines.length)
				{
					if (meshDefinition.lines[usedStylizedLines].externalId < meshDefinition.dressedPolys[usedDressedPoly].externalId)
					{
						mesh.SetIndices(lines[usedStylizedLines], MeshTopology.Lines, i);
						++usedStylizedLines;
					}
					else
					{
						mesh.SetIndices(triangles[usedDressedPoly], MeshTopology.Triangles, i);
						++usedDressedPoly;
					}
				}
				else if (usedStylizedLines < meshDefinition.lines.length)
				{
					mesh.SetIndices(lines[usedStylizedLines], MeshTopology.Lines, i);
					++usedStylizedLines;
				}
				else if (usedDressedPoly < meshDefinition.dressedPolys.length)
				{
					mesh.SetIndices(triangles[usedDressedPoly], MeshTopology.Triangles, i);
					++usedDressedPoly;
				}
				else
				{
					var pointSubmeshes = new List<int>();
					int[] pointsInd = new int[meshDefinition.points.length];
					for (int j = 0; j < pointsInd.Length; j++)
					{
						pointsInd[j] = triangles.Length + lines.Length + j;
					}

					int pointsIndex = triangles.Length + lines.Length;
					mesh.SetIndices(pointsInd, MeshTopology.Points, i, false);
				}
			}

			mesh.boneWeights = weights;
			mesh.bindposes = bindposes;
			mesh.RecalculateBounds();
		}
		#endregion
	}

}