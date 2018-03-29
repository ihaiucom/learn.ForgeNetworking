using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class WarRandom
{
    public static float Range(float minValue, float maxValue)
    {
        return Random.Range(minValue, maxValue);
        //return TSRandom.Range(minValue, maxValue);
    }
}
