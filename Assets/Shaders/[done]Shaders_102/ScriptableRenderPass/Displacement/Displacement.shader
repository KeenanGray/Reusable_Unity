// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders102/Displacement"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _DisplaceTex("Displacement Texture", 2D) = "white" {}
        _Magnitude("Magnitude",Range(0.0,0.1)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha

        HLSLINCLUDE
        #pragma vertex vert
        #pragma fragment frag

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        struct Attributes
        {
            float4 vertex: POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 vertex: SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        TEXTURE2D(_MainTex);
        float4 _Color;
        TEXTURE2D(_DisplaceTex);
        float _Magnitude;
        
        SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        float4 _MainTex_ST;

         SAMPLER(sampler_DisplaceTex);
        float4 _DisplaceTex_TexelSize;
        float4 _DisplaceTex_ST;
        
        Varyings vert(Attributes IN)
        {
            Varyings OUT;
            OUT.vertex = TransformObjectToHClip(IN.vertex.xyz);
            OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
            return OUT;
        }

        ENDHLSL

        Pass
        {
            Name "DISPLACEMENT"

            HLSLPROGRAM

            half4 frag(Varyings i) : SV_Target
            {
                //create a value over time to allow for animation
                float2 distuv = float2(i.uv.x + _Time.x * 2, i.uv.y + _Time.x * 2);

                //sample from displacement texture
                float2 disp = SAMPLE_TEXTURE2D(_DisplaceTex, sampler_DisplaceTex, distuv).xy;
                disp = ((disp*2)-1)* _Magnitude;

                float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + disp);
                return color;
            };
            ENDHLSL
        }
    }
}
