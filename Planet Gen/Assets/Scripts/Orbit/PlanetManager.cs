using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] const float G = 300f;
    private GameObject sun;

    public void SetSun(GameObject sunObj)
    {
        sun = sunObj;
    }

    // --------------------------------------------------- PHYSICS FUNTIONS ----------------------------------------------------------\\

    private void Start()
    {
        if (CelestialList.celestialBodies.Count > 1)
        {
            foreach (CelestialBody ce in CelestialList.celestialBodies)
            {
                foreach (CelestialBody ce2 in CelestialList.celestialBodies)
                {
                    InitialVelocity(ce.rb, ce2.rb);
                }
            }
        }
    }

    /// <summary>
    /// Calculate the initial velocity upon this objects creation
    /// </summary>
    private void Awake()
    {
        if (CelestialList.celestialBodies == null) CelestialList.celestialBodies = new List<CelestialBody>();
    }

    /// <summary>
    /// calculate and apply the attractive force between two celestial objects using the rigidbody component
    /// and Newtons law of universal gravitation
    /// </summary>
    void Attract(Rigidbody rb, CelestialBody objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        // Calculate the magnitude of the distance between the two celestial bodies
        Vector3 vec3Dir = rb.position - rbToAttract.position;
        float distance = vec3Dir.magnitude;

        if (distance <= 0) return;

        // Calculate the force using Newton's equation of: F = G ((m1 * m2) / r^2)
        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = vec3Dir.normalized * forceMagnitude;

        // Apply the gravitational pull force
        rbToAttract.AddForce(force);
    }

    void InitialVelocity(Rigidbody rb, Rigidbody otherRb)
    {
        float m1 = rb.mass;
        float r = (rb.position - otherRb.position).magnitude;

        if (r <= 0)
        {
            return;
        }

        // Calculate the initial velocity of the celestial body using the equation: V = sqrt((G * m1) / r)
        // The equation for initial velocity is derrived from newtons equation for universal gravitation
        // assuming that centripetal force is equal to gravitational force
        float vel = Mathf.Sqrt((G * m1) / r);
        otherRb.transform.LookAt(rb.transform);
        otherRb.velocity += otherRb.transform.right * vel;
    }

    /// <summary>
    /// Every fixed update loop, calculate the gravitational pull between this and the other celestial bodies in the scene
    /// </summary>
    private void FixedUpdate()
    {
        if (CelestialList.celestialBodies.Count > 1)
        {
            foreach (CelestialBody ce in CelestialList.celestialBodies)
            {
                foreach (CelestialBody ce2 in CelestialList.celestialBodies)
                {
                    Attract(ce2.rb, ce);
                }
            }
        }
    }

    public void NewPlanetVel(GameObject spawnedPlanet)
    {
        InitialVelocity(sun.GetComponent<Rigidbody>(), spawnedPlanet.GetComponent<Rigidbody>());
    }
}