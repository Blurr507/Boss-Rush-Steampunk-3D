using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreateObjectInBounds))]
public class Defend : SkillCheck
{
    public SteamGauge gauge; // A reference to the steam gauge in this object's children
    public ShieldBubble bubble; // A reference to the damage bubble in this object's children
    public AnimationCurve bubbleCurve; // The curve that the bubble follows when moving toward the target
    private int stage = 0; // Used to manage what part of the attack is happening
    public int failShield = 80; // The damage done if the spinner stops outside of the target angle
    public int hitShield = 100; // The damage done if the spinner stops in the target angle
    public int critShield = 120; // The damage done if the spinner stops in the target angle
    private CreateObjectInBounds create; // A reference to a CreateObjectInBounds component for creating the SmallDamage numbers
    private SkillCheck skillCheck; // Used for telling the button that the attack was cancelled

    void Start()
    {
        // Grab the guage, bubble and create from this object and its children
        gauge = GetComponentInChildren<SteamGauge>();
        bubble = GetComponentInChildren<ShieldBubble>();
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
                    SmallShield shield = create.CreateObject().GetComponent<SmallShield>();
                    switch (gauge.result)
                    {
                        case 0:
                            shield.shield = failShield;
                            break;
                        case 1:
                            shield.shield = hitShield;
                            break;
                        case 2:
                            shield.shield = critShield;
                            break;
                    }
                    //  Increment stage
                    stage++;
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    //  Cancel this healing if we haven't clicked yet, and we press RMB
                    Cancel();
                    BattleStateManager.me.BackToState0();
                    Destroy(bubble.gameObject);
                    Destroy(gameObject);
                }
                break;
            case 1: //  Move the camera to look at the boss in 2 seconds, and increment the stage to prevent it from being called again
                Invoke("Move", 2f);
                stage++;
                break;
        }
    }

    private void Spin()
    {
        //  Guess what this does
        gauge.Spin();
    }

    private void Move()
    {
        //  Increment the battle state to looking at the boss, move the damage bubble, and call attack in 1 second
        BattleStateManager.me.IncrementState();
        bubble.MoveToPos(BattleStateManager.me.GetTarget().transform.position, 1f, bubbleCurve);
        Invoke("AddShield", 0.99f);
    }

    private void AddShield()
    {
        //  Deal damage to the battle state's selected enemy, and call done in 1 second
        FindObjectOfType<Geaux>().AddShield(bubble.shield);
        Invoke("Done", 1f);
    }

    private void Done()
    {
        //  Increment the battle to the boss attack state and destroy this object, and it's damage bubble
        BattleStateManager.me.IncrementState();
        //Destroy(bubble.gameObject);
        Destroy(gameObject);
    }
}
