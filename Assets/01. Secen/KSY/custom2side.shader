Shader "Custom/custom2side" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		cull off
		
		CGPROGRAM
		#pragma surface surf CustomLight

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		
		float4 LightingCustomLight (SurfaceOutput s, float3 lightDir , float atten)
		{
			float NdotL = saturate(dot(s.Normal, lightDir))*0.5+0.5;
			float4 finalcolor;
			finalcolor.rgb = NdotL*s.Albedo*_LightColor0.rgb*atten;
			return finalcolor;
		}
		
		ENDCG
	} 
	FallBack "Diffuse"
}
