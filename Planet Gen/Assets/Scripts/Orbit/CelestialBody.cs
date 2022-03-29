using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public Rigidbody rb;
    /// <summary>
    /// When an object with this script is enabled, it will be added to the list of celestial bodies in the scene
    /// </summary>
    private void OnEnable()
    {
        CelestialList.celestialBodies.Add(this);
    }

    /// <summary>
    /// Upon disabling the object with this script, it will be rmoved from the celestial bodies list 
    /// so that no other celestial bodies are trying to look for an object that doesnt exist as far as the physics is concerned
    /// </summary>
    private void OnDisable()
    {
        CelestialList.celestialBodies.Remove(this);
    }
}
