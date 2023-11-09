using UnityEngine;
using UnityEngine.UI;

public class SkillHitChanceUI : MonoBehaviour, IMessageListener
{
    public Text skillNameLabel;
    public Text skillHitChanceLabel;

    public void ConfigureAndHide()
    {
        MessageManager.current.AddListener(MessageTag.REQUEST_SKILL_HIT_CHANCE, this);
        this.gameObject.SetActive(false);
    }

    public void Receive(Message msg)
    {
        SkillHitChanceRequest shcr = msg as SkillHitChanceRequest;

        if (shcr.isAHideRequest)
        {
            // Nada que mostrar, ocultamos.
            this.gameObject.SetActive(false);
            return;
        }

        this.gameObject.SetActive(true);

        int intChance = Mathf.RoundToInt(shcr.chance * 100);
        this.skillNameLabel.text = shcr.skill.skillName;
        this.skillHitChanceLabel.text = $"{intChance} %";
    }
}