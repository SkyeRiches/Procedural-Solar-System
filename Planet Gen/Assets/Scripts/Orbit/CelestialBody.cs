using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField] const float G = 100f;

    public static List<CelestialBody> celestialBodies;
    
    public Rigidbody rb;

    private void OnEnable()
    {
        if (celestialBodies == null) celestialBodies = new List<CelestialBody>();
        celestialBodies.Add(this);
    }
    private void OnDisable()
    {
        celestialBodies.Remove(this);
    }

    private void Start()
    {
        foreach (CelestialBody ce in celestialBodies)
        {
            if (ce != this) InitialVelocity(ce.rb);
        }
    }

    private void FixedUpdate()
    {
        foreach(CelestialBody ce in celestialBodies)
        {
            if (ce != this) Attract(ce);
        }
    }

    void Attract(CelestialBody objToAttract)
    {
        Rigidbody rbToAttract = objToAttract.rb;

        Vector3 vec3Dir = rb.position - rbToAttract.position;
        float distance = vec3Dir.magnitude;

        if (distance == 0) return;

        float forceMagnitude = G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = vec3Dir.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }

    void InitialVelocity(Rigidbody otherRb)
    {
        float m1 = rb.mass;
        float r = (rb.position - otherRb.position).magnitude;

        float vel = Mathf.Sqrt((G * m1) / r);
        otherRb.transform.LookAt(rb.transform);
        otherRb.velocity += otherRb.transform.right * vel;
    }
}
