using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all the values for the settings of the color for the planet and its biomes
/// </summary>
[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public BiomeColorSettings biomeColorSettings;
    public Gradient oceanColor;

    /// <summary>
    /// subclass that holds values specific to each biome of the planet
    /// </summary>
    [System.Serializable]
    public class BiomeColorSettings
    {
        public Biome[] biomes;
        public NoiseSettings noiseSettings;
        public float noiseOffset;
        public float noiseStrength;
        [Range(0f, 1f)]
        public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;
            public Color tint;
            [Range(0f, 1f)]
            public float startHeight;
            [Range (0f, 1f)]
            public float tintPercent;
        }
    }

}
