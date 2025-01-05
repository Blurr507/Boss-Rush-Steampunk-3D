using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreateObjectInBounds))]
public class BasicAttack : MonoBehaviour
{
    public SteamGauge gauge;
    public DamageBubble bubble;
    private int stage = 0;
    public int failDamage = 5; // The damage done if the spinner stops outside of the target angle
    public int hitDamage = 20; // The damage done if the spinner stops in the target angle
    public int critDamage = 50; // The damage done if the spinner stops in the target angle
    private CreateObjectInBounds create;

    void Start()
    {
        gauge = GetComponentInChildren<SteamGauge>();
        bubble = GetComponentInChildren<DamageBubble>();
        create = GetComponent<CreateObjectInBounds>();
        Invoke("Spin", 0f);
        
    }

    void Update()
    {
        switch(stage)
        {
            case 0:
                if (gauge.result != -1)
                {
                    Debug.Log(gauge.result);
                    SmallDamage damage = create.CreateObject().GetComponent<SmallDamage>();
                    switch (gauge.result)
                    {
                        case 0:
                            damage.damage = failDamage;
                            break;
                        case 1:
                            damage.damage = hitDamage;
                            break;
                        case 2:
                            damage.damage = critDamage;
                            break;
                    }
                    stage++;
                }
                break;
            case 1:
                Invoke("Move", 2f);
                stage++;
                break;
        }
    }

    private void Spin()
    {
        gauge.Spin();
    }

    private void Move()
    {
        BattleStateManager.me.IncrementState();
        Invoke("Attack", 1f);
    }

    private void Attack()
    {
        BattleStateManager.me.HurtTarget(bubble.damage);
        Invoke("Done", 1f);
    }

    private void Done()
    {
        BattleStateManager.me.IncrementState();
        Destroy(bubble.gameObject);
        Destroy(gameObject);
    }
}
