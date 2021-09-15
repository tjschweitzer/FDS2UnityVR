using UnityEngine;
using UnityEngine.Rendering;

namespace Pixyz.Commons.Utilities
{
    public static class ShaderUtilities
    {
		public static Shader GetPixyzLineShader()
		{
			Shader shader = Shader.Find("Pixyz/Simple Lines");
			if (shader != null)
				return shader;

			return GetDefaultShader();
		}
		public static Shader GetPixyzSplatsShader()
		{
			Shader shader = Shader.Find("Pixyz/Splats");
			if (shader != null)
				return shader;

			return GetDefaultShader();
		}

		public static Shader GetDefaultShader()
		{
			Shader shader = null;
#if UNITY_2019_1_OR_NEWER
			if (GraphicsSettings.renderPipelineAsset)
			{
				shader = UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset.defaultShader;
			}
#else
		if (Rendering.GraphicsSettings.renderPipelineAsset)
		{
			shader = Rendering.GraphicsSettings.renderPipelineAsset.GetDefaultShader();
		}
#endif
			if (shader == null)
				shader = Shader.Find("Standard");
			return shader;
		}
	}
}