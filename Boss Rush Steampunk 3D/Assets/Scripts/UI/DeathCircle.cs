using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCircle : MonoBehaviour
{
    public float minSize = 0.1f;
    public float maxSize = 30.0f;
    public float time = 2;
    public AnimationCurve sizeCurve;
    private float timer = 0;

    void Update()
    {
        if (timer < time)
        {
            timer += Time.deltaTime;
            if (timer < time)
            {
                transform.localScale = Vector3.one * Mathf.Lerp(minSize, maxSize, sizeCurve.Evaluate(timer / time));
            }
            else
            {
                transform.localScale = Vector3.one * maxSize;
            }
        }
    }
}
