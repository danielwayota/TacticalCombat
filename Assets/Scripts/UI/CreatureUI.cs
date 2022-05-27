using UnityEngine;
using UnityEngine.UI;

public class CreatureUI : MonoBehaviour
{
    public static CreatureUI current;

    public GameObject[] energyBlocks;

    void Awake()
    {
        current = this;
        this.Hide();
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
