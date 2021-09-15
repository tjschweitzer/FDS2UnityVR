Shader "Pixyz/NormalMapBlit"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata inVert)
			{
				v2f outVert;
				outVert.vertex = UnityObjectToClipPos(inVert.vertex);
				outVert.uv = inVert.uv;
				return outVert;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float4 outColor;
				//float3 unpacked = (UnpackNormal(tex2D(_MainTex, i.uv)) + 1.0) / 2.0;
				//outColor.r = unpacked.b;
				//outColor.g = unpacked.g;
				//outColor.b = unpacked.r;
				
				outColor.rgb = (UnpackNormal(tex2D(_MainTex, i.uv)) + 1.0) / 2.0;
				outColor.a = 1.0;
				return outColor;
			}
			ENDCG
		}
	}
}
