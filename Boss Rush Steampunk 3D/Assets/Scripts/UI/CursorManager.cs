using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D cursorTextureHovering;
    public Vector2 clickOffset = Vector2.zero;
    public bool hovering = false;

    void Update()
    {
        RaycastHit hit;
        hovering = false;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            hovering = hit.transform.GetComponent<CanSelect>() && hit.transform.GetComponent<CanSelect>().canSelect;
        }
        Cursor.SetCursor(hovering ? cursorTextureHovering : cursorTexture, clickOffset, CursorMode.Auto);
        //MasterVolumeSlider.SetVolume();
    }
}
