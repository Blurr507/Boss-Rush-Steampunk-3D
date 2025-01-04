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

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
