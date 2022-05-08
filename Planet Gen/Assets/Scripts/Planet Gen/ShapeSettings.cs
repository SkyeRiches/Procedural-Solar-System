using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates custom editor settings for the designer to utilise
/// </summary>
[System.Serializable]
public class ShapeSettings : MonoBehaviour
{
    public float planetRadius = 1; // Allows the designer to change the radius of the planet
    public NoiseLayer[] noiseLayers; // Allows user to add or remove noise layers to an array that is applied to the planet

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true; // Set whether layer is enabled or not
        public bool useFirstLayerAsMask; // This setting means that this layer will use the first layer as a basis for its settings meaning they blend better and there is less drastic changes
        public NoiseSettings noiseSettings; // Creates ui section that will display all the noise settings seen in NoiseSettings.cs
    }
}
