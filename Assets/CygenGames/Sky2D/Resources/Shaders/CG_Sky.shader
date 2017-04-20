// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CygenGames/Sky"
{
	Properties
	{
		_CloudTex ("Clouds (A)", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "white" {}
		_CloudColor ("Cloud Color", Color) = (0.5, 0.5, 0.5, 1)
		_CloudCover ("Cloud Cover", Range(0, 1)) = 0
		_SkyIntensity ("Sky intensity", Range(0.25, 1)) = 1
		_CloudOffset ("Cloud Offset", Float) = 0
		_VerticalMotion ("Vertical Motion", Range(-1, 1)) = 0.1
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Background"
			"IgnoreProjector" = "True"
		}

		Fog { Mode Off }
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
					float2 uv2 : TEXCOORD1;
					float2 uv3 : TEXCOORD2;
					half4 color : COLOR;
				};

				sampler2D _CloudTex;
				sampler2D _NoiseTex;
				float4 _CloudColor;
				float _CloudCover;
				fixed _SkyIntensity;
				float _CloudOffset;
				float _VerticalMotion;

				v2f vert (appdata_full v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex);
					o.uv1 = v.texcoord;
					o.uv1.x = o.uv1.x * 0.5 + _CloudOffset;
					o.uv1.y = o.uv1.y * 0.98 + 0.1;
					o.uv2 = float2(v.texcoord.y, v.texcoord.x);
					o.uv2.x = o.uv2.x + _Time.x * _VerticalMotion;
					o.uv2.y = o.uv2.y * 0.98 + 0.1;
					o.uv3 = v.texcoord * 40;
					o.color = v.color;

					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					float a1 = clamp(tex2D(_CloudTex, i.uv1).a - (1 - _CloudCover), 0, 1);
					float a2 = clamp(tex2D(_CloudTex, i.uv2).a - (1 - _CloudCover), 0, 1);
					half4 clouds = saturate((half4(a1, a1, a1, 1) + half4(a2, a2, a2, 1)) * _CloudColor.a);
					half4 c = clouds * _CloudColor;

					c.rgb = saturate(i.color.rgb * _SkyIntensity + c.rgb);
					c.rgb *= tex2D(_NoiseTex, i.uv3).rgb;
					c.a = 1;

					return c;
				}

			ENDCG
		}
	} 
}
