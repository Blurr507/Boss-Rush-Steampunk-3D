using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierSkillCheck : SkillCheck
{
    private Chandelier chandelier;
    private int stage = 0;
    private int clicked = 0;
    public float failAngle = -20, failAngleMargin = 19;
    public float targetAngle = 30, targetAngleMargin = 5.5f;
    public float critAngle = 2, critAngleMargin = 3;
    public int critDamage = 10;
    
    
    void Start()
    {
        StartCoroutine(DoStuff());
    }

    private void Update()
    {
        if(clicked == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clicked = 1;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                clicked = 2;
            }
        }
    }

    private IEnumerator DoStuff()
    {
        chandelier = FindObjectOfType<Chandelier>();
        chandelier.critDamage = critDamage;
        BattleStateManager.me.IncrementState();
        chandelier.showColors = true;

        while (true)
        {
            if (clicked == 1)
            {
                chandelier.GetComponent<DistanceJoint2D>().enabled = false;
                chandelier.cut = true;
                stage++;
                clicked = -1;
                break;
            }
            else if(clicked == 2)
            {
                //  If we right click before finishing, then cancel
                Cancel();
                chandelier.showColors = false;
                Destroy(gameObject);
                BattleStateManager.me.BackToState0();
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        Destroy(gameObject);
    }    
}
