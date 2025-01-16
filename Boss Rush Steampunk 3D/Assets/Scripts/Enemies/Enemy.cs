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

    //  Default turn script for all enemies. Can be overwritten in scritpts that inherit
    public virtual void DoTurn()
    {
        //  By default, drop the turns by 1, and if we're out of turns, then tell the BattleStateManager to go to the next enemy
        turns--;
        if (turns <= 0)
        {
            BattleStateManager.me.IncrementCurrentEnemy();
        }

        //  Increment the BattleStateManager 3 times to end our turn
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
    }

    //  Default reset script for all enemies. Can be overwritten in scripts that inherit
    public virtual void ResetTurns()
    {   
        //  By default, set turns back to maxTurns
        turns = maxTurns;
    }

    //  Deals 'damage' damage of type 'damageType' to our target's health
    public void HurtTarget(int damage, int damageType = 0)
    {
        target.GetComponent<Health>().SubtractHealth(damage, damageType);
    }

    private void OnDestroy()
    {
        //  Call the OnDestroyOverride code
        OnDestroyOverride();
    }

    public override void OnDestroyOverride()
    {
        //  When getting destroyed, remove this object from the BattleStateManager's enemies and selectables list
        BattleStateManager.me.enemies.Remove(this);
        BattleStateManager.me.selectables.Remove(GetComponent<CanSelect>());
        //  Perform the default OnDestroy code
        base.OnDestroyOverride();
    }

    //  An overridable method for all enemies to have for managing their animations
    public virtual void UpdateAnimatorBools()
    {

    }
}
