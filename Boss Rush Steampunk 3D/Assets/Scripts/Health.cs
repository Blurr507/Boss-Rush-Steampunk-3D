using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CreateObjectInBounds))]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int health, maxHealth;
    public UnityEvent die;
    public bool alive = true;
    public RectTransform hp;
    private CreateObjectInBounds damageNumberCreator;

    private void Start()
    {
        if(die.GetPersistentEventCount() == 0)
        {
            die.AddListener(Die);
        }
        damageNumberCreator = GetComponent<CreateObjectInBounds>();
        damageNumberCreator.obj = BattleStateManager.me.damageNumberObject;
    }
    void Update()
    {
        hp.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
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
        if(hp != null && hp.gameObject != null)
        {
            Destroy(hp.parent.gameObject);
            Destroy(hp.gameObject);
        }
    }
}
