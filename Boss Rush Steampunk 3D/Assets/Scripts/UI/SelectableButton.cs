using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class SelectableButton : MonoBehaviour
{
    private Button button;
    public GameObject skillCheck;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(BattleStateManager.me.IncrementState);
        button.onClick.AddListener(ActivateSkillCheck);
        Debug.Log("Yeah man");
    }

    public void ActivateSkillCheck()
    {
        Instantiate(skillCheck);
    }
}
