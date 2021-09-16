// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders103/ShowDepth"
{
    Properties
    {
        _Color("Color",Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float2 uv : TEXCOORD0;
                float depth : DEPTH;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = -UnityObjectToViewPos(  float3(v.vertex.x,v.vertex.y,v.vertex.z)  ).z * _ProjectionParams.w;
                return o;
            }

            float4 _Color;
            float4 frag(v2f i) : SV_Target
            {
                float inv = 1 - i.depth;
                return fixed4(inv,inv,inv,1) * _Color;
            };
            ENDCG
        }
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
        }
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex: POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex: SV_POSITION;
                float2 uv : TEXCOORD0;
                float depth : DEPTH;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = -UnityObjectToViewPos(  float3(v.vertex.x,v.vertex.y,v.vertex.z)  ).z * _ProjectionParams.w;
                return o;
            }

            float4 _Color;
            float4 frag(v2f i) : SV_Target
            {
                return _Color;
            };
            ENDCG
        }
    }
}
