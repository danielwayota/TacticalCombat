using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TeamCreatureDetailsSkillItemUI : MonoBehaviour
{
    public Image isBattleReadyColourBG;

    public Text skillNameLabel;
    public Text skillElementLabel;

    public Button btnUp;
    public Button btnDown;

    public void Configure(Skill skill, bool isBattleReady)
    {
        this.isBattleReadyColourBG.gameObject.SetActive(isBattleReady);

        this.skillNameLabel.text = skill.skillName;
        this.skillElementLabel.text = skill.elementalType.ToString();
    }

    public void AddClickUpEvent(UnityAction onClick)
    {
        this.btnUp.onClick.RemoveAllListeners();
        this.btnUp.onClick.AddListener(onClick);
    }

    public void AddClickDownEvent(UnityAction onClick)
    {
        this.btnDown.onClick.RemoveAllListeners();
        this.btnDown.onClick.AddListener(onClick);
    }
}