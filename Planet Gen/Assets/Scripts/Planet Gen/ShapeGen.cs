using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGen
{
    ShapeSettings shapeSettings;
    INoiseFilter[] noiseFilters;
    public MinMax elevationMinMax;

    public void UpdateSettings(ShapeSettings settings)
    {
        this.shapeSettings = settings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        elevationMinMax = new MinMax();
    }

    /// <summary>
    /// calculates the value of the point on the sphere without scaling it to the planets size, this is the raw elevation value
    /// </summary>
    public float CalculateUnscaledElevation(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            // adjusts the first layer's value to be between 0 and 1
            firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);

            if (shapeSettings.noiseLayers[0].enabled)
            {
                // if the first layer is enabled then that is the value of elevation
                elevation = firstLayerValue;
            }
        }

        for (int i = 1;i < noiseFilters.Length; i++)
        {
            if (shapeSettings.noiseLayers[i].enabled)
            {
                // if using the first layer as a mask then that is the modifier for the elevation, otherwise it is 1
                float mask = (shapeSettings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
                // increment the elevation value by the points value between 0 and 1 and multiply it by the mask value
                elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        elevationMinMax.AddValue(elevation);
        return elevation;
    }

    public float GetScaledElevation(float unscaledElevation)
    {
        // set the elevation to be the largest value on the planet
        float elevation = Mathf.Max(0, unscaledElevation);
        // scale the elevation by the size of the planet
        elevation = shapeSettings.planetRadius * (1 + elevation);
        return elevation;
    }
}
