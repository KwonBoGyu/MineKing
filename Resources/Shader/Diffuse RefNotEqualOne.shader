Shader "Custom/Stencil/Diffuse NotEqualOne"
{

	Properties
	{
		_MainTex("Base (RGBA)", 2D) = "white" {}
		_SubTex("HeadLight (RGBA)", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Geometry" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _SubTex;

		struct Input
		{
			float2 uv_MainTex;
			float2 uv_SubTex;
		};

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = 1;
			//o.Albedo *= tex2D(_SubTex, IN.uv2_SubTex).a;
		}

		ENDCG
	}

		Fallback "VertexLit"
}
