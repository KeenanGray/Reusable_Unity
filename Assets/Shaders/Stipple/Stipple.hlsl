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

void Stipple_float(float2 UV, float4 ScreenPosition, float BlurSize, float depth, out float4 Out)
{
    float halfScaleFloor = floor(1 * 0.5);
    float halfScaleCeil = ceil(1 * 0.5);
    float2 Texel = (1.0) / float2(_CameraColorTexture_TexelSize.z, _CameraColorTexture_TexelSize.w);

    float2 uvSamples[4];

    uvSamples[0] = UV - float2(Texel.x, Texel.y) * halfScaleFloor;
    uvSamples[1] = UV + float2(Texel.x, Texel.y) * halfScaleCeil;
    uvSamples[2] = UV + float2(Texel.x * halfScaleCeil, -Texel.y * halfScaleFloor);
    uvSamples[3] = UV + float2(-Texel.x * halfScaleFloor, Texel.y * halfScaleCeil);

    float2 uv = ScreenPosition.xy;
    
    float2 qq = uv;
    qq = frac(qq * (float2(314.159, 314.265)));
    qq += dot(qq, qq.yx + 17.17);
    qq = frac((qq.xx + qq.yx) * qq.xy);

    float2 r = qq; 
    r.x *= 6.28318530718;//TWO_PI
    float2 cr = float2(sin(r.x),cos(r.x))*sqrt(r.y);

    float freq = depth * 10;
    float amp = 5;
    BlurSize = BlurSize * sin(freq+(_Time.y)) ;

    uvSamples[1] = uv + cr * (float2(BlurSize,BlurSize) /_ScreenParams );

    float4 original = SAMPLE_TEXTURE2D(_CameraColorTexture, sampler_CameraColorTexture, uvSamples[1]);	

    float3 blurred = original;

    float greyScaleValue = dot(blurred, float3(0.333, 0.333, 0.333));

    float4 grey = float4(greyScaleValue,greyScaleValue,greyScaleValue,1.0);
    float4 blurry = float4(blurred.x,blurred.y,blurred.z,1);
    Out =  blurry;
    //Out = float4(depth,depth,depth,1);
}
