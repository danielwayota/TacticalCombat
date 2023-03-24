using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Text label;
    public Text skillCost;
    public Text skillElement;
    public Button btn;

    public void Configure(string text, UnityAction onClick, Skill skill)
    {
        this.label.text = text;

        this.btn.onClick.RemoveAllListeners();
        this.btn.onClick.AddListener(onClick);

        if (skill != null)
        {
            this.skillCost.gameObject.SetActive(true);
            this.skillCost.text = skill.cost.ToString();

            this.skillElement.gameObject.SetActive(true);
            this.skillElement.text = skill.elementalType.ToString();
        }
        else
        {
            this.skillCost.gameObject.SetActive(false);
            this.skillElement.gameObject.SetActive(false);
        }
    }
}
