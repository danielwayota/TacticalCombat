using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CreatureUI : MonoBehaviour
{
    public static CreatureUI current;

    public GameObject[] energyBlocks;

    public Slider healthSlider;

    public DynamicItemUIList dynButtonList;
    public DynamicItemUIList dynStatList;

    void Awake()
    {
        current = this;

        this.dynButtonList.ConfigureAndHide();
        this.dynStatList.ConfigureAndHide();

        this.Hide();
    }

    public void DisplayStats(Stats stats)
    {
        this.DisplayEnergy(stats.energy);

        this.healthSlider.value = stats.hp / (float)stats.maxhp;

        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Atk", stats.attack);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Def", stats.defense);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("Spd", stats.speed);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("EAtk", stats.elemAttack);
        this.dynStatList.GetNextItemAndActivate<SingleStatUI>().Configure("EAtk", stats.elemDefense);
    }

    public void DisplayEnergy(int energy)
    {
        foreach (var block in this.energyBlocks)
        {
            block.SetActive(false);
        }

        for (int i = 0; i < energy; i++)
        {
            this.energyBlocks[i].SetActive(true);
        }
    }

    public void AddSkillButtton(string skillName, UnityAction onClick)
    {
        SkillButton btn = this.dynButtonList.GetNextItemAndActivate<SkillButton>();
        btn.Configure(skillName, onClick);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);

        this.dynButtonList.HideAll();
        this.dynStatList.HideAll();
    }
}
