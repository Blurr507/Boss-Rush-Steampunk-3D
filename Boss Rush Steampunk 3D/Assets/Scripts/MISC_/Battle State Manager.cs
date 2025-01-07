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
    public int battleState = 0;

	public GameObject bubble;
    public List<CanSelect> selectables = new List<CanSelect>();
    public List<Enemy> enemies = new List<Enemy>();
    private CanSelect target;
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

    public void IncrementState()
    {
        switch(battleState)
        {
            case 0: //  Choose target
                gooseAnimator.Play("Thinking");
                for (int i = 0; i < selectables.Count; i++)
                {
                    selectables[i].canSelect = false;
                }
                for(int i = 0; i < target.buttons.Count; i++)
                {
                    target.buttons[i].gameObject.SetActive(true);
                }
                battleState = 1;
                break;
            case 1: //  Choose action
                gooseAnimator.Play("Idle");
                for (int i = 0; i < target.buttons.Count; i++)
                {
                    target.buttons[i].gameObject.SetActive(false);
                }
                battleState = 2;
                break;
            case 2: //  Skill check
                gooseAnimator.Play("Idle");
                battleState = 3;
                break;
            case 3: //  Damage/heal target
                if (enemies.Count > 0)
                {
                    ToState4();
                }
                else
                {
                    BackToState0();
                }
                break;
            case 4: //  Enemy attack
                battleState = 5;
                break;
            case 5: //  Player Block
                battleState = 6;
                break;
            case 6: //  Player Damage
                if(currentEnemy < enemies.Count)
                {
                    ToState4();
                }
                else
                {
                    BackToState0();
                }
                break;
        }
    }

    public void HurtTarget(int hp, int damageType = 0)
    {
        target.health.SubtractHealth(hp, damageType);
    }

    public void HealTarget(int hp, int damageType = -1)
    {
        target.health.AddHealth(hp, damageType);
    }

    public CanSelect GetTarget()
    {
        return target;
    }

    public void BackToState0()
    {
        gooseAnimator.Play("Idle");
        for (int i = 0; i < selectables.Count; i++)
        {
            selectables[i].canSelect = true;
        }
        battleState = 0;
        for (int i = 0; i < target.buttons.Count; i++)
        {
            target.buttons[i].gameObject.SetActive(false);
        }
        ResetEnemies();
    }

    public void ToState4()
    {
        gooseAnimator.Play("Idle");
        for (int i = 0; i < selectables.Count; i++)
        {
            selectables[i].canSelect = false;
        }
        battleState = 4;
        for (int i = 0; i < target.buttons.Count; i++)
        {
            target.buttons[i].gameObject.SetActive(false);
        }

        enemies[currentEnemy].DoTurn();
        if (enemies[currentEnemy].turns <= 0)
        {
            currentEnemy++;
        }
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
