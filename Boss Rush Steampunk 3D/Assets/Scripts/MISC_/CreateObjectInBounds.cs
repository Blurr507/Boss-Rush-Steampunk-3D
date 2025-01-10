using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjectInBounds : MonoBehaviour
{
    public GameObject obj;
    public Bounds bounds;
    public bool faceCamera = true;

    public GameObject CreateObject()
    {
        Vector3 pos = HelperFunctions.RandomPosInBounds(new Bounds(transform.TransformPoint(bounds.center), bounds.size));
        return Instantiate(obj, pos, faceCamera ? Quaternion.LookRotation(pos - Camera.main.transform.position) : Quaternion.identity, GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.TransformPoint(bounds.center), bounds.size);
    }
}
