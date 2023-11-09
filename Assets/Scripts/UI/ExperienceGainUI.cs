using UnityEngine;
using UnityEngine.UI;

public class ExperienceGainUI : MonoBehaviour
{
    public Text idLabel;
    public Text levelGainLabel;
    public Text expGainLabel;

    public Text newSkillLabel;

    public void Display(BattleOverCreatureData data)
    {
        this.idLabel.text = data.start.id;

        if (data.levelGain != 0)
        {
            this.levelGainLabel.gameObject.SetActive(true);
            this.levelGainLabel.text = $"+{data.levelGain} lv!";
        }
        else
        {
            this.levelGainLabel.gameObject.SetActive(false);
        }

        this.expGainLabel.text = $"+{data.experienceGain} EXP";

        this.newSkillLabel.gameObject.SetActive(data.hasNewSkill);
    }
}