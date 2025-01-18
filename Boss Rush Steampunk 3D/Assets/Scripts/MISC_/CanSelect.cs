using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SmoothRandomBobbingAndRotation))]
public class CanSelect : MonoBehaviour
{
    public bool canSelect = false;                                  //  If the object can currently be selected
    public bool selected = false;                                   //  If the object currently is selected
    public Vector3 selectOffset = new Vector3(0, 0.25f, -0.25f);    //  What offset to add to the position when selected
    public float transitionSpeed = 2f;                              //  A multiplier for how fast to transition between the default position and the selected position
    private Vector3 initialLocalPosition;                           //  Save the initial position
    private Collider collider;                                      //  A reference to the collider for hit detection
    private SmoothRandomBobbingAndRotation bobble;                  //  A reference to the bobble script, so that it can be enabled/disabled
    public List<GameObject> buttons = new List<GameObject>();       //  The list of buttons that can be used when this target is selected

    private void Start()
    {
        //  Get the collider, bobble and health off of the object
        collider = GetComponent<Collider>();
        bobble = GetComponent<SmoothRandomBobbingAndRotation>();
        //  Disable the bobble and set the save the initial position
        bobble.active = false;
        initialLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (canSelect)
        {
            //  If we can be selected, then Raycast the mouse position and set selected equal to whether or not this object was hit
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                selected = collider == hit.collider;
            }
            else
            {
                selected = false;
            }
        }
        else
        {
            //  If we cannot be selected, set selected to false
            selected = false;
        }
        if (selected)
        {
            //  If selected, activate the bobble, and Lerp it's center position to the saved initial position plus the select offset
            bobble.initialLocalPosition = Vector3.Lerp(bobble.initialLocalPosition, initialLocalPosition + selectOffset, Time.deltaTime * transitionSpeed);
            bobble.active = true;
        }
        else
        {
            //  If not selected, deactivate the bobble, and Lerp it's center position to the saved initial position
            bobble.initialLocalPosition = Vector3.Lerp(bobble.initialLocalPosition, initialLocalPosition, Time.deltaTime * transitionSpeed);
            bobble.active = false;
        }
    }
}

