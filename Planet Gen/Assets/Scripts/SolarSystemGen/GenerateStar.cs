using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateStar : MonoBehaviour
{
    private float starDiameter;
    private float starMass;
    private float starSpin;

    // Start is called before the first frame update
    void Start()
    {
        starDiameter = Random.Range(100, 1000);
        StarInfo.starDiameter = starDiameter;
        transform.localScale *= starDiameter;
        starMass = Random.Range(100000, 500000);
        StarInfo.starMassValue = starMass;
        transform.GetComponent<Rigidbody>().mass = starMass;
    }

    private void Update()
    {
        transform.Rotate(0, starSpin * Time.deltaTime, 0);
    }
}
