using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class OilmancerMinion : Enemy
{
    public int healAmount = 20;
    private Animator anim;
    private Oilmancer oilmancer;

    private void Start()
    {
        StartOverride();
    }

    public override void StartOverride()
    {
        anim = GetComponent<Animator>();
        base.StartOverride();
        if (!BattleStateManager.me.enemies.Contains(this))
        {
            BattleStateManager.me.selectables.Add(GetComponent<CanSelect>());
            BattleStateManager.me.enemies.Add(this);
            GetComponent<CanSelect>().canSelect = true;
        }
        AddEffect("oiled");
        //  Find the oilmancer in the scene, add self to his minions, and set his buttons to our buttons
        oilmancer = FindObjectOfType<Oilmancer>();
        oilmancer.minions.Add(this);
        GetComponent<CanSelect>().buttons = oilmancer.GetComponent<CanSelect>().buttons;
        UpdateAnimatorBools();
        turns = 0;
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
                turns = 2;
                base.DoTurn();
            }
        }
    }

    public override bool AddEffect(string effect)
    {
        switch(effect)
        {
            case "burning":
                if(effects.Contains("oiled") && !effects.Contains("burning"))
                {
                    effects.Add(effect);
                    UpdateAnimatorBools();
                    return true;
                }
                break;
            case "oiled":
                if(!effects.Contains("burning") && !effects.Contains("oiled"))
                {
                    effects.Add(effect);
                    UpdateAnimatorBools();
                    return true;
                }
                break;
        }
        return false;
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

    private void OnDestroy()
    {
        OnDestroyOverride();
    }

    public override void OnDestroyOverride()
    {
        base.OnDestroyOverride();
        if(oilmancer != null)
        {
            oilmancer.minions.Remove(this);
        }
    }
}
