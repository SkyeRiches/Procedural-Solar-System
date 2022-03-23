using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGen
{
    ColorSettings settings;
    Texture2D texture;
    const int textureRes = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings)
    {
        this.settings = settings;
        // if number or biomes changes or there is no texture then a new 2d texture will be created using the resolution for size and the number of biomes as rows in the texture
        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length)
        {
            texture = new Texture2D(textureRes * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
        // create a noise filter for the biomes
        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noiseSettings);
    }

    // update the elevation if it has changed, this will then affect the shader as it changes color based on elevation
    public void UpdateElevation(MinMax elevationMinMax)
    {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    /// <summary>
    /// returns a value between 0 and 1 depending on what biome the point is in
    /// 0 is the first biome and 1 is the last
    /// </summary>
    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + 0.001f;

        for (int i = 0; i < numBiomes; i++)
        {
            float distance = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public void UpdateColors()
    {
        // create new color array the size of the texture
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;

        foreach (var biome in settings.biomeColorSettings.biomes)
        {
            for (int i = 0; i < textureRes * 2; i++)
            {
                Color gradientColor;
                // if the value of i is less than texture res, calculate an ocean color for it based on its value between 0 and 1
                if (i < textureRes)
                {
                    gradientColor = settings.oceanColor.Evaluate(i / (textureRes - 1f));
                }
                // else calculate the color of it for the land biome
                else
                {
                    gradientColor = biome.gradient.Evaluate((i - textureRes) / (textureRes - 1f));
                }
                // adds a tint to the biome color
                Color tintColor = biome.tint;
                // calculate the final color value based on the gradient and tint
                colors[colorIndex] = gradientColor * (1-biome.tintPercent) + tintColor * biome.tintPercent;
                colorIndex++;
            }
        }
        
        // Apply the texture to the material of the planet
        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
