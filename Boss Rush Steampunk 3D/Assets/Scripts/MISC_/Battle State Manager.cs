using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleStateManager : MonoBehaviour
{
    public Animator gooseAnimator;

	public AudioSource[] audios;
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

	public LineRenderer lr;

	public Transform[] lrpos;
    [HideInInspector] public List<SelectableButton> buttons;                     //  A hidden list of all buttons in the scene, so that the BattleStateManager can access them when they're deactivated.

    private int battleState = 0;

    public List<CanSelect> selectables = new List<CanSelect>(); //  A list of all the things that can be selected in battleState 0 (i.e. enemies, heroes, chandeliers, etc.)
    public List<CanSelect> heroes = new List<CanSelect>();      //  A list of the heroes in the scene, (currently only geaux, but this will be used to help enemies choose who to target)
    public List<Enemy> enemies = new List<Enemy>();             //  A list of the enemies in the scene, used to keep track of turns, and to figure out win conditions eventually
    public CanSelect target;                                    //  The thing that was clicked on during battleState 0.
    public int currentEnemy = 0;                                //  An integer to keep track of which enemy's turn it currently is
    public GameObject damageNumberObject;                       //  A prefab for creating a damageNumberObject. (Referenced by the health objects so that we don't have to add it to all of them)
    public Color baseDamage = Color.white;                      //  The color of the damage numbers when using base(regular) damage
    public Color fireDamage = Color.red;                        //  The color of the damage numbers when using fire damage
    public Color electricDamage = Color.yellow;                 //  The color of the damage numbers when using electric damage
    public Color oilDamage = Color.black;                       //  The color of the damage numbers when using oil damage
    public Color healDamage = Color.green;                      //  The color of the damage numbers when using heal damage
    public bool paused = false;                                 //  Used to pause the battle for cinematics and stuff

    public static BattleStateManager me; //awful code

    void Awake()
    {
        //  Randomize the random number generator
        Random.InitState(System.DateTime.Today.Second * System.DateTime.Today.Minute);
        //  Set the static reference 'me' to this object, so that it can be reference by other scripts
        me = this;
        BackToState0();
    }

    void Update()
    {
        if (lrpos[0] != null)
        {
            lr.SetPosition(0, lrpos[0].position);
        }
        if (lrpos[1] != null)
        {
            lr.SetPosition(1, lrpos[1].position);
        }
        switch(battleState)
        {
            case 0: //  Select who to interact with
                if(Input.GetMouseButtonDown(0))
                {
                    //  If we're clicking, check if we're hovering over any of our selectables
                    for(int i = 0; i < selectables.Count; i++)
                    {
                        if (selectables[i].selected)
                        {
                            //  If we are, make that selectable the target, and increment the state
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
        if (!paused)
        {
            switch (battleState)
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
                    for (int i = 0; i < target.buttons.Count; i++)
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
                    for (int i = 0; i < selectables.Count; i++)
                    {
                        selectables[i].canSelect = false;
                    }
                    //  Increment the state
                    battleState = 2;
                    break;
                case 2: //  Skill check
                        //  Make sure geaux isn't somehow thinking (we can't be having that now can we)
                    gooseAnimator.SetBool("Thinking", false);
                    for (int i = 0; i < selectables.Count; i++)
                    {
                        selectables[i].canSelect = false;
                    }
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
                    if (currentEnemy < enemies.Count)
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
        //  If the battleState is currently 6, then we are on the last state, and this turn is over
        bool battlePhaseOver = battleState == 6;
        //  Set the state back to 0
        battleState = 0;
        //  If there is a selected target, then deactivate its buttons
        if (target != null)
        {
            for (int i = 0; i < target.buttons.Count; i++)
            {
                target.buttons[i].gameObject.SetActive(false);
            }
        }
        //  Reset all enemies (specifically their turn count) if we were just on battleState 6
        //  When reseting, also tick down all of the button cooldowns
        if (battlePhaseOver)
        {
            ResetEnemies();
            DecreaseButtonCooldowns();
            ManageGeauxDOTs();
        }
    }
    
    public void ToState4()
    {
        //  Don't let the geaux think
		gooseAnimator.SetBool("Thinking", false);
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
        enemies[currentEnemy].DoTurn();
    }

    //  For focusing on geaux when he dies
    public void ToState7()
    {
        battleState = 7;
    }

    public void IncrementCurrentEnemy()
    {
        //  Guess what this does
        currentEnemy++;
    }

    public void ResetEnemies()
    {
        //  Reset the turns for each enemy
        foreach(Enemy enemy in enemies)
        {
            enemy.ResetTurns();
        }
        //  Set currentEnemy back to 0
        currentEnemy = 0;
    }

    public void DecreaseButtonCooldowns()
    {
        Debug.Log($"There are {buttons.Count} buttons.");
        foreach(SelectableButton button in buttons)
        {
            button.StepCooldown();
        }
    }

    public void ManageGeauxDOTs()
    {
        Geaux geaux = FindObjectOfType<Geaux>();
        if(geaux != null && geaux.effects.Contains("burning"))
        {
            geaux.Burn();
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void NextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            BackToMenu();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
