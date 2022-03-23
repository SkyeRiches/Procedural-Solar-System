using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stores the values to be used to adjust the noise on the planet in order to be able to modify how the terrain looks
/// </summary>
[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Rigid};
    public FilterType filterType;

    [ConditionalHideAttributes("filterType", 0)]
    public SimpleNoiseSettings simpleNoiseSettings;
    [ConditionalHideAttributes("filterType", 1)]
    public RigidNoiseSettings rigidNoiseSettings;

    [System.Serializable]
    public class SimpleNoiseSettings
    {
        public float strength = 1;
        [Range(1, 8)]
        public int numLayers = 1;
        public float baseRoughness = 1;
        public float roughness = 2;
        public float persistence = 0.5f;
        public Vector3 centre;
        public float minValue;
    }
    [System.Serializable]
    public class RigidNoiseSettings : SimpleNoiseSettings 
    {
        public float weightMultiplier = 0.8f;
    }
}
