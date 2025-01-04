using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageBubble : MonoBehaviour
{
    private TextMeshProUGUI text;   //  Text object that displays the current damage
    public int damage;              //  The currently stored amount of damage

    void Start()
    {
        //  Set text equal to the TMP object in this object's children
        text = GetComponentInChildren<TextMeshProUGUI>();
        //  Make the text invisible if the current damage is 0
        text.gameObject.SetActive(damage != 0);
    }

    //  Add damage to the total
    public void AddDamage(int num)
    {
        //  Add the damage, and assume that it's a positive amount, so we want the text to be visible
        damage += num;
        text.gameObject.SetActive(true);
        //  Update the text
        text.text = damage.ToString();
    }

	public void Reset(){ //reset damage
		damage = 0;
		text.gameObject.SetActive(false);
	}
}
