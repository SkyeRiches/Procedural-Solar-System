using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public float m_Speed = 1.0f;
    private GameObject m_Star = null;

    // Start is called before the first frame update
    void Start()
    {
        // Find the star in the scene when the planet spawns in
        m_Star = GameObject.FindGameObjectWithTag("Star");
    }

    // Update is called once per frame
    void Update()
    {
        // Rotates around the star, using it as the pivot point for rotation in order to visualise gravity
        gameObject.transform.RotateAround(m_Star.transform.position, Vector3.up, m_Speed * Time.deltaTime);
    }
}
