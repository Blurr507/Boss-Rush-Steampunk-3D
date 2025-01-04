using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;

	//public bool boss;

	//private bool ready;

	public RectTransform hp;

    public void AddHealth(int num)
    {
        health += num;
    }

    public void SubtractHealth(int num)
    {
        health -= num;
    }

    public int GetHealth()
    {
        return health;
    }

	void Update(){
		hp.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
	}
}
