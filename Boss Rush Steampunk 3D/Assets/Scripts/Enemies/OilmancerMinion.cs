using System.Collections;
using UnityEngine;

public class OilmancerMinion : Enemy
{
    public int healAmount = 20;
    private Animator anim;
    private Oilmancer oilmancer;

    public override void StartOverride()
    {
        base.StartOverride();
        if (!BattleStateManager.me.enemies.Contains(this))
        {
            BattleStateManager.me.selectables.Add(GetComponent<CanSelect>());
            BattleStateManager.me.enemies.Add(this);
            GetComponent<CanSelect>().canSelect = true;
        }
        if (!effects.Contains("oiled"))
        {
            effects.Add("oiled");
        }
        anim = GetComponent<Animator>();
        UpdateAnimatorBools();
    }

    public override void DoTurn()
    {
        turns--;
        if (turns <= 0)
        {
            BattleStateManager.me.IncrementCurrentEnemy();
        }

        if (effects.Contains("burning"))
        {
            StartCoroutine(PutOutFire());
        }
        else if(!effects.Contains("oiled"))
        {
            StartCoroutine(OilSelf());
        }
        else
        {
            oilmancer = FindObjectOfType<Oilmancer>();
            if(oilmancer != null)
            {
                StartCoroutine(HealOilmancer());
            }
            else
            {
                turns++;
                base.DoTurn();
            }
        }
    }

    private IEnumerator PutOutFire()
    {
        yield return new WaitForSeconds(1f);
        effects.Remove("burning");
        effects.Remove("oiled");
        UpdateAnimatorBools();
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
    }
    private IEnumerator OilSelf()
    {
        yield return new WaitForSeconds(1f);
        effects.Add("oiled");
        UpdateAnimatorBools();
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
    }

    private IEnumerator HealOilmancer()
    {
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        oilmancer.AddHealth(healAmount);
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
    }

    public override void UpdateAnimatorBools()
    {
        anim.SetBool("Burning", effects.Contains("burning"));
        anim.SetBool("Oiled", effects.Contains("oiled"));
    }
}
