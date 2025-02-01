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
    public int hitDamage = 10; // The damage done if the spinner stops in the target angle
    public int critDamage = 15; // The damage done if the spinner stops in the target angle
    public int damageCap = 50; // The damage at which it starts to fall off, and speed gets much faster
    public float damageMultiplier = 1; // The amount that the damage is multiplied by
    public float damageReductionMultiplier = 0.75f; // The amount that the new damage is multiplied by after passing the damageCap
    public int damageType = 0;  // This dictates the damage type.
    public float startRotSpeed = 400f; // This is how fast it starts spinning
    public float rotHitMultiplierLow = -1.1f; // How much the rotation speed is multiplied with each successful hit
    public float rotHitMultiplierHigh = -1.2f; // How much the rotation speed is multiplied with each successful hit
    private CreateObjectInBounds create; // A reference to a CreateObjectInBounds component for creating the SmallDamage numbers
    private bool canCancel = true; // Set to false once we click once for the wheel

	public AudioSource aud;

    void Start()
    {
        // Grab the guage, bubble and create from this object and its children
        gauge = GetComponentInChildren<SteamGauge>();
        gauge.rotationSpeed = startRotSpeed;
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
                    canCancel = false;
					aud.Play();
					aud.pitch += 0.1f;
                    //  Once the spinner stops, create a small damage object, and assign it's number to the correct damage
                    SmallDamage damage = create.CreateObject().GetComponent<SmallDamage>();
                    switch (gauge.result)
                    {
                        case 0:
                            damage.damage = Mathf.CeilToInt(failDamage * damageMultiplier);
                            //  Increment stage
                            stage++;
                            break;
                        case 1:
                            if (gauge.tolerance > 0)
                            {
                                gauge.tolerance--;
                            }
                            if (bubble.damage >= damageCap)
                            {
                                gauge.rotationSpeed *= rotHitMultiplierHigh;
                                damageMultiplier *= damageReductionMultiplier;
                            }
                            else
                            {
                                gauge.rotationSpeed *= rotHitMultiplierLow;
                            }
                            damage.damage = Mathf.CeilToInt(hitDamage * damageMultiplier);
                            gauge.Spin();
							
                            break;
                        case 2:
                            if (gauge.tolerance > 0)
                            {
                                gauge.tolerance--;
                            }
                            if (bubble.damage >= damageCap)
                            {
                                gauge.rotationSpeed *= rotHitMultiplierHigh;
                                damageMultiplier *= damageReductionMultiplier;
                            }
                            else
                            {
                                gauge.rotationSpeed *= rotHitMultiplierLow;
                            }
                            damage.damage = Mathf.CeilToInt(critDamage * damageMultiplier);
                            gauge.Spin();
                            break;
                    }
                }
                else if (canCancel && Input.GetMouseButtonDown(1))
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
		BattleStateManager.me.gooseAnimator.SetTrigger("Grenade");
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        bubble.MoveToPos(BattleStateManager.me.target.transform.position, 1f, bubbleCurve);
        yield return new WaitForSeconds(0.99f);
		BattleStateManager.me.parts[0].Play();
        for (int i = BattleStateManager.me.enemies.Count - 1; i >= 0; i--)
        {
            Enemy enemy = BattleStateManager.me.enemies[i];
            enemy.SubtractHealth(bubble.damage, damageType);
        }
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
