Shader "CelShading/Standard" {
	Properties {
		_ToonRamp ("Toon Ramp", 2D) = "white" {}
		[HDR] _RimColor ("Rim Color", Vector) = (0,1,0.8758622,0)
		_RimPower ("Rim Power", Range(0, 10)) = 0.5
		_RimOffset ("Rim Offset", Float) = 0.24
		_Diffuse ("Diffuse", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
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

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}