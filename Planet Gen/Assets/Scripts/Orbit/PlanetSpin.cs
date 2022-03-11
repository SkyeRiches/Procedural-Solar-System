using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpin : MonoBehaviour
{
    [SerializeField] public float spinRate = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spinRate * Time.deltaTime, 0);
    }

    void SetSpin(float spin)
    {
        spinRate = spin;
    }
}
