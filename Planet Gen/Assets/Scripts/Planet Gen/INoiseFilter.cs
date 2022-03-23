using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoiseFilter
{
    // turns the value of the point from -1 to 1 into 0 to 1
    float Evaluate(Vector3 point);
}
