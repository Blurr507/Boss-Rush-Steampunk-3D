using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectableButton : MonoBehaviour
{
    [HideInInspector]
    public Button button;               //  A reference to this object's button
    [HideInInspector]
    public TextMeshProUGUI text;        //  A reference to the text written on the button
    [HideInInspector]
    public string actionDescription;    //  What the text says
    public GameObject skillCheck;       //  The skill check object to be instantiated when clicking this button
    public int cooldown = 0;            //  How many turns you must wait between using this button
    [HideInInspector]
    public int currentCooldown = 0;     //  How many turns you must still wait to use this button again
    private bool usedThisTurn = false;  //  If this button was used this turn

	public AudioSource aud;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        actionDescription = text.text;
        button = GetComponent<Button>();
        button.onClick.AddListener(BattleStateManager.me.IncrementState);
        button.onClick.AddListener(ActivateSkillCheck);
		//button.onHover.AddListener(ActivateSkillCheck);
        BattleStateManager.me.buttons.Add(this);
    }

	void OnMouseEnter(){
		//aud.Play();
	}

    public void ActivateSkillCheck()
    {
        Instantiate(skillCheck).GetComponent<SkillCheck>().button = this;
        currentCooldown = cooldown;
        usedThisTurn = true;
        ManageButtonActiveness();
    }

    public int GetCooldown()
    {
        return currentCooldown;
    }

    public void ResetCooldown()
    {
        currentCooldown = 0;
        ManageButtonActiveness();
    }

    public void StepCooldown()
    {
        if(usedThisTurn)
        {
            usedThisTurn = false;
            return;
        }
        if (currentCooldown > 0)
        {
            currentCooldown--;
            ManageButtonActiveness();
        }
    }

    //  Sets the button to pressable if currentCooldown is 0. Otherwise,
    public virtual void ManageButtonActiveness()
    {
        if(currentCooldown <= 0)
        {
            button.interactable = true;
            text.text = actionDescription;
        }
        else
        {
            text.text = actionDescription + $" ({currentCooldown})";
            button.interactable = false;
        }
    }

    private void OnDestroy()
    {
        BattleStateManager.me.buttons.Remove(this);
    }
}
