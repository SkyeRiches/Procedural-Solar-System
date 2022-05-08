using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
    [SerializeField] public float spinRate = 0f;

    // Update is called once per frame
    void Update()
    {
        // Make the planet rotate on the spot to visualise planet axis spin
        transform.Rotate(0, spinRate * Time.deltaTime, 0);
    }
}
