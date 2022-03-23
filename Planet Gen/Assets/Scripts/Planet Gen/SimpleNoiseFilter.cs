using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    Noise noise = new Noise();
    NoiseSettings.SimpleNoiseSettings settings;

    // constructor for the class
    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0f;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numLayers; i++)
        {
            // adjusts the value to be between 0 and 1
            float v = noise.Evaluate(point * frequency + settings.centre);
            // increase the noise value by the amplitude 
            noiseValue += (v + 1) * 0.5f * amplitude;
            // Frequency will increase with each layer when roughness is above 1
            frequency *= settings.roughness;
            // Amplitude will decrease with each layer when persistence is above 1
            amplitude *= settings.persistence;
        }
        // decreases the noise value by the min value so that the noise adjsutments made are an addition to the minimum and not inclusive of it
        noiseValue = noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}
