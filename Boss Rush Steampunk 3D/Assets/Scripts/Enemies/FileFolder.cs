using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileFolder : Enemy
{
    private Animator anim;
    private Kevin kevin;
    public GameObject attack;
    public AnimationCurve posCurve1, posCurve2;
    public int damage = 5;
    public bool leader = false;

    private void Awake()
    {
        if (FindObjectsOfType<FileFolder>().Length <= 1)
        {
            leader = true;
        }
        BattleStateManager.me.selectables.Add(GetComponent<CanSelect>());
        BattleStateManager.me.enemies.Add(this);
        GetComponent<CanSelect>().canSelect = true;
    }

    private void Start()
    {
        StartOverride();
    }

    public override void StartOverride()
    {
        anim = GetComponent<Animator>();
        base.StartOverride();
        AddEffect("oiled");
        //  Find the kevin in the scene, add self to his files, and set his buttons to our buttons
        kevin = FindObjectOfType<Kevin>();
        kevin.files.Add(this);
        GetComponent<CanSelect>().buttons = kevin.GetComponent<CanSelect>().buttons;
        UpdateAnimatorBools();
        turns = 0;
        target = FindObjectOfType<Geaux>().GetComponent<CanSelect>();
    }

    public override void DoTurn()
    {
        if (leader && turns > 0)
        {
            turns--;
            if (turns <= 0)
            {
                BattleStateManager.me.IncrementCurrentEnemy();
            }
            //  What to do this turn
            StartCoroutine(Attack());
        }
        else
        {
            //  If we're already out of turns or not the leader, then skip this turn
            BattleStateManager.me.IncrementCurrentEnemy();
            BattleStateManager.me.IncrementState();
            BattleStateManager.me.IncrementState();
            BattleStateManager.me.IncrementState();
        }
    }

    public override bool AddEffect(string effect)
    {
        switch (effect)
        {
            case "burning":
                if (effects.Contains("oiled") && !effects.Contains("burning"))
                {
                    effects.Add(effect);
                    UpdateAnimatorBools();
                    return true;
                }
                break;
            case "oiled":
                if (!effects.Contains("burning") && !effects.Contains("oiled"))
                {
                    effects.Add(effect);
                    UpdateAnimatorBools();
                    return true;
                }
                break;
        }
        return false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        List<DamageBubble> damageBubbles = new List<DamageBubble>();
        foreach(FileFolder file in kevin.files)
        {
            DamageBubble bubble = Instantiate(attack, file.transform).GetComponentInChildren<DamageBubble>();
            bubble.MoveToPos(bubble.transform.position + Vector3.up, 1.5f, posCurve1, false);
            damageBubbles.Add(bubble);
        }
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(1.5f);
        foreach(DamageBubble bubble in damageBubbles)
        {
            bubble.MoveToPos(target.transform.position, 1f, posCurve2);
        }
        yield return new WaitForSeconds(0.99f);
        for(int i = 0; i < damageBubbles.Count; i++)
        {
            HurtTarget(damage);
        }
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
    }

    private void OnDestroy()
    {
        OnDestroyOverride();
    }

    public override void OnDestroyOverride()
    {
        base.OnDestroyOverride();
        if (kevin != null)
        {
            kevin.files.Remove(this);
        }
    }
}
