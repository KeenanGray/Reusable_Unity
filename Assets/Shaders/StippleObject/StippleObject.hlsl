TEXTURE2D(_CameraColorTexture);
SAMPLER(sampler_CameraColorTexture);
float4 _CameraColorTexture_TexelSize;

//TEXTURE2D(_CameraDepthTexture);
//SAMPLER(sampler_CameraDepthTexture);

TEXTURE2D(_CameraDepthNormalsTexture);
SAMPLER(sampler_CameraDepthNormalsTexture);

float3 DecodeNormal(float4 enc)
{
    float kScale = 1.7777;
    float3 nn = enc.xyz*float3(2*kScale,2*kScale,0) + float3(-kScale,-kScale,1);
    float g = 2.0 / dot(nn.xyz,nn.xyz);
    float3 n;
    n.xy = g*nn.xy;
    n.z = g-1;
    return n;
}

void StippleObject_float(float2 UV, float BlurSize, float depth, out float2 Out)
{
    float2 uv = UV.xy;
    
    float2 qq = uv;
    qq = frac(qq * (float2(314.159, 314.265)));
    qq += dot(qq, qq.yx + 17.17);
    qq = frac((qq.xx + qq.yx) * qq.xy);

    float2 r = qq; 
    r.x *= 6.28318530718;//TWO_PI
    float2 cr = float2(sin(r.x),cos(r.x))*sqrt(r.y);

/*
    float freq = 10;
    float amp = 5;
    BlurSize = 2+(4*depth);
    BlurSize =  5 + sin(freq+(_Time.y)) ;
*/
    float2 uv2 = uv + cr * (float2(BlurSize,BlurSize) /_ScreenParams );

    Out =  uv2;
}

