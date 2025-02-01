using TMPro;
using UnityEngine;

public class SmallShield : MonoBehaviour
{
    public int shield;                  //  The amount of damage
    public float time = 1;              //  How long it takes to move towards the DamageBubble
    public AnimationCurve posCurve;     //  A curve that decides how it moves towards the bubble over time (in seconds)
    private Vector3 startPos, endPos;   //  Self-explanitory
    private float timer = 0;            //  A timer to keep track of where the object should currently be/how long it's existed
    private ShieldBubble bubble;        //  A reference to the target DamageBubble (there should only be one, so this could potentially be removed)

    void Start()
    {
        //  Set the TMP on this object's text to the damage
        GetComponent<TextMeshProUGUI>().text = $"{shield}";
        //  Set the bubble to the only existing bubble
        bubble = FindObjectOfType<ShieldBubble>();
        startPos = transform.position;
        endPos = bubble.transform.position;
    }

    void Update()
    {
        if (timer < time)
        {
            //  If timer is less than time, lerp position based on the curve towards the endPos and increment timer
            transform.position = startPos + (endPos - startPos) * posCurve.Evaluate(timer / time);
            timer += Time.deltaTime;
        }
        else
        {
            //  Once we've reached the position, add our damage to the bubble and destroy this object
            bubble.AddShield(shield);
            Destroy(gameObject);
        }
    }
}
