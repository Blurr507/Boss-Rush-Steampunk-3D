using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CreateObjectInBounds))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int health, maxHealth;  //  The current health and maximum health
    public UnityEvent die;          //  The event that will be called when this object dies (runs out of health)
    public bool alive = true;       //  Used to specify if the object is alive, to ensure that the death script is only called once
    public RectTransform hp;        //  A reference to this object's healthbar object
    public List<string> effects = new List<string>();   //  A list to keep track of the effects on this object (i.e. burning, wet, buffed, etc.)
    private CreateObjectInBounds damageNumberCreator;   //  Used to create a DamageNumber in a specified bounds.
    public static int healthbarScaleMultiplier = 200;   //  A multiplier for the scale of all healthbars

    private void Start()
    {
        StartOverride();
    }

    public virtual void StartOverride()
    {
        hp.parent.SetParent(GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform);
        if (die.GetPersistentEventCount() == 0)
        {
            die.AddListener(Die);
        }
        damageNumberCreator = GetComponent<CreateObjectInBounds>();
        damageNumberCreator.obj = BattleStateManager.me.damageNumberObject;
    }

    void Update()
    {
        hp.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthbarScaleMultiplier * health / maxHealth);
        if(health <= 0 && alive)
        {
            die.Invoke();
            alive = false;
        }
    }

    public void AddHealth(int num, int type = -1)
    {
        health = Mathf.Min(health + num, maxHealth);
        DamageNumber damageNumber = damageNumberCreator.CreateObject().GetComponent<DamageNumber>();
        damageNumber.damage = num;
        switch(type)
        {
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

    public void SubtractHealth(int num, int type = 0)
    {
        health = Mathf.Max(health - num, 0);
        DamageNumber damageNumber = damageNumberCreator.CreateObject().GetComponent<DamageNumber>();
        damageNumber.damage = num;
        switch(type)
        {
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

    public int GetHealth()
    {
        return health;
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDestroyOverride();
    }

    public virtual void OnDestroyOverride()
    {
        if (hp != null && hp.gameObject != null)
        {
            Destroy(hp.parent.gameObject);
            Destroy(hp.gameObject);
        }
    }
}
