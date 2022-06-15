Shader "MaskFlow" {
	Properties {
		[Enum(Add,1,Blend,10)] _AddOrBlend ("AddOrBlend", Float) = 1
		[HDR] _Color ("Color", Vector) = (1,1,1,1)
		_Intensity ("Intensity", Float) = 1
		_Opacity ("Opacity", Float) = 1
		_PositionContro ("PositionContro", Vector) = (0,0,0,0)
		_MainTex ("MainTex", 2D) = "white" {}
		[Toggle(_DESATURATE_ON)] _Desaturate ("Desaturate", Float) = 0
		_Mask ("Mask", 2D) = "white" {}
		_MaskFlow ("MaskFlow", 2D) = "white" {}
		_MaskTilingAndOffset ("MaskTilingAndOffset", Vector) = (1,1,0,0)
		_U ("U", Float) = 0
		_V ("V", Float) = 0
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}