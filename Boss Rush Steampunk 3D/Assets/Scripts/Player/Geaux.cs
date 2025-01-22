using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geaux : Health
{
    public int burnDamage = 10;
    public GameObject blackCircleOfDeath;
    public GameObject firePart;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartOverride();
    }

    public override bool AddEffect(string effect)
    {
        switch(effect)
        {
            case "burning":
                if (!effects.Contains("burning"))
                {
                    effects.Add("burning");
                    firePart.SetActive(true);
                    return true;
                }
                break;
        }
        return false;
    }

    public void Burn()
    {
        if(effects.Contains("burning"))
        {
            SubtractHealth(burnDamage, 1);
        }
    }

    public void Extinguish()
    {
        if (effects.Contains("burning"))
        {
            effects.Remove("burning");
            firePart.SetActive(false);
        }
    }

    public override void Die()
    {
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        Instantiate(blackCircleOfDeath);
        GameObject.FindGameObjectWithTag("WorldSpaceCanvas").SetActive(false);
        BattleStateManager.me.ToState7();
        anim.SetTrigger("Dead");
        yield return new WaitForSeconds(4f);
        BattleStateManager.me.RestartScene();
    }
}
