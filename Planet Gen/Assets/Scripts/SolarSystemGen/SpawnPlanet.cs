using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanet : MonoBehaviour
{
    [SerializeField] private GameObject planet;
    private GameObject spawnedPlanet;
    [SerializeField] private GameObject sun;

    private List<Vector3> planetPositions = new List<Vector3>();

    private float planetDiameter;
    private float planetMass;
    private float planetDistance;
    private float planetSpin;
    private Vector3 planetPos;

    private void Start()
    {
        var star = Instantiate(sun, transform.position, transform.rotation);
        gameObject.GetComponent<PlanetManager>().SetSun(star);
        gameObject.GetComponent<OrbitPaths>().Sun = star;
    }

    public void SpawnCelestialBody()
    {
        planetDiameter = Random.Range(1, StarInfo.starDiameter / 2);

        planetSpin = Random.Range(1, 360);

        planetMass = Random.Range(0.05f, 500f); // Values chosen based on a scale model made using our solar system, these values work well for the physics model. will be changed later

        PlanetPosition();

        if (planetPositions.Count > 0)
        {
            foreach (Vector3 pos in planetPositions)
            {
                if (Vector3.Distance(planetPos, pos) <= planetDiameter)
                {
                    PlanetPosition();
                }
            }
        }

        planetPositions.Add(planetPos);
        spawnedPlanet = Instantiate(planet, planetPos, transform.rotation);
        SetPlanet();
        transform.GetComponent<PlanetManager>().NewPlanetVel(spawnedPlanet);

        gameObject.GetComponent<OrbitPaths>().SimulateTrajectory(spawnedPlanet, spawnedPlanet.transform.position);


    }

    void SetPlanet()
    {

        spawnedPlanet.transform.localScale *= planetDiameter;

        spawnedPlanet.transform.GetComponent<Rigidbody>().mass = planetMass;

        spawnedPlanet.transform.GetComponent<PlanetSpin>().spinRate = planetSpin;
    }

    void PlanetPosition()
    {
        planetDistance = Random.Range(StarInfo.starDiameter / 2, 5000);
        planetPos = new Vector3(planetDistance, 0, 0);
    }
}
