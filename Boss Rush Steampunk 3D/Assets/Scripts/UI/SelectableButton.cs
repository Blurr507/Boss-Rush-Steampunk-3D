using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectableButton : MonoBehaviour
{
    private Button button;
    public GameObject skillCheck;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(BattleStateManager.me.IncrementState);
        button.onClick.AddListener(ActivateSkillCheck);
    }

    public void ActivateSkillCheck()
    {
        Instantiate(skillCheck);
    }
}
