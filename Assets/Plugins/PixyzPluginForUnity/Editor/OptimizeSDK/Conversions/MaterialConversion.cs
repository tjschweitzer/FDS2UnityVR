using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Pixyz.Commons.Utilities;
using Pixyz.Commons.Extensions;

namespace Pixyz.OptimizeSDK
{
	public static partial class Conversions
	{
		#region MaterialExtensions

		private static void SetTexture(this UnityEngine.Material material, string property, Native.Material.Texture textureExtract, ref Dictionary<uint, Texture2D> textureMap)
		{
			if (!material.HasProperty(property))
				return;
			material.SetTextureOffset(property, new Vector2((float)textureExtract.offset.x, (float)textureExtract.offset.y));
			material.SetTextureScale(property, new Vector2((float)textureExtract.tilling.x, (float)textureExtract.tilling.y));
			material.SetTexture(property, GetTexture(textureExtract, ref textureMap));
		}

		public static bool TryGetTexture(this UnityEngine.Material material, string texturePropertyName, out Native.Material.Texture texture, ref Dictionary<Texture2D, uint> texMap)
		{
			if (!material.HasProperty(texturePropertyName))
			{
				texture = null;
				return false;
			}
			Texture2D texture2D = material.GetTexture(texturePropertyName) as Texture2D;
			if (!texture2D)
			{
				texture = null;
				return false;
			}
			if (!texMap.ContainsKey(texture2D))
			{
				Debug.LogError($"Texture '{texture2D.name}' haven't been converted to a Native PiXYZ texture");
				texture = null;
				return false;
			}
			texture = new Native.Material.Texture();
			texture.offset = new Native.Geom.Point2();
			Vector2 offset = material.GetTextureOffset(texturePropertyName);
			texture.offset.x = offset.x;
			texture.offset.y = offset.y;
			texture.tilling = new Native.Geom.Point2();
			Vector2 tilling = material.GetTextureScale(texturePropertyName);
			texture.tilling.x = tilling.x;
			texture.tilling.y = tilling.y;
			texture.image = texMap[texture2D];
			return true;
		}

		public static Color TryGetColor(this Material material, string colorPropertyName)
		{
			if (!material.HasProperty(colorPropertyName))
			{
				return Color.white;
			}
			return material.GetColor(colorPropertyName);
		}

		public static float TryGetFloat(this Material material, string floatPropertyName, float defaultValue = 0f)
		{
			if (!material.HasProperty(floatPropertyName))
			{
				return defaultValue;
			}
			return material.GetFloat(floatPropertyName);
		}

		#endregion

		#region TextureUtilities

		private static Texture2D GetTexture(Native.Material.Texture textureExtract, ref Dictionary<uint, Texture2D> textureMap)
		{
			if (!textureMap.ContainsKey(textureExtract.image))
			{
				if (textureExtract.image == 0)
				{
					return null;
				}
				else
				{
					textureMap.Add(textureExtract.image, ConvertImageDefinition(Native.NativeInterface.GetImage(textureExtract.image)));
				}
			}
			return textureMap[textureExtract.image];
		}

		private static Texture2D CreateSpecGlossMap(Texture2D specular, Texture2D roughness)
		{
			if (specular == null || roughness == null)
				return null;
			if (specular.width != roughness.width || specular.height != roughness.height)
			{
				Debug.LogError("Specular/metallic map and roughness map must be of the same size");
				return specular;
			}

			Texture2D specGloss = new Texture2D(specular.width, specular.height, TextureFormat.RGBA32, false);
			specGloss.name = specular.name;

			Color[] colorsSpecular = specular.GetPixels();
			Color[] colorsRoughness = roughness.GetPixels();

			for (int i = 0; i < colorsSpecular.Length; i++)
			{
				Color colorSpecular = colorsSpecular[i];
				Color colorRoughness = colorsRoughness[i];
				colorsSpecular[i] = new Color(colorSpecular.r, colorSpecular.g, colorSpecular.b, 1f - colorRoughness.r);
			}

			specGloss.SetPixels(colorsSpecular);
			specGloss.Apply();
			return specGloss;
		}
		#endregion

