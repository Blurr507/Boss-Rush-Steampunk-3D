using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spider : Enemy
{
    public int attack1Damage = 30;
    public int damageType = 0;
    public GameObject attack1;
    public AnimationCurve posCurve;

	public Animator spiderstuff;
	public ParticleSystem fireparts;

	public Transform[] lazerspots;
	public LineRenderer[] lazers;

	public int turn;

	public override void StartOverride()
	{
		base.StartOverride();
		Random.InitState(10);
	}

	public override void DoTurn()
    {
		turn++;
        BattleStateManager.me.lastEnemy = this;
        if (turn % 3 == 0){
        Attack2();
		} else {
		Attack1();
		}
        turns--;
        if (turns <= 0)
        {
            BattleStateManager.me.IncrementCurrentEnemy();
        }
    }
    public void Attack1()
    {
        StartCoroutine(Attack1Co());
    }

	public void Attack2()
	{
		StartCoroutine(Attack2Co());
	}

	private void Update(){
		ManageHealth();
		for(int i = 0; i < 4; i++){
			lazers[i].SetPosition(1, lazerspots[i].position);
		}
	}

    private IEnumerator Attack1Co()
    {
        //  State = 4
		spiderstuff.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(attack1);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
		bubble.AddDamage((int)(attack1Damage * 1.6f));
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();

        // State = 6
        bubble.MoveToPos(target.transform.position, 1, posCurve);
        yield return new WaitForSeconds(1f);
		fireparts.Play();
		HurtTarget((int)(attack1Damage * 1.6f), damageType);
        Destroy(attack);
        BattleStateManager.me.IncrementState();
    }
	private IEnumerator Attack2Co()
	{
		//  State = 4
		spiderstuff.SetTrigger("attack2");
		yield return new WaitForSeconds(0.5f);
		GameObject attack = Instantiate(attack1);
		DamageBubble bubble = FindObjectOfType<DamageBubble>();
		bubble.AddDamage(attack1Damage);
		yield return new WaitForSeconds(0.25f);
		BattleStateManager.me.IncrementState();
		BattleStateManager.me.IncrementState();
		
		// State = 6
		bubble.MoveToPos(target.transform.position, 2.25f, posCurve);
		yield return new WaitForSeconds(2.25f);
		//fireparts.Play();
		HurtTarget(attack1Damage, damageType);
		Destroy(attack);
		BattleStateManager.me.IncrementState();
	}
}
