Shader "Hidden/PointillismStipplingImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile COLORED GRAYSCALE
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 scrPos : TEXCOORD0;
				float2 uv : TEXCOORD1;
			};

			sampler2D _MainTex;

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.scrPos = ComputeScreenPos(o.pos);
				o.uv = v.uv;
				return o;
			}

			uniform int _BlurSize;

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.scrPos.xy/i.scrPos.w;
				float3 col = tex2D(_MainTex, uv).rgb;
			
				float2 qq = uv;
				qq = frac(qq * (float2(314.159, 314.265)));
				qq += dot(qq, qq.yx + 17.17);
				qq = frac((qq.xx + qq.yx) * qq.xy);

				float2 r = qq;
				r.x *= UNITY_TWO_PI;
				float2 cr = float2(sin(r.x),cos(r.x))*sqrt(r.y);
				
				float3 blurred = tex2D(_MainTex, uv + cr * (float2(_BlurSize,_BlurSize) / _ScreenParams.xy) ).rgb;
				
#if defined(COLORED)
				return fixed4(blurred, 1.0);
#else
				float greyScaleValue = dot(blurred, float3(0.333, 0.333, 0.333));
				float3 res = float3(greyScaleValue,greyScaleValue,greyScaleValue);

				return fixed4(res, 1.0);
#endif
			}

			ENDCG
		}
	}
}
