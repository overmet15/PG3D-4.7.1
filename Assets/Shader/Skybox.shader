Shader "RenderFX/Skybox" {
Properties {
 _Tint ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
 _FrontTex ("Front (+Z)", 2D) = "white" {}
 _BackTex ("Back (-Z)", 2D) = "white" {}
 _LeftTex ("Left (+X)", 2D) = "white" {}
 _RightTex ("Right (-X)", 2D) = "white" {}
 _UpTex ("Up (+Y)", 2D) = "white" {}
 _DownTex ("down (-Y)", 2D) = "white" {}
}
	//DummyShaderTextExporter
	
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
}