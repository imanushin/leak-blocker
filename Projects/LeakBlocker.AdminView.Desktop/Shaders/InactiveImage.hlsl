sampler2D implicitInput : register(s0);

float4 main(float2 uv : TEXCOORD) : COLOR 
{
    /*float4 color = tex2D(implicitInput,uv);

    float4 result = color;

    result.rgb -= tex2D(implicitInput, uv - .005).rbg;
    result.rgb += 0.5;
    
    float grayscale = (result.r + result.g + result.b) / 2.0;

    grayscale *= color.a;

    return float4(grayscale, grayscale, grayscale, color.a);*/

    
    float4 color = tex2D(implicitInput, uv);
    float grayscale = 0.21 * color.r + 0.71 * color.g + 0.07 * color.b;
    
    grayscale = log(grayscale * 3 + 1.0) * 0.7;

    grayscale *= color.a;

    return float4(grayscale, grayscale, grayscale, color.a);
}