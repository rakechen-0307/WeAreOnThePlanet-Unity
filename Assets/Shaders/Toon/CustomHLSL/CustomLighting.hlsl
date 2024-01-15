#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#pragma multi_compile _ _ADDITIONAL_LIGHTS
#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS

/*
void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float DistanceAtten, out float ShadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
	float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;

	#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
		ShadowAtten = 1.0h;
	#else
        ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
        float shadowStrength = GetMainLightShadowStrength();
        ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
        sampler_MainLightShadowmapTexture),
        shadowSamplingData, shadowStrength, false);
    #endif
#endif
}
*/

/*
void DirectSpecular_float(float3 Specular, float Smoothness, float3 Direction, float3 Color, float3 WorldNormal, float3 WorldView, out float3 Out)
{
#ifdef SHADERGRAPH_PREVIEW
    Out = 0;
#else
    Smoothness = exp2(10 * Smoothness + 1);
    WorldNormal = normalize(WorldNormal);
    WorldView = SafeNormalize(WorldView);
    Out = LightingSpecular(Color, Direction, WorldNormal, WorldView, float4(Specular, 0), Smoothness);
#endif
}
*/

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
        Light light = GetAdditionalLight(i, WorldPosition, 0);
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        float3 color = LightingLambert(attenuatedLightColor, light.direction, WorldNormal);
        
        color = Grad(gradient, color);
        
        float fresnelDir = dot((-1) * normalize(WorldNormal) + normalize(WorldView), light.direction);
        float remapFresnelDir = ((fresnelDir - (-1)) / 2) * (FresnelAngle - 1) + 1;
        float4 fresnelEffect = step(0.1, (FresnelEffect * remapFresnelDir)) * FresnelIntensity;
        
        // get maximum
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
        specularColor += LightingSpecular(attenuatedLightColor, light.direction, WorldNormal, WorldView, float4(SpecColor, 0), Smoothness);
    }
#endif

    Diffuse = diffuseColor;
    Specular = specularColor;
}

#endif