using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanet : MonoBehaviour
{
    [SerializeField] private GameObject planet;

    public void SpawnCelestialBody()
    {
        Instantiate(planet, transform.position, transform.rotation);
    }
}
