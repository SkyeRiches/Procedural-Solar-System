using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlanet : MonoBehaviour
{
    private float planetDiameter;
    private float planetMass;
    private float planetDistance;
    private float planetSpin;

    //[SerializeField] private AnimationCurve animCurve;

    // Start is called before the first frame update
    void Awake()
    {
        planetDiameter = Random.Range(1, StarInfo.starDiameter / 2);
        transform.localScale *= planetDiameter;

        planetMass = Random.Range(0.05f, 500f); // Values chosen based on a scale model made using our solar system, these values work well for the physics model
        transform.GetComponent<Rigidbody>().mass = planetMass;

        planetDistance = Random.Range(StarInfo.starDiameter / 2, 20000); // Value may need altering depending on if low mass far distance would still follow orbit
        transform.position = new Vector3(planetDistance, 0, 0);

        planetSpin = Random.Range(1, 360);

        transform.GetComponent<PlanetSpin>().spinRate = planetSpin;
    }
}
