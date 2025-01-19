using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanse : SkillCheck
{
    void Start()
    {
        StartCoroutine(DoStuff());
    }

    private IEnumerator DoStuff()
    {
        BattleStateManager.me.IncrementState();
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<Geaux>().Extinguish();
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        Destroy(gameObject);
    }
}
