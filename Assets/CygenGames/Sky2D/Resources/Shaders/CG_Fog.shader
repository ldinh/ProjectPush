// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CygenGames/Sky"
{
	Properties
	{
		_FogTex ("Fog (A)", 2D) = "white" {}
		_FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
		_FogCover ("Fog Cover", Range(0, 1)) = 0
		_FogOffset ("Fog Offset", Float) = 0
		_FogTilingX ("Fog Tiling X", Float) = 1
		_FogTilingY ("Fog Tiling Y", Float) = 1
		_VerticalMotion ("Vertical Motion", Range(-5, 5)) = 0.1
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Transparent"
			"IgnoreProjector" = "True"
		}

		Lighting Off
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200

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
				};

				sampler2D _FogTex;
				float4 _FogColor;
				float _FogCover;
				fixed _SkyIntensity;
				float _FogOffset;
				float _FogTilingX;
				float _FogTilingY;
				float _VerticalMotion;

				v2f vert (appdata_full v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex);
					o.uv1 = v.texcoord;
					o.uv1.x = (o.uv1.x * 0.5 + _FogOffset) * _FogTilingX;
					o.uv1.y = (o.uv1.y * 0.98 + 0.1) * _FogTilingY;
					o.uv2 = float2(v.texcoord.y, v.texcoord.x);
					o.uv2.x = (o.uv2.x + _Time.x * _VerticalMotion) * _FogTilingY;
					o.uv2.y = (o.uv2.y * 0.98 + 0.1) * _FogTilingX;

					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					float a1 = clamp(tex2D(_FogTex, i.uv1).a - (1 - _FogCover), 0, 1);
					float a2 = clamp(tex2D(_FogTex, i.uv2).a - (1 - _FogCover), 0, 1);
					half4 c = saturate(half4(a1, a1, a1, 1) + half4(a2, a2, a2, 1));
					c.a = c.r;
					c *= _FogColor;

					return c;
				}

			ENDCG
		}
	} 
}
