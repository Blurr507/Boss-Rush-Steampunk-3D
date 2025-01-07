/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditor.IMGUI.Controls;

[CustomEditor(typeof(CreateObjectInBounds))]
public class CreateObjectBounds : Editor
{
    CreateObjectInBounds create;
    BoxBoundsHandle boxBoundsHandle = new BoxBoundsHandle();

    private void OnEnable()
    {
        create = (CreateObjectInBounds)target;
        boxBoundsHandle.size = create.bounds.size;
        boxBoundsHandle.center = create.bounds.center;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        boxBoundsHandle.DrawHandle();
        create.bounds = new Bounds(boxBoundsHandle.center, boxBoundsHandle.size);
    }
} */
