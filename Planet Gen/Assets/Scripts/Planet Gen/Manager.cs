using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject planet;
    public void SpawnTerrainPlanet()
    {
        GameObject spawnedPlanet = Instantiate(planet, transform.position, transform.rotation);
        spawnedPlanet.GetComponent<Planet>().GeneratePlanet();
    }
}