		#region ImageDefinition <> Texture2D

		public static Native.Material.ImageDefinition ConvertTexture(Texture2D texture, bool isNormalMap = false)
		{
			Native.Material.ImageDefinition imageDefinition = new Native.Material.ImageDefinition();

			imageDefinition.name = texture.name;
			imageDefinition.width = texture.width;
			imageDefinition.height = texture.height;
			imageDefinition.data = new Native.Core.ByteList();

			switch (texture.format)
			{
				case TextureFormat.R8:
					imageDefinition.bitsPerComponent = 8;
					imageDefinition.componentsCount = 1;
					imageDefinition.data.list = texture.GetRawTextureData();
					break;
				case TextureFormat.R16:
					imageDefinition.bitsPerComponent = 8;
					imageDefinition.componentsCount = 2;
					imageDefinition.data.list = texture.GetRawTextureData();
					break;
				case TextureFormat.RGB24:
					imageDefinition.bitsPerComponent = 8;
					imageDefinition.componentsCount = 3;
					imageDefinition.data.list = texture.GetRawTextureData();
					break;
				case TextureFormat.RGBA32:
				default:
					Texture2D tempTex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
					Color32[] pixels = TextureUtilities.GetColor32(texture, isNormalMap);

					tempTex.SetPixels32(pixels);
					tempTex.Apply();

					byte[] rawData = tempTex.GetRawTextureData();

					imageDefinition.data.list = new byte[rawData.Length];
					imageDefinition.bitsPerComponent = 8;
					imageDefinition.componentsCount = 4;

					System.Array.Copy(rawData, imageDefinition.data.list, rawData.Length);

					GameObject.DestroyImmediate(tempTex);

					break;
			}

			return imageDefinition;
		}

		public static Texture2D ConvertImageDefinition(Native.Material.ImageDefinition imageDefinition)
		{
			TextureFormat format = TextureFormat.Alpha8;
			switch (imageDefinition.componentsCount)
			{
				case 1:
					if (imageDefinition.bitsPerComponent == 8)
						format = TextureFormat.R8;
					else if (imageDefinition.bitsPerComponent == 16)
						format = TextureFormat.R16;
					break;
				case 2:
					if (imageDefinition.bitsPerComponent == 8)
						format = TextureFormat.RG16;
					break;
				case 3:
					if (imageDefinition.bitsPerComponent == 8)
						format = TextureFormat.RGB24;
					break;
				case 4:
					if (imageDefinition.bitsPerComponent == 8)
						format = TextureFormat.RGBA32;
					break;
				default:
					break;
			}
			Texture2D texture = new Texture2D(imageDefinition.width, imageDefinition.height, format, false);
			texture.name = imageDefinition.name;

			if (imageDefinition.width <= 0 || imageDefinition.height <= 0)
			{
				Debug.LogWarning($"Texture '{texture.name}' is 0x0 and was ignored");
				return null;
			}
			if (imageDefinition.width > 16384 || imageDefinition.height > 16384)
			{
				Debug.LogWarning($"Texture '{texture.name}' is larger than 16384, which is not supported");
				return null;
			}
			try
			{
				if (imageDefinition.data != null && imageDefinition.data.length > 0)
				{
					texture.LoadRawTextureData(imageDefinition.data);
				}
			}
			catch
			{
				Debug.LogWarning($"Texture data for '{texture.name}' is corrupted and couldn't be converted properly.");
			}
			texture.Apply();

			return texture;
		}
		#endregion

		#region MaterialDefinition <> Material


