// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Studies/MiddleOut"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _SampleTex("Sample Tex", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Cutoff("Cut off",Range(0,1)) = .5
        [MaterialToggle] _Distort("Distort",Float) = 0
        _Fade("Fade", Range(0,1)) = 0
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
                float2 uv1 : TEXCOORD0;
            };

            float2 _MainTex_TexelSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv1 = v.uv;

                #if UNITY_UV_STARTS_AT_TOP
                    if(_MainTex_TexelSize.y<0)
                    {
                        o.uv1.y=1 - o.uv1.y;
                    }
                #endif

                return o;
            }
            sampler2D _MainTex;
            sampler2D _SampleTex;
            float4 _Color;
            float _Cutoff;
            float _Fade;
            bool _Distort;

            float4 frag(v2f i) : SV_Target
            {              
                fixed4 transit = tex2D(_SampleTex, i.uv1);

                fixed2 direction=float2(0,0);
                if(_Distort)
                {
                    //x b=and y offset based on red (x) and green(y) values of the texture
                    direction = normalize(float2((transit.r - 0.5) * 2, (transit.g - 0.5) * 2));
                }

                
                fixed4 col = tex2D(_MainTex, i.uv + _Cutoff * direction);

                //change the corresponding screen coordinates based on the textures blue value. 
                
                if(transit.b < _Cutoff)
                    return col = lerp(col, _Color,_Fade);
                
                return col;
            };
            ENDCG
        }
    }
}
