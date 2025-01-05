using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanSelect))]
public class Enemy : MonoBehaviour
{
    [HideInInspector]public List<UnityEvent> attacks = new List<UnityEvent>();
    public CanSelect target;
    public int turns = 1, maxTurns = 1;

    public void DoTurn()
    {
        attacks[0].Invoke();
        turns--;
    }

    public void ResetTurns()
    {
        turns = maxTurns;
    }

    public void HurtTarget(int damage)
    {
        target.health.SubtractHealth(damage);
    }

    private void OnDestroy()
    {
        BattleStateManager.me.enemies.Remove(this);
    }
}
