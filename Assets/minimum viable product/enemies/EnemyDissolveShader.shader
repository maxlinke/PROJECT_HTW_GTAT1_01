Shader "Custom/EnemyDissolveShader" {

	Properties {
		[PerRendererData] _Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Dissolve Texture", 2D) = "white" {}
		[PerRendererData] _Dissolve ("Dissolve Value", Range(0,1)) = 0.5
	}

	SubShader {

		Tags { "Queue"="AlphaTest" "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Dissolve

		sampler2D _MainTex;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color;
			o.Alpha = (tex2D(_MainTex, IN.uv_MainTex).r / 1.01);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
