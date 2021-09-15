using UnityEngine;

namespace Pixyz.Commons.Utilities
{
    public static class TextureUtilities
	{
		public static Color32[] GetColor32(Texture texture, bool isNormalMap = false)
		{
			Material blitMat = new Material(Shader.Find("Pixyz/NormalMapBlit"));
			Texture2D tempTex = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, false);
			RenderTexture tempRT = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear/*sRGB*/);

			tempRT.filterMode = FilterMode.Point;
			RenderTexture.active = tempRT;

			if (isNormalMap)
			{
				Graphics.Blit(texture, tempRT, blitMat);
			}
			else
			{
				Graphics.Blit(texture, tempRT);
			}

			tempTex.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
			tempTex.Apply();

			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(tempRT);

			return tempTex.GetPixels32();
		}
	}
}

