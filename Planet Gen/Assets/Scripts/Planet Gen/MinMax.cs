using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stores the highest and lowest points of the terrain
/// </summary>
public class MinMax
{
    public float Min { get; private set; }
    public float Max { get; private set; }

    public MinMax()
    {
        Min = float.MaxValue;
        Max = float.MinValue;
    }

    public void AddValue(float v)
    {
        if (v > Max)
        {
            Max = v;
        }
        if (v < Min)
        {
            Min = v;
        }
    }
}
