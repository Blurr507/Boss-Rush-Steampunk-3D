using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierSkillCheck : MonoBehaviour
{
    private Chandelier chandelier;
    private int stage = 0;
    private int clicked = 0;
    
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
        BattleStateManager.me.IncrementState();

        while (true)
        {
            if (clicked == 1)
            {
                Debug.Log(Vector3.SignedAngle(Vector3.down, chandelier.transform.position - chandelier.hinge.position, Vector3.forward));
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
