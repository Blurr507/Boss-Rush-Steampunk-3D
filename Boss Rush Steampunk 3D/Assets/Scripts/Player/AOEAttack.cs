using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttack : SkillCheck
{
    public SteamGauge gauge; // A reference to the steam gauge in this object's children
    public DamageBubble bubble; // A reference to the damage bubble in this object's children
    public AnimationCurve bubbleCurve; // The curve that the bubble follows when moving toward the target
    private int stage = 0; // Used to manage what part of the attack is happening
    public int failDamage = 10; // The damage done if the spinner stops outside of the target angle
    public int hitDamage = 20; // The damage done if the spinner stops in the target angle
    public int critDamage = 30; // The damage done if the spinner stops in the target angle
    public int damageType = 0;  // This dictates the damage type.
    private CreateObjectInBounds create; // A reference to a CreateObjectInBounds component for creating the SmallDamage numbers

    void Start()
    {
        // Grab the guage, bubble and create from this object and its children
        gauge = GetComponentInChildren<SteamGauge>();
        bubble = GetComponentInChildren<DamageBubble>();
        create = GetComponent<CreateObjectInBounds>();
        // Spin the wheel in .5 seconds
        Invoke("Spin", 0.5f);
    }

    void Update()
    {
        switch (stage)
        {
            case 0: //  While the wheel is spinning
                if (gauge.result != -1)
                {
                    //  Once the spinner stops, create a small damage object, and assign it's number to the correct damage
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
                    //  Increment stage
                    stage++;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    //  Cancel this attack if we haven't attacked yet, and we press RMB
                    Cancel();
                    BattleStateManager.me.BackToState0();
                    Destroy(bubble.gameObject);
                    Destroy(gameObject);
                }
                break;
            case 1: //  Move the camera to look at the boss in 2 seconds, and increment the stage to prevent it from being called again
                StartCoroutine(MoveDamageBubble());
                stage++;
                break;
        }
    }

    private void Spin()
    {
        //  Guess what this does
        gauge.Spin();
    }

    private IEnumerator MoveDamageBubble()
    {
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < BattleStateManager.me.enemies.Count; i++)
        {
            Enemy enemy = BattleStateManager.me.enemies[i];
            bubble.MoveToPos(enemy.transform.position, 1f, bubbleCurve, false);
            yield return new WaitForSeconds(1f);
            enemy.SubtractHealth(bubble.damage, damageType);
            if(enemy.GetHealth() <= 0)
            {
                i--;
            }
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(bubble.gameObject);
        yield return new WaitForSeconds(1f);
        Done();
    }

    private void Done()
    {
        //  Increment the battle to the boss attack state and destroy this object, and it's damage bubble
        BattleStateManager.me.IncrementState();
        //Destroy(bubble.gameObject);
        Destroy(gameObject);
    }
}
