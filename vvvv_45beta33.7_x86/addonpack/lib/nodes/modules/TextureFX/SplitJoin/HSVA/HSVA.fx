float2 R;
texture tex0,tex1,tex2,tex3;
sampler s0=sampler_state{Texture=(tex0);MipFilter=LINEAR;MinFilter=LINEAR;MagFilter=LINEAR;};
sampler s1=sampler_state{Texture=(tex1);MipFilter=LINEAR;MinFilter=LINEAR;MagFilter=LINEAR;};
sampler s2=sampler_state{Texture=(tex2);MipFilter=LINEAR;MinFilter=LINEAR;MagFilter=LINEAR;};
sampler s3=sampler_state{Texture=(tex3);MipFilter=LINEAR;MinFilter=LINEAR;MagFilter=LINEAR;};
struct col4{float4 c0:COLOR0;float4 c1:COLOR1;float4 c2:COLOR2;float4 c3:COLOR3;};
#include "ColorSpace.fxh"
col4 hsvSplit(float2 x:TEXCOORD0):color{
    float4 c=tex2D(s0,x);
    c.rgb=RGBtoHSV(c.rgb);
    col4 HSVA=(col4)c.a;
    HSVA.c0.rgb=c.r;
    HSVA.c1.rgb=c.g;
    HSVA.c2.rgb=c.b;
    HSVA.c3.rgb=c.a;
    return HSVA;
}
float4 hsvJoin(float2 x:TEXCOORD0):color{
    float4 c0=tex2D(s0,x);
    float4 c1=tex2D(s1,x);
    float4 c2=tex2D(s2,x);
    float4 c3=tex2D(s3,x);
    float4 c=float4(c0.r,c1.r,c2.r,sqrt(c3.a)*sqrt(c3.r));
    float3 alp=float3(c0.a,c1.a,c2.a);
    c.rgb=lerp(float3(1,1,1),c.rgb,alp<c.a?alp/c.a:1);
    c.rgb=HSVtoRGB(c.rgb);
    return c;
}
void vs2d(inout float4 vp:POSITION0,inout float2 uv:TEXCOORD0){vp.xy*=2;uv+=.5/R;}
technique HSVASplit{pass pp0{vertexshader=compile vs_3_0 vs2d();pixelshader=compile ps_3_0 hsvSplit();}}
technique HSVAJoin{pass pp0{vertexshader=compile vs_3_0 vs2d();pixelshader=compile ps_3_0 hsvJoin();}}
