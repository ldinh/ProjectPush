// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CygenGames/Star"
{
	Properties
	{
		_StarTex ("Star", 2D) = "white" {}
		_Brightness ("Brightness", Range(0, 1)) = 1
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Transparent"
			"Queue"="Geometry+100"
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
					fixed2 uv: TEXCOORD0;
					half4 color : COLOR;
				};

				sampler2D _StarTex;
				float _Brightness;

				v2f vert (appdata_full v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos (v.vertex);
					o.uv = v.texcoord;
					o.color = v.color;
					
					// Version 1.0.4 - removed to implement rotating star plane
					//o.color.a = o.color.a * (2 * (_ScreenParams.y * 0.5 + v.vertex.y + 0.5) / _ScreenParams.y + 0.25);
					
					return o;
				}

				half4 frag(v2f i) : COLOR
				{
					half4 c =  tex2D(_StarTex, i.uv) * i.color;
					c.a *= _Brightness;

					return c;
				}

			ENDCG
		}
	} 
}
