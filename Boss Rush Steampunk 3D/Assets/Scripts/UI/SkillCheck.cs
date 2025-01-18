using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheck : MonoBehaviour
{
    public SelectableButton button;
    
    public void Cancel()
    {
        button.ResetCooldown();
    }
}
