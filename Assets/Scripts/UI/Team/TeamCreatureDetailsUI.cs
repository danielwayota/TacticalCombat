using UnityEngine;
using UnityEngine.UI;

public class TeamCreatureDetailsUI : MonoBehaviour
{
    public Text creatureNameLabel;
    public Text creatureLevelLabel;

    public HealthBarUI healthBarUI;
    public Slider expSlider;

    public DynamicItemUIList dynStatsList;
    public DynamicItemUIList dynSkillList;

    public bool isVisible { get => this.gameObject.activeSelf; }

    public void ConfigureAndHide()
    {
        this.dynStatsList.ConfigureAndHide();
        this.dynSkillList.ConfigureAndHide();
        this.Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void DisplayCreatureDetails(CreatureData creatureData)
    {
        this.creatureNameLabel.text = creatureData.id;
        this.creatureLevelLabel.text = "Lvl " + creatureData.level;

        this.healthBarUI.SetHealth(creatureData.stats.hp, creatureData.stats.maxhp);
        this.expSlider.value = creatureData.stats.experiencePercent;

        Stats baseStats = creatureData.stats;

        this.dynStatsList.HideAll();

        // Mostrar stats
        this.dynStatsList.GetNextItemAndActivate<SingleStatUI>().Configure("Atk", baseStats.attack);
        this.dynStatsList.GetNextItemAndActivate<SingleStatUI>().Configure("Def", baseStats.defense);
        this.dynStatsList.GetNextItemAndActivate<SingleStatUI>().Configure("Spd", baseStats.speed);
        this.dynStatsList.GetNextItemAndActivate<SingleStatUI>().Configure("EAtk", baseStats.elemAttack);
        this.dynStatsList.GetNextItemAndActivate<SingleStatUI>().Configure("EDef", baseStats.elemDefense);
        this.dynStatsList.GetNextItemAndActivate<SingleStatUI>().Configure("Acc", baseStats.accuracy);
        this.dynStatsList.GetNextItemAndActivate<SingleStatUI>().Configure("Eva", baseStats.evasion);

        this.DisplaySkills(creatureData);
    }

    private void DisplaySkills(CreatureData creatureData)
    {
        this.dynSkillList.HideAll();

        for (int i = 0; i < creatureData.skillPrefabs.Count; i++)
        {
            GameObject skillPrefab = creatureData.skillPrefabs[i];

            Skill skill = skillPrefab.GetComponentInChildren<Skill>();
            var skillItemUI = this.dynSkillList.GetNextItemAndActivate<TeamCreatureDetailsSkillItemUI>();

            skillItemUI.Configure(skill, i < 3);

            int skillIndex = i;
            skillItemUI.AddClickUpEvent(() =>
            {
                creatureData.ChangeSkillOrderByIndex(skillIndex, skillIndex - 1);
                this.DisplaySkills(creatureData);
            });

            skillItemUI.AddClickDownEvent(() =>
            {
                creatureData.ChangeSkillOrderByIndex(skillIndex, skillIndex + 1);
                this.DisplaySkills(creatureData);
            });
        }
    }
}