using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanseButton : SelectableButton
{
    private void OnEnable()
    {
        ManageButtonActiveness();
    }

    public override void ManageButtonActiveness()
    {
        if (currentCooldown <= 0 && FindObjectOfType<Geaux>() != null && FindObjectOfType<Geaux>().effects.Count > 0)
        {
            button.interactable = true;
            text.text = actionDescription;
        }
        else if(currentCooldown > 0)
        {
            text.text = actionDescription + $" ({currentCooldown})";
            button.interactable = false;
        }
        else
        {
            text.text = actionDescription;
            button.interactable = false;
        }
    }
}