		private static void CombineAlbedoAndOpacity(Native.Material.Texture albedo, Native.Material.Texture opacity, Dictionary<uint, Texture2D> textures)
		{
			Texture2D albedotex = GetTexture(albedo, ref textures);
			Texture2D opacitytex = GetTexture(opacity, ref textures);

			Texture2D combined = null;
			if (albedotex != null && opacitytex != null)
			{
				combined = new Texture2D(albedotex.width, albedotex.height, TextureFormat.RGBA32, false);

				Color[] albedoColors = albedotex.GetPixels();
				Color[] opacityColors = opacitytex.GetPixels();
				for (int i = 0; i < albedoColors.Length; i++)
				{
					Color c = albedoColors[i];
					albedoColors[i] = new Color(c.r, c.g, c.b, opacityColors[i].r);
				}
				combined.SetPixels(albedoColors);
				combined.Apply();
			}
			else
			{
				if (albedotex == null)
					combined = opacitytex;
				else
					combined = albedotex;
			}

			textures[albedo.image] = combined;
		}

		public static void ConvertMaterialExtract(Native.Material.MaterialDefinition materialExtract, ref UnityEngine.Material material, ref Dictionary<uint, Texture2D> textureMap)
		{
			material.name = materialExtract.name;

			float alpha = (materialExtract.opacity._type == Native.Material.CoeffOrTexture.Type.COEFF ? (float)materialExtract.opacity.coeff : 1f);

			material.SetFloat("_Cutoff", 0.5f);

			if (materialExtract.opacity._type == Native.Material.CoeffOrTexture.Type.TEXTURE)
			{
				CombineAlbedoAndOpacity(materialExtract.albedo.texture, materialExtract.opacity.texture, textureMap);
				material.SetFloat("_Mode", 1);
				material.SetOverrideTag("RenderType", "TransparentCutout");
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.Zero);
				material.SetInt("_ZWrite", 1);
				material.SetFloat("_Cutoff", 0.5f);
				material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f, alpha)); // Standard
				material.SetColor("_BaseColor", new Color(1.0f, 1.0f, 1.0f, alpha)); // HDRP and LWRP
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
			}
			else if (alpha < 1f)
			{
				//Set to Fade, if we would set it to transparent we could have issue with metallic making it opaque
				material.SetFloat("_Mode", 3);
				material.SetOverrideTag("RenderType", "Transparent");
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				material.SetInt("_ZWrite", 0);
				material.DisableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 3000;
			}

			if (materialExtract.albedo._type == Native.Material.ColorOrTexture.Type.COLOR)
			{
				Color color = new Color((float)materialExtract.albedo.color.r, (float)materialExtract.albedo.color.g, (float)materialExtract.albedo.color.b, alpha);
				material.SetColor("_Color", color); // Standard
				material.SetColor("_BaseColor", color); // HDRP and LWRP
			}
			else if (materialExtract.albedo._type == Native.Material.ColorOrTexture.Type.TEXTURE)
			{
				material.SetTexture("_MainTex", materialExtract.albedo.texture, ref textureMap);
			}

			if (materialExtract.normal._type == Native.Material.ColorOrTexture.Type.TEXTURE)
			{
				material.EnableKeyword("_NORMALMAP");
				material.SetTexture("_BumpMap", materialExtract.normal.texture, ref textureMap);
			}

			if (materialExtract.roughness._type == Native.Material.CoeffOrTexture.Type.TEXTURE)
			{
				material.EnableKeyword("_METALLICGLOSSMAP");
				Texture2D roughness = GetTexture(materialExtract.roughness.texture, ref textureMap);
				Texture2D specular = roughness;
				if (materialExtract.metallic._type == Native.Material.CoeffOrTexture.Type.TEXTURE)
				{
					specular = GetTexture(materialExtract.metallic.texture, ref textureMap);
				}
				else
				{
					//Unity does not handle transparency with metallic. If there is metallic the material will behave as an opaque mode
					float metallicCoeff = Mathf.Clamp((float)materialExtract.metallic.coeff, 0, 1);
					if(alpha < 1 && material.shader.name == ShaderUtilities.GetDefaultShader().name)
						material.SetFloat("_Metallic", 0);
					else
						material.SetFloat("_Metallic", metallicCoeff);
				}
				material.SetTexture("_MetallicGlossMap", CreateSpecGlossMap(specular, roughness));
			}
			else
			{
				material.SetFloat("_Glossiness", 1 - Mathf.Clamp((float)materialExtract.roughness.coeff, 0, 1));

				if (materialExtract.metallic._type == Native.Material.CoeffOrTexture.Type.TEXTURE)
				{
					material.EnableKeyword("_METALLICGLOSSMAP");
					Texture2D metallic = GetTexture(materialExtract.metallic.texture, ref textureMap);
					material.SetTexture("_MetallicGlossMap", metallic);
				}
				else
				{
					//Unity does not handle transparency with metallic. If there is metallic the material will behave as an opaque mode
					float metallicCoeff = Mathf.Clamp((float)materialExtract.metallic.coeff, 0, 1);
					if(alpha < 1 && material.shader.name == ShaderUtilities.GetDefaultShader().name)
						material.SetFloat("_Metallic", 0);
					else
						material.SetFloat("_Metallic", metallicCoeff);
				}
			}

			if (materialExtract.ao._type == Native.Material.CoeffOrTexture.Type.COEFF)
				material.SetFloat("_OcclusionStrength", Mathf.Clamp((float)materialExtract.ao.coeff, 0, 1));
			else if (materialExtract.ao._type == Native.Material.CoeffOrTexture.Type.TEXTURE)
				material.SetTexture("_OcclusionMap", materialExtract.ao.texture, ref textureMap);
		}

		public static UnityEngine.Material ConvertMaterialExtract(Native.Material.MaterialDefinition materialExtract, ref Dictionary<uint, Texture2D> textureMap, Shader shader = null)
		{
			var material = new UnityEngine.Material(shader == null ? ShaderUtilities.GetDefaultShader() : shader); // todo: pick right shader
			ConvertMaterialExtract(materialExtract, ref material, ref textureMap);
			return material;
		}

		public static UnityEngine.Material[] ConvertMaterialExtracts(Native.Material.MaterialDefinitionList materialExtractList, ref Dictionary<uint, Texture2D> textureMap, Shader shader = null)
		{
			var materials = new UnityEngine.Material[materialExtractList.length];
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i] = ConvertMaterialExtract(materialExtractList[i], ref textureMap, shader);
			}
			return materials;
		}

		public static Native.Material.MaterialDefinition CreateStandardPixyzMaterial(string name, UnityEngine.Color baseColor)
		{
			Native.Material.MaterialDefinition nativeMaterial = new Native.Material.MaterialDefinition();
			nativeMaterial.name = name;

			Native.Core.ColorAlpha mainColor = baseColor.ToPiXYZColorAlpha();

			nativeMaterial.opacity = new Native.Material.CoeffOrTexture();
			nativeMaterial.opacity._type = Native.Material.CoeffOrTexture.Type.COEFF;
			nativeMaterial.opacity.coeff = mainColor.a;

			nativeMaterial.albedo = new Native.Material.ColorOrTexture();
			nativeMaterial.albedo._type = Native.Material.ColorOrTexture.Type.COLOR;
			nativeMaterial.albedo.color = mainColor.ColorAlphaToColor();

			nativeMaterial.normal = new Native.Material.ColorOrTexture();
			nativeMaterial.normal._type = Native.Material.ColorOrTexture.Type.COLOR;
			nativeMaterial.normal.color = Color.black.ToPiXYZColor();

			nativeMaterial.metallic = new Native.Material.CoeffOrTexture();
			nativeMaterial.metallic._type = Native.Material.CoeffOrTexture.Type.COEFF;
			nativeMaterial.metallic.coeff = 0.5;

			nativeMaterial.roughness = new Native.Material.CoeffOrTexture();
			nativeMaterial.roughness._type = Native.Material.CoeffOrTexture.Type.COEFF;
			nativeMaterial.roughness.coeff = 0.5;

			nativeMaterial.ao = new Native.Material.CoeffOrTexture();
			nativeMaterial.ao._type = Native.Material.CoeffOrTexture.Type.COEFF;
			nativeMaterial.ao.coeff = 0;

			return nativeMaterial;
		}

		public static Native.Material.MaterialDefinition ConvertMaterial(UnityEngine.Material material, ref Dictionary<Texture2D, uint> texMap)
		{
			Native.Material.MaterialDefinition materialExtract = new Native.Material.MaterialDefinition();
			materialExtract.name = material.name;
			materialExtract.id = material.GetInstanceID().ToUInt32();

			Color mainColor = Color.white;
			if (material.HasProperty("_Color"))
			{
				mainColor = material.GetColor("_Color");
			}
			else if (material.HasProperty("_BaseColor"))
			{
				mainColor = material.GetColor("_BaseColor");
			}

			materialExtract.opacity = new Native.Material.CoeffOrTexture();
			materialExtract.opacity._type = Native.Material.CoeffOrTexture.Type.COEFF;

			if (material.HasProperty("_Mode"))
			{
				if (material.GetFloat("_Mode") != 0)
					materialExtract.opacity.coeff = mainColor.a;
				else
					materialExtract.opacity.coeff = 1.0;
			}

			materialExtract.albedo = new Native.Material.ColorOrTexture();
			if (material.TryGetTexture("_MainTex", out Native.Material.Texture albedoTexture, ref texMap))
			{
				materialExtract.albedo._type = Native.Material.ColorOrTexture.Type.TEXTURE;
				materialExtract.albedo.texture = albedoTexture;
			}
			else
			{
				materialExtract.albedo._type = Native.Material.ColorOrTexture.Type.COLOR;
				materialExtract.albedo.color = mainColor.ToPiXYZColor();
			}

			materialExtract.normal = new Native.Material.ColorOrTexture();
			if (material.TryGetTexture("_BumpMap", out Native.Material.Texture normalTexture, ref texMap))
			{
				materialExtract.normal._type = Native.Material.ColorOrTexture.Type.TEXTURE;
				materialExtract.normal.texture = normalTexture;
			}
			else
			{
				materialExtract.normal._type = Native.Material.ColorOrTexture.Type.COLOR;
			}

			materialExtract.metallic = new Native.Material.CoeffOrTexture();
			if (material.TryGetTexture("_MetallicGlossMap", out Native.Material.Texture metallicTexture, ref texMap))
			{
				materialExtract.metallic._type = Native.Material.CoeffOrTexture.Type.TEXTURE;
				materialExtract.metallic.texture = metallicTexture;
			}
			else
			{
				materialExtract.metallic._type = Native.Material.CoeffOrTexture.Type.COEFF;
				materialExtract.metallic.coeff = material.TryGetFloat("_Metallic", 0.5f);
			}

			materialExtract.roughness = new Native.Material.CoeffOrTexture();
			materialExtract.roughness._type = Native.Material.CoeffOrTexture.Type.COEFF;
			if (material.HasProperty("_Glossiness"))
				materialExtract.roughness.coeff = 1.0 - material.TryGetFloat("_Glossiness", 0.5f);

			materialExtract.ao = new Native.Material.CoeffOrTexture();
			if (material.TryGetTexture("_OcclusionMap", out Native.Material.Texture occlusionTexture, ref texMap))
			{
				materialExtract.ao._type = Native.Material.CoeffOrTexture.Type.TEXTURE;
				materialExtract.ao.texture = occlusionTexture;
			}
			else
			{
				materialExtract.ao._type = Native.Material.CoeffOrTexture.Type.COEFF;
				materialExtract.ao.coeff = material.TryGetFloat("_OcclusionStrength", 1.0f);
			}

			return materialExtract;
		}
		#endregion
	}
}
