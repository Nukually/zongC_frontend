Shader "Custom/TrailEffectShader"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _GlowStrength ("Glow Strength", Range(0, 1)) = 0.5
        _NoiseScale ("Noise Scale", Range(0.1, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _GlowStrength;
            float _NoiseScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 这里可以添加更复杂的特效计算，比如添加噪声等效果
                float noiseValue = tex2D(_MainTex, i.uv * _NoiseScale).r;
                fixed4 col = _Color;
                col.a *= (1 + noiseValue * _GlowStrength);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}