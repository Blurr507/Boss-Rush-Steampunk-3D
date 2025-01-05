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

	public GameObject bubble;
    public List<CanSelect> selectables = new List<CanSelect>();
    private CanSelect target;

	public static BattleStateManager me; //awful code

    void Start()
    {
		me = this;
        for(int i =0; i < selectables.Count; i++)
        {
            selectables[i].canSelect = true;
        }
    }

    void Update()
    {
        switch(battleState)
        {
            case 0: //  Select who to interact with
                if(Input.GetMouseButtonDown(0))
                {
                    for(int i = 0; i < selectables.Count; i++)
                    {
                        if (selectables[i].selected)
                        {
                            target = selectables[i];
                            IncrementState();
                        }
                    }
                }
                break;
        }
    }

    public void IncrementState()
    {
        switch(battleState)
        {
            case 0:
                for(int i = 0; i < selectables.Count; i++)
                {
                    selectables[i].canSelect = false;
                }
                for(int i = 0; i < target.buttons.Count; i++)
                {
                    target.buttons[i].gameObject.SetActive(true);
                }
                battleState = 1;
                break;
            case 1:
                for(int i = 0; i < target.buttons.Count; i++)
                {
                    target.buttons[i].gameObject.SetActive(false);
                }
                battleState = 2;
                break;
            case 2:
                battleState = 3;
                break;
            case 3:
                for (int i = 0; i < selectables.Count; i++)
                {
                    selectables[i].canSelect = true;
                }
                battleState = 0;
                break;
        }
    }

    public void HurtTarget(int hp)
    {
        target.health.health -= hp;
    }
}
