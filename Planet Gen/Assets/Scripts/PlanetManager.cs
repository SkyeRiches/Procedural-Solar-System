using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField]
    private GameObject planet;
    public void CreatePlanet()
    {
        GameObject planetInstance = Instantiate(planet, transform.position, transform.rotation);
        planetInstance.GetComponent<Planet>().GeneratePlanet();
    }
}