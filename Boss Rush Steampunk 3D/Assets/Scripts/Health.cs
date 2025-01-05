using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int health;
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
        health += num;
    }

    public void SubtractHealth(int num)
    {
        health -= num;
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
        if(hp.gameObject != null)
        {
            Destroy(hp.parent.gameObject);
            Destroy(hp.gameObject);
        }
    }
}
