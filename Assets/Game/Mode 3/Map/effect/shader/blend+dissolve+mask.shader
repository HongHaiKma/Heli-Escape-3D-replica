Shader "VFX/blend" {
	Properties {
		_GradientTex ("GradientTex", 2D) = "white" {}
		[Toggle(_GRADIENTUV2_ON)] _GradientUV2 ("GradientUV2", Float) = 0
		_GradientTexU ("GradientTexU", Float) = 0
		_GradientTexV ("GradientTexV", Float) = 0
		[Toggle] _DistortionInfluenceGradient ("DistortionInfluenceGradient", Float) = 0
		[Toggle] _DepthFade ("DepthFade", Float) = 0
		_DepthFadeIndensity ("DepthFadeIndensity", Float) = 1
		_MainPower ("MainPower", Float) = 1
		_OpacityPower ("OpacityPower", Float) = 1
		[HDR] _Color ("Color", Vector) = (1,1,1,1)
		_Indensity ("Indensity", Float) = 1
		_Opacity ("Opacity", Float) = 1
		_DistortionTex ("DistortionTex", 2D) = "white" {}
		[Toggle] _Distortion2UV ("Distortion2UV", Float) = 0
		_DistortionU ("DistortionU", Float) = 0
		_DistortionV ("DistortionV", Float) = 0
		_DistortionIndensity ("DistortionIndensity", Float) = 0
		_MainTex ("MainTex", 2D) = "white" {}
		[Toggle(_MAINTEXUV2_ON)] _MainTexUV2 ("MainTexUV2", Float) = 0
		_MainU ("MainU", Float) = 0
		_MainV ("MainV", Float) = 0
		[Toggle(_DESATURATE_ON)] _Desaturate ("Desaturate", Float) = 0
		_AlphaTex ("AlphaTex", 2D) = "white" {}
		[Toggle] _AlphaTexUV2 ("AlphaTexUV2", Float) = 0
		_AlphaU ("AlphaU", Float) = 0
		_AlphaV ("AlphaV", Float) = 0
		_AlphaTex2 ("AlphaTex2", 2D) = "white" {}
		[Toggle] _AlphaTex2UV2 ("AlphaTex2UV2", Float) = 0
		_Alpha2U ("Alpha2U", Float) = 0
		_Alpha2V ("Alpha2V", Float) = 0
		[Toggle] _SoftDissolveSwitch ("SoftDissolveSwitch", Float) = 0
		[Toggle] _DistortionInfluenceSoft ("DistortionInfluenceSoft", Float) = 0
		_SoftDissolveTex ("SoftDissolveTex", 2D) = "white" {}
		[Toggle(_DISSOLVEUV2_ON)] _DissolveUV2 ("DissolveUV2", Float) = 0
		[Toggle] _VertexColorInfluenceSoftDissolve ("VertexColorInfluenceSoftDissolve", Float) = 0
		[Toggle] _CustomDataUV2XInfluenceSoftDissolve ("CustomDataUV2XInfluenceSoftDissolve", Float) = 0
		_CustomDataUV2X_Indensity ("CustomDataUV2X_Indensity", Float) = 1
		_DissolveTexPlusValue ("DissolveTexPlusValue", Float) = 0
		_SoftDissolveTexU ("SoftDissolveTexU", Float) = 0
		_SoftDissolveTexV ("SoftDissolveTexV", Float) = 0
		_SoftDissolveIndensity ("SoftDissolveIndensity", Range(0, 1.05)) = 1.05
		_SoftDissolveSoft ("SoftDissolveSoft", Float) = 0
		_LineRange ("LineRange", Float) = 0.5
		_LineWidth ("LineWidth", Float) = 0.1
		[HDR] _LineColor ("LineColor", Vector) = (1,1,1,1)
		_LineIndensity ("LineIndensity", Float) = 1
		[Toggle] _DistortionInfluenceOffset ("DistortionInfluenceOffset", Float) = 0
		_VertexOffsetTex ("VertexOffsetTex", 2D) = "white" {}
		_VertexOffsetTexU ("VertexOffsetTexU", Float) = 0
		_VertexOffsetTexV ("VertexOffsetTexV", Float) = 0
		_VertexOffsetIndensity ("VertexOffsetIndensity", Float) = 0
		[HideInInspector] _tex4coord2 ("", 2D) = "white" {}
		[HideInInspector] _texcoord2 ("", 2D) = "white" {}
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] _tex4coord ("", 2D) = "white" {}
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