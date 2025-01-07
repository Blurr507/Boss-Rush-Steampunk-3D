using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CreateObjectInBounds))]
public class SixShooterAttack : MonoBehaviour
{
    public SteamGauge gauge1;
    public SteamGauge gauge2;
    public SteamGauge gauge3;
    public SteamGauge gauge4;
    public SteamGauge gauge5;
    public SteamGauge gauge6;
    public DamageBubble bubble;
    public AnimationCurve bubbleCurve;
    private int stage = 0;
    public int failDamage = 5;  // The damage done if the spinner stops outside of the target angle
    public int hitDamage = 20; // The damage done if the spinner stops in the target angle
    public int critDamage = 50; // The damage done if the spinner stops in the target angle
    public int damageType = 2; // The type of damage done by this attack
    private CreateObjectInBounds create;

    private int currentGaugeIndex = 0; // Tracks which gauge is currently spinning
    private SteamGauge[] gauges; // Array to store all the gauges
    private List<int> results = new List<int>(); // Store the results of all gauges

    void Start()
    {
        bubble = GetComponentInChildren<DamageBubble>();
        create = GetComponent<CreateObjectInBounds>();

        // Initialize the gauges array
        gauges = new SteamGauge[] { gauge1, gauge2, gauge3, gauge4, gauge5, gauge6 };

        // Start the process with the first gauge
        SpinGauge(currentGaugeIndex);
    }

    void Update()
    {
        if (stage == 0 && currentGaugeIndex < gauges.Length)
        {
            SteamGauge currentGauge = gauges[currentGaugeIndex];

            // Check if the current gauge has a result
            if (currentGauge.result != -1)
            {
                results.Add(currentGauge.result);

                // Handle the damage based on the result
                SmallDamage damage = create.CreateObject().GetComponent<SmallDamage>();
                switch (currentGauge.result)
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

                // Move to the next gauge
                currentGaugeIndex++;
                if (currentGaugeIndex < gauges.Length)
                {
                    SpinGauge(currentGaugeIndex);
                }
                else
                {
                    // All gauges have been processed
                    stage++;
                }
            }
        }
        else if (stage == 1)
        {
            Invoke("Move", 2f);
            stage++;
        }
    }

    private void SpinGauge(int index)
    {
        gauges[index].Spin();
    }

    private void Move()
    {
        BattleStateManager.me.IncrementState();
        bubble.MoveToPos(BattleStateManager.me.GetTarget().transform.position, 1f, bubbleCurve);
        Invoke("Attack", 1f);
    }

    private void Attack()
    {
        // Calculate total damage from the results
        int totalDamage = 0;
        foreach (int result in results)
        {
            switch (result)
            {
                case 0:
                    totalDamage += failDamage;
                    break;
                case 1:
                    totalDamage += hitDamage;
                    break;
                case 2:
                    totalDamage += critDamage;
                    break;
            }
        }

        bubble.damage = totalDamage;
        BattleStateManager.me.HurtTarget(bubble.damage, damageType);
        Invoke("Done", 1f);
    }

    private void Done()
    {
        BattleStateManager.me.IncrementState();
        Destroy(gameObject);
    }
}