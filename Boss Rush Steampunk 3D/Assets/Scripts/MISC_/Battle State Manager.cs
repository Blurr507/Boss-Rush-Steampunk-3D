using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : MonoBehaviour
{
    //battleState refers to the state of the battle (choosing attacks stage, skill checks stage, boss damager stage, etc)
    //it controls things like logic and camera position
    //the main battle menu (choosing to attack or heal etc) is assigned the intereger 0
    //the attack menu (choosing attacks) is assigned the interger 1
    //the skill check stage is assigned the interger 2
    //the boss damage stage is assigned the interger 3
    //more stages to be added/assigned
    public int battleState = 0;

    public GameObject attacksMenu;
    public GameObject spinners;

    void Start()
    {
        
    }

    void Update()
    {
        //EVERYTHING IN THIS UPDATE METHOD IS TEMPORARY AND FOR SHOWCASE TO THE TEAM
        //also a switch case statement would be more efficient
        if (battleState == 1)
        {
            attacksMenu.SetActive(true);
        }
        else
        {
            attacksMenu.SetActive(false);
        }

        if (battleState == 2)
        {
            spinners.SetActive(true);
        }
        else
        {
            spinners.SetActive(false);
        }
    }
}
