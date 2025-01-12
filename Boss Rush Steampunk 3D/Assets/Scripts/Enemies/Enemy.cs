using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanSelect))]
public class Enemy : Health  //  A base class for all enemies which will be inherited from by additional enemy's
{
    public CanSelect target;
    public int turns = 1, maxTurns = 1;

    //  Default turn script for all enemies. Can be overriden in scritpts that inherit
    public virtual void DoTurn()
    {
        turns--;
        if (turns <= 0)
        {
            BattleStateManager.me.IncrementCurrentEnemy();
        }

        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
    }

    public virtual void ResetTurns()
    {
        turns = maxTurns;
    }

    public void HurtTarget(int damage, int damageType = 0)
    {
        target.GetComponent<Health>().SubtractHealth(damage, damageType);
    }

    public override void OnDestroyOverride()
    {
        BattleStateManager.me.enemies.Remove(this);
        BattleStateManager.me.selectables.Remove(GetComponent<CanSelect>());
        base.OnDestroyOverride();
    }

    public virtual void UpdateAnimatorBools()
    {

    }
}
