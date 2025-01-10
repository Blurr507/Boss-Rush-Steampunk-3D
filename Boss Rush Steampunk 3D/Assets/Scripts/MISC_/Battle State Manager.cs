using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateManager : MonoBehaviour
{
    public Animator gooseAnimator;
    //battleState refers to the state of the battle (choosing attacks stage, skill checks stage, boss damager stage, etc)
    //it controls things like logic and camera position
    //the main battle menu (choosing to attack or heal etc) is assigned the intereger 0
    //the attack menu (choosing attacks) is assigned the interger 1
    //the skill check stage is assigned the interger 2
    //the boss damage stage is assigned the interger 3
    //the boss attack stage is assigned the integer 4
    //the defense stage is assigned the integer 5
    //the player damage stage is assigned the integer 6
    //more stages to be added/assigned
    [SerializeField]
    private int battleState = 0;

	public GameObject bubble;
    public List<CanSelect> selectables = new List<CanSelect>();
    public List<Enemy> enemies = new List<Enemy>();
    public CanSelect target;
    public int currentEnemy = 0;
    public GameObject damageNumberObject;
    public Color baseDamage = Color.white;
    public Color fireDamage = Color.red;
    public Color electricDamage = Color.yellow;
    public Color oilDamage = Color.black;
    public Color healDamage = Color.green;

    public static BattleStateManager me; //awful code

    void Awake()
    {
        Random.InitState(System.DateTime.Today.Second * System.DateTime.Today.Minute);
        me = this;
        for(int i = 0; i < selectables.Count; i++)
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

    public int GetState()
    {
        return battleState;
    }

    public void IncrementState()
    {
        switch(battleState)
        {
            case 0: //  Choose target
                //  Swap geaux's animation to "Thinking"
                gooseAnimator.SetBool("Thinking", true);
                //  Stop all CanSelects from being selectable
                for (int i = 0; i < selectables.Count; i++)
                {
                    selectables[i].canSelect = false;
                }
                //  Enable the chosen target's action buttons
                for(int i = 0; i < target.buttons.Count; i++)
                {
                    target.buttons[i].gameObject.SetActive(true);
                }
                //  Increment the state
                battleState = 1;
                break;
            case 1: //  Choose action
                //  Pull geaux from his deep thought
				gooseAnimator.SetBool("Thinking", false);
                //  Deactivate the action buttons
                for (int i = 0; i < target.buttons.Count; i++)
                {
                    target.buttons[i].gameObject.SetActive(false);
                }
                //  Increment the state
                battleState = 2;
                break;
            case 2: //  Skill check
                //  Make sure geaux isn't somehow thinking (we can't be having that now can we)
				gooseAnimator.SetBool("Thinking", false);
                //  Increment the state
                battleState = 3;
                break;
            case 3: //  Damage/heal target
                if (enemies.Count > 0)
                {
                    //  If there are any enemies, then proceed to state 4, starting an enemy's turn
                    ToState4();
                }
                else
                {
                    //  If there are no enemies, geaux may go
                    BackToState0();
                }
                break;
            case 4: //  Enemy attack
                //  Increment the state
                battleState = 5;
                break;
            case 5: //  Player Block
                //  Increment the state
                battleState = 6;
                break;
            case 6: //  Player Damage
                if(currentEnemy < enemies.Count)
                {
                    //  If more enemies need to play, then return to state 4
                    ToState4();
                }
                else
                {
                    //  Otherwise, go back to geaux
                    BackToState0();
                }
                break;
        }
    }

    public void HurtTarget(int hp, int damageType = 0)
    {
        //  Deal 'hp' damage of 'damageType' type to the selected target
        target.GetComponent<Health>().SubtractHealth(hp, damageType);
    }

    public void HealTarget(int hp, int damageType = -1)
    {
        //  Heal the selected target for the specified amount of hp and type
        target.GetComponent<Health>().AddHealth(hp, damageType);
    }

    public CanSelect GetTarget()
    {
        //  Return the selected target
        return target;
    }

    public void BackToState0()
    {
        //  Geaux can't do no thinking
		gooseAnimator.SetBool("Thinking", false);
        //  Make our selectables selectable
        for (int i = 0; i < selectables.Count; i++)
        {
            selectables[i].canSelect = true;
        }
        //  Set the state back to 0
        battleState = 0;
        //  Deactivate the selected target's action buttons
        for (int i = 0; i < target.buttons.Count; i++)
        {
            target.buttons[i].gameObject.SetActive(false);
        }
        //  Reset all enemies (specifically their turn count)
        ResetEnemies();
    }
    
    public void ToState4()
    {
        //  Don't let the geaux think
		//gooseAnimator.SetBool("Thinking", false);
        //  Make sure all selectables can't be selected
        for (int i = 0; i < selectables.Count; i++)
        {
            selectables[i].canSelect = false;
        }
        //  Set the state to 4
        battleState = 4;
        //  Make sure that the current target's buttons are deactivated
        for (int i = 0; i < target.buttons.Count; i++)
        {
            target.buttons[i].gameObject.SetActive(false);
        }

        //  Do the current enemy's turn
        //StartCoroutine(DoEnemyTurn());
        enemies[currentEnemy].DoTurn();
    }

    private IEnumerator DoEnemyTurn()
    {
        yield return new WaitForEndOfFrame();
        enemies[currentEnemy].DoTurn();
        //yield return null;
    }

    public void IncrementCurrentEnemy()
    {
        currentEnemy++;
    }

    public void ResetEnemies()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.ResetTurns();
        }
        currentEnemy = 0;
    }
}
