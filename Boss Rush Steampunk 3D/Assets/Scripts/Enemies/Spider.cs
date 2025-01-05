using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spider : Enemy
{
    public int attack1Damage = 30;
    public GameObject attack1;
    public AnimationCurve posCurve;

    private void Start()
    {
        attacks.Add(new UnityEvent());
        attacks[0].AddListener(Attack1);
    }

    public void Attack1()
    {
        StartCoroutine(Attack1Co());
    }

    private IEnumerator Attack1Co()
    {
        //  State = 4
        yield return new WaitForSeconds(0.5f);
        GameObject attack = Instantiate(attack1);
        DamageBubble bubble = FindObjectOfType<DamageBubble>();
        bubble.AddDamage(attack1Damage);
        yield return new WaitForSeconds(0.5f);
        BattleStateManager.me.IncrementState();
        BattleStateManager.me.IncrementState();
        // State = 6
        bubble.MoveToPos(target.transform.position, 1, posCurve);
        yield return new WaitForSeconds(1f);
        HurtTarget(attack1Damage);
        Destroy(attack);
        BattleStateManager.me.IncrementState();
    }
}
