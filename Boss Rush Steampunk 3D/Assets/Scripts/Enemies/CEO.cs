using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Android;

public class CEO : Enemy
{
    private int turnIndex;
    public GameObject ionCanonAttack;
    public int ionCanonDamage = 150;
    public int smgDamage = 75;
    public int dashDamage = 50;
    public AnimationCurve posCurve;
    public GameObject guns;
    private Animator anim;

	public ParticleSystem[] parts;

	public bool gunsDestroyed;

	public AudioSource[] Musik;


    public override void StartOverride()
    {
		gunsDestroyed = false;
        base.StartOverride();
        Random.InitState(10);
        anim = GetComponent<Animator>();
    }

	void FixedUpdate(){
		if(gunsDestroyed){
			parts[1].Play();
		}
	}

    public override void DoTurn()
    {
        if (turns > 0)
        {
            BattleStateManager.me.lastEnemy = this;
            turns--;
            if (turns <= 0)
            {
                BattleStateManager.me.IncrementCurrentEnemy();
            }
            if (guns != null)
            {
                if (turnIndex == 0)
                {
                    StartCoroutine(FireIonCanon());
                }
                else
                {
                    StartCoroutine(FireSMG());
                }
                IncrementTurn();
            }
            else
            {
                StartCoroutine(Dash());
            }
        }
        else
        {
            //  If we're already out of turns, then skip this turn
            BattleStateManager.me.IncrementCurrentEnemy();
            BattleStateManager.me.IncrementState();
            BattleStateManager.me.IncrementState();
            BattleStateManager.me.IncrementState();
        }
    }

    private IEnumerator FireIonCanon()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(ionCanonAttack);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
        bubble.AddDamage(ionCanonDamage);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        bubble.MoveToPos(target.transform.position, 1, posCurve);
        yield return new WaitForSeconds(1f);
        HurtTarget(ionCanonDamage);
        yield return new WaitForSeconds(0.5f);
        Destroy(attack);
        BattleStateManager.me.IncrementState();
    }

    private IEnumerator FireSMG()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(ionCanonAttack);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
        bubble.AddDamage(smgDamage);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        bubble.MoveToPos(target.transform.position, 1f, posCurve);
        yield return new WaitForSeconds(1f);
        HurtTarget(smgDamage);
        yield return new WaitForSeconds(0.75f);
        Destroy(attack);
        BattleStateManager.me.IncrementState();
    }

    private IEnumerator Dash()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(ionCanonAttack);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
        bubble.AddDamage(dashDamage);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Dash");
        bubble.MoveToPos(target.transform.position, 0.83f, posCurve);
        yield return new WaitForSeconds(0.83f);
        HurtTarget(dashDamage);
        yield return new WaitForSeconds(0.5f);
        Destroy(attack);
        anim.ResetTrigger("Dash");
        BattleStateManager.me.IncrementState();
    }

    private void IncrementTurn()
    {
        if(turnIndex < 3)
        {
            turnIndex++;
        }
        else
        {
            turnIndex = 0;
        }
    }

    public override void Die()
    {
        if(guns != null)
        {
            StartCoroutine(DestroyGuns());
        }
        else
        {
            base.Die();
        }
    }

    private IEnumerator DestroyGuns()
    {
		gunsDestroyed = true;
		parts[0].Play();
        BattleStateManager.me.paused = true;
        yield return new WaitForSeconds(1f);
        Destroy(guns);
        alive = true;
        hp.gameObject.SetActive(true);
        AddHealth(GetMaxHealth());
		Musik[0].volume = 0f;
		Musik[1].volume = 1f;
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.paused = false;
        BattleStateManager.me.IncrementState();
    }
}
