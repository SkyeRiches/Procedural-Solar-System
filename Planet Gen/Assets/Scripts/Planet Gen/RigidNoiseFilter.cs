using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to create a peak and ridge effect on the terrain to allow for mountainous like terrain
/// </summary>
public class RigidNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.RigidNoiseSettings settings;

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0f;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            // invert the absolue value of the point
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            // makes ridges more pronounced
            v *= v;
            // weight the noise to be heavier on the ridges so there is more noise detail on them
            v *= weight;
            // clamp the weight between 0 and 1
            weight = Mathf.Clamp01(v * settings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistence;
        }
        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}
