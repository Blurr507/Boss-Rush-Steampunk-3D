using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanSelect))]
public class Enemy : MonoBehaviour  //  A base class for all enemies which will be inherited from by additional enemy's
{
    public CanSelect target;
    public int turns = 1, maxTurns = 1;

    //  Default turn script for all enemies. Can be overriden in scritpts that inherit
    public virtual void DoTurn()
    {
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        turns--;
    }

    public void ResetTurns()
    {
        turns = maxTurns;
    }

    public void HurtTarget(int damage, int damageType = 0)
    {
        target.health.SubtractHealth(damage, damageType);
    }

    private void OnDestroy()
    {
        BattleStateManager.me.enemies.Remove(this);
    }
}
