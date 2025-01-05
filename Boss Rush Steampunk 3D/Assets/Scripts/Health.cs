using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health, maxHealth;
    public UnityEvent die;
    public bool alive = true;
    public RectTransform hp;

    private void Start()
    {
        if(die.GetPersistentEventCount() == 0)
        {
            die.AddListener(Die);
        }
    }
    void Update()
    {
        hp.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
        if(health <= 0 && alive)
        {
            die.Invoke();
            alive = false;
        }
    }

    public void AddHealth(int num)
    {
        health = Mathf.Min(health + num, maxHealth);
    }

    public void SubtractHealth(int num)
    {
        health = Mathf.Max(health - num, 0);
    }

    public int GetHealth()
    {
        return health;
    }


    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(hp != null && hp.gameObject != null)
        {
            Destroy(hp.parent.gameObject);
            Destroy(hp.gameObject);
        }
    }
}
