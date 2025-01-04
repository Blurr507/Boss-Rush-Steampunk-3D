using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(SteamGauge))]
public class SteamGaugeBoundingBox : Editor
{
    SteamGauge gauge;
    BoxBoundsHandle boxBoundsHandle = new BoxBoundsHandle();

    private void OnEnable()
    {
        gauge = (SteamGauge)target;
        boxBoundsHandle.size = gauge.bounds.size;
        boxBoundsHandle.center = gauge.bounds.center;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        boxBoundsHandle.DrawHandle();
        gauge.bounds = new Bounds(boxBoundsHandle.center, boxBoundsHandle.size);
    }
}
