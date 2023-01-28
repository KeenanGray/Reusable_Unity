// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Studies/HorizontalScreenWipe"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Cutoff("Cut off",Range(-1,1)) = .5
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
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
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _Color;
            float _Cutoff;

            float4 frag(v2f i) : SV_Target
            {
                if(_Cutoff<0)
                {
                    if(i.uv.x > -_Cutoff)
                    {
                        return _Color;
                    }
                    else
                    {
                        return tex2D(_MainTex,i.uv);
                    }
                }
                else
                {
                    if(i.uv.x < _Cutoff)
                    {
                        return _Color;
                    }
                    else
                    {
                        return tex2D(_MainTex,i.uv);
                    }
                }
            };
            ENDCG
        }
    }
}
