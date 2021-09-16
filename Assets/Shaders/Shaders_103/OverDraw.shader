// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shaders103/OverDraw"
{
    Properties
    {
    }

   
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
        }
        
        ZTest Always
        ZWrite Off
        Blend One One

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
                return o;
            }

            half4 _OverDrawColor;

            float4 frag(v2f i) : SV_Target
            {
                return _OverDrawColor;
            };
            ENDCG
        }
    }
}
