// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Stencil/Mask OneZLess"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		//플레이어 위치값
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Geometry-1" }
		ColorMask 0
		ZWrite off

		Stencil
		{
			Ref 1
			Comp always
			Pass replace
		}

		Pass
		{
			Cull Off
			//ZTest Less

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
			};
			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D  _MainTex;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				return o;
			}
			half4 frag(v2f i) : COLOR
			{
				return i.color;
			}
			ENDCG
		}
	}
}