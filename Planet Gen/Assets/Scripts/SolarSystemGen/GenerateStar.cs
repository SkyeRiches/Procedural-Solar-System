using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Upon the system being run, the star will have random values generated for its attributes
/// </summary>
public class GenerateStar : MonoBehaviour
{
    private float starDiameter;
    private float starSpin;

    // Start is called before the first frame update
    void Awake()
    {
        // generate random star size
        starDiameter = Random.Range(1, 10);
        StarInfo.starDiameter = starDiameter;
        transform.localScale *= starDiameter;

        starSpin = 10f;
    }

    private void Update()
    {
        // The star rotates on the spot to visualise celestial axis spin
        transform.Rotate(0, starSpin * Time.deltaTime, 0);
    }
}
