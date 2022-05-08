using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the information of the star so other classes can access it
/// </summary>
public static class StarInfo
{
    private static float starSize;
    private static float starMass;

    public static float starDiameter { get; set; }
    public static float starMassValue { get; set; }
}
