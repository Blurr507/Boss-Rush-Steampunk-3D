using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oilmancer : Enemy
{
    [SerializeField]
    private GameObject minion, attack1, block1, attack2;
    public int attack1Damage = 50;
    public int block1Fail = 0;
    public int block1Target = -35;
    public int block1Crit = -50;
    public int damageType1 = 3;
    public int attack2Damage = 50;
    public int damageType2 = 1;
    public int maxAttacks = 1, attacks = 1;
    public int spawnPreps = 1, spawns = 0;
    public List<OilmancerMinion> minions = new List<OilmancerMinion>();
    public List<Transform> minionLocations = new List<Transform>();
    public AnimationCurve posCurve;
    private Animator anim;

	private int turn;

	public ParticleSystem fire;

    private void Start()
    {
        Random.InitState(0);
        StartOverride();
    }

    public override void StartOverride()
    {
        base.StartOverride();
        anim = GetComponent<Animator>();
    }

    public override void DoTurn()
    {
        BattleStateManager.me.lastEnemy = this;
        turns--;

        if (turns <= 0)
        {
            BattleStateManager.me.IncrementCurrentEnemy();
        }

        if(effects.Contains("burning"))
        {
            //  If we're on fire, put it out
            StartCoroutine(PutOutFire());
			turn--;
        }
        else if(minions.Count < minionLocations.Count && spawns > 0)
        {
            //  If we're not burning, and we aren't at max minion capacity, and we have more spawns available this turn then spawn a minion
            StartCoroutine(SpawnMinion());
			turn++;
        }
        else if(turns > 0 && attacks > 0)
        {
            //  If it's not the last turn, and we have more attacks this turn, then attack
            Attack();
			turn++;
        }
        else if(attacks > 0)
        {
            Attack();
			turn++;
        }
        else
        {
            turns = 2;
            base.DoTurn();
        }
    }

    private void Attack()
    {
        //  75% chance to use regular attack
        float randomFloat = Random.Range(0f, 1f);
        Debug.Log(randomFloat);
		if (turn % 4 != 0)
        {
            StartCoroutine(Attack1());
        }
        else
        {
            StartCoroutine(Attack2());
        }
    }

    private IEnumerator Attack1()
    {
        attacks--;
        //  State = 4
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(attack1);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
        bubble.AddDamage(attack1Damage);
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
        GameObject block = Instantiate(block1);
        SteamGauge gauge = block.GetComponentInChildren<SteamGauge>();
        yield return new WaitForSeconds(0.5f);
        gauge.Spin();
        while(true)
        {
            if(gauge.result != -1)
            {
                if(block1Fail != 0 || gauge.result > 0)
                {
                    SmallDamage smallDamage = gauge.GetComponent<CreateObjectInBounds>().CreateObject().GetComponent<SmallDamage>();
                    switch (gauge.result)
                    {
                        case 0:
                            smallDamage.damage = block1Fail;
                            break;
                        case 1:
                            smallDamage.damage = block1Target;
                            break;
                        case 2:
                            smallDamage.damage = block1Crit;
                            break;
                    }
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(1f);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        BattleStateManager.me.IncrementState();
        // State = 6
        bubble.MoveToPos(target.transform.position, 1, posCurve);
        yield return new WaitForSeconds(1f);
        HurtTarget(bubble.damage, damageType1);
        Destroy(attack);
        Destroy(block);
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
    }

    private IEnumerator Attack2()
    {
        attacks--;
        //  State = 4
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(attack2);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
        Transform oilBlob = attack.transform.GetChild(0);
        GameObject fire = attack.transform.GetChild(0).GetChild(0).gameObject;
        bubble.AddDamage(attack2Damage);
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        // State = 6
        yield return new WaitForSeconds(0.5f);
        fire.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bubble.MoveToPos(target.transform.position, 1, posCurve);
        Vector3 blobStart = oilBlob.position;
        for(float t = 0; t < 0.99f; t += Time.deltaTime)
        {
            oilBlob.position = Vector3.Lerp(blobStart, target.transform.position, posCurve.Evaluate(t / 0.99f));
            yield return new WaitForEndOfFrame();
        }
        HurtTarget(bubble.damage, damageType2);
        FindObjectOfType<Geaux>().AddEffect("burning");
        Destroy(attack);
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
    }

    private IEnumerator SpawnMinion()
    {
        spawns--;
        spawnPreps--;
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < minionLocations.Count; i++)
        {
            //  Loop through the minion spawns and look for one that doesn't have a child (minion)
            if (minionLocations[i].childCount == 0)
            {
                //  Once it's found create a minion there and break the loop
                Transform spawn = minionLocations[i];
                SmoothRandomBobbingAndRotation bob = Instantiate(minion, spawn.position, spawn.rotation, spawn).GetComponent<SmoothRandomBobbingAndRotation>();
                bob.SetInitialPositionAndRotation();
                break;
            }
        }
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
    }

    private IEnumerator PutOutFire()
    {
        yield return new WaitForSeconds(1f);
        effects.Remove("burning");
        UpdateAnimatorBools();
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
    }

    public override bool AddEffect(string effect)
    {
        switch(effect)
        {
            case "burning":
                if(!effects.Contains(effect))
                {
                    effects.Add(effect);
					fire.Play ();
                    UpdateAnimatorBools();
                    return true;
                }
                break;
        }
        return false;
    }

    public override void UpdateAnimatorBools()
    {
		//uhh
    }

    public override void ResetTurns()
    {
        turns = maxTurns;
        attacks = maxAttacks;
        if(minions.Count < minionLocations.Count)
        {
            if(spawnPreps == 0)
            {
                spawnPreps++;
            }
            else
            {
                spawns = 1;
            }
        }
    }
}
