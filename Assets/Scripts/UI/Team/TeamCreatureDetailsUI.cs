using UnityEngine;
using UnityEngine.UI;

public class TeamCreatureDetailsUI : MonoBehaviour
{
    public Text creatureNameLabel;
    public Text creatureLevelLabel;

    public HealthBarUI healthBarUI;
    public Slider expSlider;

    public DynamicItemUIList dynStatsList;

    public bool isVisible { get => this.gameObject.activeSelf; }

    public void ConfigureAndHide()
    {
        this.dynStatsList.ConfigureAndHide();
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
    }
}