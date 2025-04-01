using System.Collections;
using UnityEngine;

public class Geaux : Health
{
    public int burnDamage = 10;
    public GameObject blackCircleOfDeath;
    public GameObject firePart;
    private Animator anim;
    public int shield = 0;
    public RectTransform shieldBar, reflectBar;
    public GameObject reflect;
    public AnimationCurve posCurve;

	public AudioSource timetravel;

	public AudioSource normalMusic;

	private ParticleSystem deathparts;

    public static Geaux main;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartOverride();
        deathparts = GetComponent<ParticleSystem>();
    }

    public override void ManageHealth()
    {
        //  Scale the healthbar based on the amount of health that we have versus our maxHealth
        if (alive)
        {
            UpdateHealthText();
            float healthRatio = Mathf.Clamp((float)GetHealth() / GetMaxHealth(), 0, 1);
            hp.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthbarScaleMultiplier * healthRatio);
            shieldBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthbarScaleMultiplier * healthRatio * Mathf.Clamp(shield, 0, 100) / 100);
            reflectBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthbarScaleMultiplier / 5 * healthRatio * Mathf.Clamp(shield - 100, 0, 20) / 20);
            reflectBar.localPosition = hp.localPosition + (hp.right * healthbarScaleMultiplier * healthRatio);
            if (GetHealth() <= 0)
            {
                //  If we're out of health and still alive, call our death event, and set alive to false (to prevent the death event from being called more than once)
                Die();
                alive = false;
            }
        }
        else
        {
            hp.gameObject.SetActive(false);
        }
    }

    //  Subtracts 'num' health of type 'type'
    public override void SubtractHealth(int num, int type = 0)
    {
        if (shield > 100)
        {
            StartCoroutine(Reflect(num * (shield - 100) / 100));
        }
        //  Scale num based on shield
        num = Mathf.FloorToInt(num * Mathf.Clamp(100 - shield, 0, 100) / 100f);
        //  Reduce health by num down to a minimum of 0
        SetHealth(Mathf.Max(GetHealth() - num, 0));
        //  Create a DamageNumber, and save a reference of it so that we can assign its damage
        DamageNumber damageNumber = damageNumberCreator.CreateObject().GetComponent<DamageNumber>();
        damageNumber.damage = num;
        switch (type)
        {
            //  Set the color of the damageNumber to the appropriate type's color
            case -1:
                damageNumber.color = BattleStateManager.me.healDamage;
                break;
            case 0:
                damageNumber.color = BattleStateManager.me.baseDamage;
                break;
            case 1:
                damageNumber.color = BattleStateManager.me.fireDamage;
                break;
            case 2:
                damageNumber.color = BattleStateManager.me.electricDamage;
                break;
            case 3:
                damageNumber.color = BattleStateManager.me.oilDamage;
                break;
            default:
                damageNumber.color = BattleStateManager.me.baseDamage;
                break;
        }
    }

    private IEnumerator Reflect(int damage)
    {
        BattleStateManager.me.paused = true;
        Enemy attacker = BattleStateManager.me.lastEnemy;
        GameObject reflectObject = Instantiate(reflect);
        DamageBubble bubble = reflectObject.GetComponentInChildren<DamageBubble>();
        bubble.AddDamage(damage);
        yield return new WaitForSeconds(0.5f);
        bubble.MoveToPos(attacker.transform.position, 1f, posCurve);
        yield return new WaitForSeconds(0.99f);
        attacker.SubtractHealth(damage);
        yield return new WaitForSeconds(1f);
        Destroy(reflectObject);
        BattleStateManager.me.paused = false;
        BattleStateManager.me.IncrementState();
    }

    public void AddShield(int num)
    {
        shield += num;
    }

    public override bool AddEffect(string effect)
    {
        switch(effect)
        {
            case "burning":
                if (shield == 0 && !effects.Contains("burning"))
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
		timetravel.Play();
		deathparts.Play();
		normalMusic.Stop();
        yield return new WaitForSeconds(15f);
        BattleStateManager.me.RestartScene();
    }
}
