Shader "Custom/Scrolling"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _ScrollX ("Scroll Speed X", Float) = 0.1
        _ScrollY ("Scroll Speed Y", Float) = 0.1
        _Color ("Color", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0,1)) = 1.0
        _EmissionTex ("Emission Texture", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(1)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollX;
            float _ScrollY;
            float4 _Color;
            float _Transparency;

            sampler2D _EmissionTex;
            float4 _EmissionColor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float2 scrollUV = v.uv + float2(_ScrollX * _Time.y, _ScrollY * _Time.y);
                o.uv = TRANSFORM_TEX(scrollUV, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 tex = tex2D(_MainTex, i.uv);
                tex *= _Color;
                tex.a *= _Transparency;

                half4 emission = tex2D(_EmissionTex, i.uv) * _EmissionColor;
                tex.rgb += emission.rgb;

                UNITY_APPLY_FOG(i.fogCoord, tex);
                return tex;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/Diffuse"
}

