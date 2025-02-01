using System.Collections;
using TMPro;
using UnityEngine;

public class ShieldBubble : MonoBehaviour
{
    private TextMeshProUGUI text;   //  Text object that displays the current damage
    public int shield;              //  The currently stored amount of damage
    private Coroutine move;

    void Start()
    {
        //  Set text equal to the TMP object in this object's children
        text = GetComponentInChildren<TextMeshProUGUI>();
        if (text == null) Debug.Log(text);
        //  Make the text invisible if the current damage is 0
        text.gameObject.SetActive(shield != 0);
        //  Set parent to the world space camera (for cases where it's instantiated as a child of a skillcheck object)
        transform.SetParent(GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform);
    }

    //  Add damage to the total
    public void AddShield(int num)
    {
        //  Make sure that text is assigned
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();
        //  Add the damage, and assume that it's a positive amount, so we want the text to be visible
        shield += num;
        text.gameObject.SetActive(true);
        //  Update the text
        text.text = $"{shield}";
    }

    public void MoveToPos(Vector3 endPos, float time, AnimationCurve posCurve, bool destroyOnArrival = true)
    {
        if (move != null)
        {
            StopCoroutine(move);
        }
        move = StartCoroutine(MoveToPosition(endPos, time, posCurve, destroyOnArrival));
    }

    private IEnumerator MoveToPosition(Vector3 endPos, float time, AnimationCurve posCurve, bool destroyOnArrival)
    {
        Vector3 startPos = transform.position;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            //  Lerp position based on curve
            transform.position = startPos + (endPos - startPos) * posCurve.Evaluate(t / time);
            yield return new WaitForEndOfFrame();
        }

        if (destroyOnArrival)
        {
            //  Once we've reached the position, if we want to destroy, then destroy
            Destroy(gameObject);
        }
        else
        {
            //  If not, then set position precisely to endPos
            transform.position = endPos;
        }
    }

    public void Reset()
    { //reset damage
        shield = 0;
        text.gameObject.SetActive(false);
    }
}
