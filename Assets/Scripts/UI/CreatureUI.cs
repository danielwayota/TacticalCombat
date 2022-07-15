using UnityEngine;
using UnityEngine.UI;

public class CreatureUI : MonoBehaviour
{
    public static CreatureUI current;

    public GameObject[] energyBlocks;

    public Slider healthSlider;

    public Text atkLabel;
    public Text defLabel;
    public Text spdLabel;

    void Awake()
    {
        current = this;
        this.Hide();
    }

    public void DisplayStats(Stats stats)
    {
        this.DisplayEnergy(stats.energy);

        this.healthSlider.value = stats.hp / (float)stats.maxhp;

        this.atkLabel.text = stats.attack.ToString();
        this.defLabel.text = stats.defense.ToString();
        this.spdLabel.text = stats.speed.ToString();
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

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
