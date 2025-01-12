using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierSkillCheck : MonoBehaviour
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
        if(clicked == 0 && Input.GetMouseButtonDown(0))
        {
            clicked = 1;
        }
    }

    private IEnumerator DoStuff()
    {
        chandelier = FindObjectOfType<Chandelier>();
        chandelier.critDamage = critDamage;
        BattleStateManager.me.IncrementState();

        while (true)
        {
            if (clicked == 1)
            {
                float angle = Vector3.SignedAngle(Vector3.down, chandelier.transform.position - chandelier.hinge.position, Vector3.forward);
                if(Mathf.Abs(angle - critAngle) < critAngleMargin)
                {
                    Debug.Log($"Critical hit! Angle was {angle}");
                    chandelier.result = 3;
                    
                }
                else if(Mathf.Abs(angle - targetAngle) < targetAngleMargin)
                {
                    Debug.Log($"Successful hit! Angle was {angle}");
                    chandelier.result = 2;
                }
                else if (Mathf.Abs(angle - failAngle) < failAngleMargin)
                {
                    Debug.Log($"You're a failure! Angle was {angle}");
                    chandelier.result = 1;
                }
                else
                {
                    Debug.Log($"Well that was boring :( ... Your angle was {angle}");
                    chandelier.result = 0;
                }

                chandelier.GetComponent<DistanceJoint2D>().enabled = false;
                stage++;
                clicked = -1;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        BattleStateManager.me.IncrementState();
        Destroy(gameObject);
    }    
}
