using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlanet : MonoBehaviour
{
    #region VARIABLES
    [SerializeField] private GameObject planet;
    private GameObject spawnedPlanet;

    public List<GameObject> planets = new List<GameObject>();

    private float planetDiameter;
    private float planetDistance;
    [SerializeField] private float maxDist = 100;
    private float planetSpin;
    private Vector3 planetPos;
    private float orbitSpeed;

    [SerializeField] private Shader planetShader;

    [SerializeField] private SelectPlanet selectPScript;
    #endregion

    /// <summary>
    /// This function spawns a new planet and randomises its attributes
    /// </summary>
    public void SpawnCelestialBody()
    {
        // Randomise planet size and orbit
        planetDiameter = Random.Range(0.1f, 5);

        planetSpin = Random.Range(1, 360);

        orbitSpeed = Random.Range(1, 50);

        PlanetPosition();

        // Create the new planet at the randomised distance and apply the random attributes to it
        spawnedPlanet = Instantiate(planet, Vector3.zero, transform.rotation);
        spawnedPlanet.GetComponent<ShapeSettings>().planetRadius = planetDiameter;
        spawnedPlanet.GetComponent<SphereCollider>().radius = planetDiameter;
        spawnedPlanet.transform.GetComponent<PlanetSpin>().spinRate = planetSpin;

        // Call the functions to randomise the planet terrain and set the termperature color tint
        PlanetColors();
        PlanetTerrain();

        // Generate the planet mesh with the new random values and reset the position back to where it was originally spawned
        // This was needed as generating the planet resets its position
        spawnedPlanet.GetComponent<Planet>().GeneratePlanet();
        spawnedPlanet.transform.position = planetPos;
        // Set the speed of the orbit
        spawnedPlanet.GetComponent<PlanetMovement>().m_Speed = orbitSpeed;

        // Generate the line render that shows the orbit path of the celestial body.
        gameObject.GetComponent<OrbitLine>().SimulateTrajectory(spawnedPlanet, spawnedPlanet.transform.position, orbitSpeed);

        // if the other planets orbits are paused then pause the new planet once spawned
        if (selectPScript.paused)
        {
            spawnedPlanet.GetComponent<PlanetMovement>().enabled = false;
        }
    }

    /// <summary>
    /// Generates a random position from the star in the scene that the planet will spawn at
    /// </summary>
    void PlanetPosition()
    {
        planetDistance = Random.Range(StarInfo.starDiameter * 2, maxDist);
        planetPos = new Vector3(planetDistance, 0, 0);
    }

    /// <summary>
    /// Calculates the percentage of how close the planet is to the sun,
    /// the closer it is, the higher the tint level of red
    /// the further it is, the higher the tint level of blue
    /// This is to visualise temperature of the planet
    /// </summary>
    void PlanetColors()
    {
        ColorSettings planetCol = spawnedPlanet.GetComponent<ColorSettings>();
        planetCol.planetMaterial = new Material(planetShader);

        for (int i = 0; i < planetCol.biomeColorSettings.biomes.Length; i++)
        {
            // if the planet is close to the star then its biome tints will be red as it will be hotter
            if (planetDistance < maxDist / 2)
            {
                planetCol.biomeColorSettings.biomes[i].tint = Color.red;
                planetCol.biomeColorSettings.biomes[i].tintPercent = 1 - (planetDistance / (maxDist / 2));
            }
            // if the planet is far from the star, its biome tints will have a cyan tint to show its colder as its further from the heat source
            if (planetDistance >= maxDist / 2)
            {
                planetCol.biomeColorSettings.biomes[i].tint = Color.cyan;
                planetCol.biomeColorSettings.biomes[i].tintPercent = (planetDistance / (maxDist / 2)) - 1;
            }
        }
    }

    void PlanetTerrain()
    {
        ShapeSettings terrainSettings = spawnedPlanet.GetComponent<ShapeSettings>();

        // Loop through the noise layers on the planet
        for (int i = 0; i < terrainSettings.noiseLayers.Length; i++)
        {
            if (terrainSettings.noiseLayers[i].noiseSettings.filterType == NoiseSettings.FilterType.Simple)
            {
                // Generate random values for all of the terrain settings of the planet so that it has a unique appearance.
                // The values have been chosen as comfortable values for each setting
                // Values outside these ranges tend to have alot less realistic terrain
                // The user can make it go beyond these values but for initial spawning of the planet, realistic terrain is chosen
                NoiseSettings.SimpleNoiseSettings noiseSettingSimple = terrainSettings.noiseLayers[i].noiseSettings.simpleNoiseSettings;
                noiseSettingSimple.strength = Random.Range(0.01f, 0.2f);
                noiseSettingSimple.baseRoughness = Random.Range(0.4f, 2f);
                noiseSettingSimple.roughness = Random.Range(0.5f, 5f);
                noiseSettingSimple.persistence = Random.Range(0.1f, 0.5f);
                noiseSettingSimple.centre.x = Random.Range(0, 1);
                noiseSettingSimple.centre.y = Random.Range(0, 1);
                noiseSettingSimple.centre.z = Random.Range(0, 1);
            }
            if (terrainSettings.noiseLayers[i].noiseSettings.filterType == NoiseSettings.FilterType.Rigid)
            {
                // Generate random values for all of the terrain settings of the planet so that it has a unique appearance.
                // The values have been chosen as comfortable values for each setting
                // Values outside these ranges tend to have alot less realistic terrain
                // The user can make it go beyond these values but for initial spawning of the planet, realistic terrain is chosen
                NoiseSettings.RigidNoiseSettings noiseSettingRigid = terrainSettings.noiseLayers[i].noiseSettings.rigidNoiseSettings;
                if (i == 0)
                {
                    noiseSettingRigid.strength = Random.Range(0.01f, 0.2f);
                }
                else
                {
                    noiseSettingRigid.strength = Random.Range(0.01f, 5f);
                }
                noiseSettingRigid.baseRoughness = Random.Range(0.4f, 2f);
                noiseSettingRigid.roughness = Random.Range(0.5f, 5f);
                noiseSettingRigid.persistence = Random.Range(0.1f, 0.5f);
                noiseSettingRigid.centre.x = Random.Range(0, 1);
                noiseSettingRigid.centre.y = Random.Range(0, 1);
                noiseSettingRigid.centre.z = Random.Range(0, 1);
            }
        }
    }
}
