using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class HelperFunctions
{
    public static Vector3 RandomPosInBounds(Bounds bounds)
    {
        return new Vector3(Random.Range(bounds.center.x - bounds.extents.x, bounds.center.x + bounds.extents.x),
                           Random.Range(bounds.center.y - bounds.extents.y, bounds.center.y + bounds.extents.y),
                           Random.Range(bounds.center.z - bounds.extents.z, bounds.center.z + bounds.extents.z));
    }
}
