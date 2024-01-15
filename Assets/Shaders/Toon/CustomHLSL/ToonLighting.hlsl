#ifndef TOON_LIGHTING_INCLUDED
#define TOON_LIGHTING_INCLUDED

// Enable Shadows
#pragma multi_compile _ _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS

// Sample Gradient
float4 Grad(Gradient gradient, float Time)
{
    float3 color = gradient.colors[0].rgb;
    [unroll]
    for (int c = 1; c < 8; c++)
    {
        float colorPos = saturate((Time - gradient.colors[c - 1].w) / (gradient.colors[c].w - gradient.colors[c - 1].w)) * step(c, gradient.colorsLength - 1);
        color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
    }
    return float4(color, 1);
}
// Lighting Calculation
void AdditionalLights_float(float3 SpecColor, float Smoothness, float3 WorldPosition, float3 WorldNormal, 
    float3 WorldView, Gradient gradient, float4 FresnelIntensity, float FresnelEffect, float FresnelAngle,
    out float3 Diffuse, out float3 Specular)
{
    float3 diffuseColor = 0;
    float3 specularColor = 0;

#if !defined(SHADERGRAPH_PREVIEW)
    Smoothness = exp2(10 * Smoothness + 1);
    WorldNormal = normalize(WorldNormal);
    WorldView = SafeNormalize(WorldView);
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        // diffusion
        Light light = GetAdditionalLight(i, WorldPosition, 0);
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        float3 color = LightingLambert(attenuatedLightColor, light.direction, WorldNormal); // diffusion
        color = Grad(gradient, color); // take gradient
        
        // fresnel effect
        float fresnelDir = dot((-1) * normalize(WorldNormal) + normalize(WorldView), light.direction);
        float remapFresnelDir = ((fresnelDir - (-1)) / 2) * (FresnelAngle - 1) + 1;
        float4 fresnelEffect = step(0.1, (FresnelEffect * remapFresnelDir)) * FresnelIntensity;
        
        // blend color by taking maximum
        for (int i = 0; i < 3; i++)
        {
            if (fresnelEffect[i] > color[i])
            {
                color[i] = fresnelEffect[i];
            }
        }
        color *= light.color;
        
        for (int j = 0; j < 3; j++)
        {
            if (color[j] > diffuseColor[j])
            {
                diffuseColor[j] = color[j];
            }
        }
        
        // specular
        specularColor += LightingSpecular(attenuatedLightColor, light.direction, WorldNormal, WorldView, float4(SpecColor, 0), Smoothness);
    }
#endif

    Diffuse = diffuseColor;
    Specular = specularColor;
}

#endif