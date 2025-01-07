using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DamageNumber : MonoBehaviour
{
    public int damage;
    public float speed = 1;
    public float wobbleAmplitude = 0.2f;
    public float wobbleSpeed = 0.5f;
    private float wobblePos = 0, wobble;
    public Vector3 direction = Vector3.up; 
    public float life = 1;
    public float startFade = 0.5f;
    public Color color = Color.white;
    private TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = damage.ToString();
        text.color = color;
    }

    void Update()
    {
        wobblePos += wobbleSpeed * Time.deltaTime;
        float newWobble = (Mathf.PerlinNoise1D(wobblePos) * wobbleAmplitude) - (wobbleAmplitude / 2);
        transform.position += direction * speed * Time.deltaTime + Vector3.Cross(direction, transform.position - Camera.main.transform.position).normalized * (newWobble - wobble);
        wobble = newWobble;
        text.color = SetAlpha(text.color, Mathf.Min(life, startFade) / startFade);
        life -= Time.deltaTime;
        if(life <= 0)
        {
            Destroy(gameObject);
        }
    }

    private Color SetAlpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
