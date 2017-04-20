// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CygenGames/Simple Sky"
{
	Properties
	{
		_CloudTex ("Clouds (A)", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "white" {}
		_CloudColor ("Cloud Color", Color) = (0.5, 0.5, 0.5, 1)
		_CloudCover ("Cloud Cover", Range(0, 1)) = 0
		_SkyIntensity ("Sky intensity", Range(0.25, 1)) = 1
		_CloudOffset ("Cloud Offset", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"IgnoreProjector" = "True"
		}

		Lighting Off
		Cull Off

		Pass
		{
			CGPROGRAM
				#include "UnityCG.cginc"
				#pragma vertex vert
				#pragma fragment frag

				struct v2f
				{
					float4 pos : SV_POSITION;
					fixed2 uv1: TEXCOORD0;
					half4 color : COLOR;
				};

				sampler2D _CloudTex;
				sampler2D _NoiseTex;
				float4 _CloudColor;
				float _CloudCover;
				fixed _SkyIntensity;
				float _CloudOffset;

				v2f vert (appdata_full v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex);
					o.uv1 = v.texcoord;
					o.uv1.x = o.uv1.x * 0.5 + _CloudOffset;
					o.uv1.y = o.uv1.y * 0.98 + 0.1;
					o.color = v.color;

					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					float a1 = clamp(tex2D(_CloudTex, i.uv1).a - (1 - _CloudCover), 0, 1);
					half4 c = half4(a1, a1, a1, 1) * _CloudColor.a * _CloudColor;

					c.rgb = saturate(i.color.rgb * _SkyIntensity + c.rgb);
					c.a = 1;

					return c;
				}

			ENDCG
		}
	} 
}
