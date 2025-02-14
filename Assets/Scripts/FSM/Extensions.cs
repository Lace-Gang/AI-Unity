using UnityEngine;

public static class Extentions
{
    //gets distance between two positions but ignores y values
    public static float DistanceXZ(this Vector3 v1, Vector3 v2) 
    {
        Vector3 v = v1 - v2;
        v.y = 0;

        return v.magnitude;
    }
}
