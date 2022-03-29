using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlanetManager : MonoBehaviour
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
        if (GhostCelestials.ghostCelestials.Count > 1)
        {
            foreach (GameObject ce in GhostCelestials.ghostCelestials)
            {
                foreach (GameObject ce2 in GhostCelestials.ghostCelestials)
                {
                    InitialVelocity(ce.GetComponent<Rigidbody>(), ce2.GetComponent<Rigidbody>());
                }
            }
        }
    }

    /// <summary>
    /// Calculate the initial velocity upon this objects creation
    /// </summary>
    private void Awake()
    {
        if (GhostCelestials.ghostCelestials == null) GhostCelestials.ghostCelestials = new List<GameObject>();
    }

    /// <summary>
    /// calculate and apply the attractive force between two celestial objects using the rigidbody component
    /// and Newtons law of universal gravitation
    /// </summary>
    void Attract(Rigidbody rb, GameObject objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.GetComponent<Rigidbody>();

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
        if (GhostCelestials.ghostCelestials.Count > 1)
        {
            foreach (GameObject ce in GhostCelestials.ghostCelestials)
            {
                foreach (GameObject ce2 in GhostCelestials.ghostCelestials)
                {
                    Attract(ce2.GetComponent<Rigidbody>(), ce);
                }
            }
        }
    }

    public void NewPlanetVel(GameObject spawnedPlanet)
    {
        InitialVelocity(sun.GetComponent<Rigidbody>(), spawnedPlanet.GetComponent<Rigidbody>());
    }
}
