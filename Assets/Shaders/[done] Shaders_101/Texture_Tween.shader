// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders101/Texture_Tween"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Texture2("Texture",2D) = "black" {}
        _Color("Color", Color) = (1,1,1,1)
        _Tween("Range", Range(0,1)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
        }
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

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
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _Texture2;
            float4 _Color;
            float _Tween;

            float4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv) * _Tween *_Color +  tex2D(_Texture2, i.uv) * (1 - _Tween) *_Color;
                return color;
            };
            ENDCG
        }
    }
}
