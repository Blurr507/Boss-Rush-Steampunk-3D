using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CreateObjectInBounds))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int health, maxHealth;  //  The current health and maximum health
    public bool alive = true;       //  Used to specify if the object is alive, to ensure that the death script is only called once
    public RectTransform hp;        //  A reference to this object's healthbar object
    public TextMeshProUGUI healthIndicator;             //  A text object saying how much health ya got
    public List<string> effects = new List<string>();   //  A list to keep track of the effects on this object (i.e. burning, wet, buffed, etc.)
    [HideInInspector]
    public CreateObjectInBounds damageNumberCreator;    //  Used to create a DamageNumber in a specified bounds.
    public static int healthbarScaleMultiplier = 200;   //  A multiplier for the scale of all healthbars

    private void Start()
    {
        //  This was because I didn't realize that scripts inheriting from Health could have their own Start Event, all of the desired start code is in StartOverride()
        StartOverride();
    }

    public virtual void StartOverride()
    {
        //  Make sure that the healthbar's background (which is it's parent) is a child of the WorldSpaceCanvas
        hp.parent.SetParent(GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform);
        //  Assign damageNumberCreator to our CreateObjectInBounds, and set the object it creates to the damageNumberObject assigned in the BattleStateManager
        damageNumberCreator = GetComponent<CreateObjectInBounds>();
        damageNumberCreator.obj = BattleStateManager.me.damageNumberObject;
    }

    void Update()
    {
        ManageHealth();
    }

    public virtual void ManageHealth()
    {
        //  Scale the healthbar based on the amount of health that we have versus our maxHealth
        if (alive)
        {
            hp.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthbarScaleMultiplier * health / maxHealth);
            UpdateHealthText();
            if (health <= 0)
            {
                //  If we're out of health and still alive, call our death event, and set alive to false (to prevent the death event from being called more than once)
                Die();
                alive = false;
            }
        }
        else
        {
            hp.gameObject.SetActive(false);
        }
    }

    //  Adds `num` health of type `type` up to our maxHealth
    public void AddHealth(int num, int type = -1)
    {
        //  Add the health, but clamp it to not go above our maxHealth
        health = Mathf.Min(health + num, maxHealth);
        //  Create a DamageNumber, and save a reference of it so that we can assign its healing
        DamageNumber damageNumber = damageNumberCreator.CreateObject().GetComponent<DamageNumber>();
        damageNumber.damage = num;
        switch(type)
        {
            //  Set the color of the damageNumber to the appropriate type's color
            case -1:
                damageNumber.color = BattleStateManager.me.healDamage;
                break;
            case 0:
                damageNumber.color = BattleStateManager.me.baseDamage;
                break;
            case 1:
                damageNumber.color = BattleStateManager.me.fireDamage;
                break;
            case 2:
                damageNumber.color = BattleStateManager.me.electricDamage;
                break;
            case 3:
                damageNumber.color = BattleStateManager.me.oilDamage;
                break;
            default:
                damageNumber.color = BattleStateManager.me.baseDamage;
                break;
        }
    }

    //  Subtracts 'num' health of type 'type'
    public virtual void SubtractHealth(int num, int type = 0)
    {
        //  Reduce health by num down to a minimum of 0
        health = Mathf.Max(health - num, 0);
        //  Create a DamageNumber, and save a reference of it so that we can assign its damage
        DamageNumber damageNumber = damageNumberCreator.CreateObject().GetComponent<DamageNumber>();
        damageNumber.damage = num;
        switch(type)
        {
            //  Set the color of the damageNumber to the appropriate type's color
            case -1:
                damageNumber.color = BattleStateManager.me.healDamage;
                break;
            case 0:
                damageNumber.color = BattleStateManager.me.baseDamage;
                break;
            case 1:
                damageNumber.color = BattleStateManager.me.fireDamage;
                break;
            case 2:
                damageNumber.color = BattleStateManager.me.electricDamage;
                break;
            case 3:
                damageNumber.color = BattleStateManager.me.oilDamage;
                break;
            default:
                damageNumber.color = BattleStateManager.me.baseDamage;
                break;
        }
    }

    //  Returns this object's health
    public int GetHealth()
    {
        return health;
    }

    //  Returns this object's health
    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    //  Returns this object's max health
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    //  An overridable event for what happens when this object dies
    public virtual void Die()
    {
        //  The default die event is just destroying this object
        Destroy(gameObject);
    }

    //  An overridable method for the classes that inherit from Health to individually say how they respond to specific effects. Returns whether or not the effect was ignored.
    public virtual bool AddEffect(string effect)
    {
        return false;
    }

    public void UpdateHealthText()
    {
        healthIndicator.text = $"{health}/{maxHealth}";
    }

    private void OnDestroy()
    {
        //  This is because I didn't realize that scripts inheriting from Health still have their own OnDestroy Event. All OnDestroy code is found in OnDestroyOverride
        OnDestroyOverride();
    }

    //  An overridable method for what happens when this obejct is destroyed
    public virtual void OnDestroyOverride()
    {
        //  By default, when destroyed, make sure to also destroy the healthbar if it still exists
        if (hp != null && hp.gameObject != null)
        {
            Destroy(hp.parent.gameObject);
            Destroy(hp.gameObject);
        }
    }
}
