using UnityEngine;

public class TeamUI : MonoBehaviour
{
    public TeamCreatureListUI creatureListUI;
    public TeamCreatureDetailsUI creatureDetailsUI;

    public bool isVisible { get => this.creatureListUI.isVisible || this.creatureDetailsUI.isVisible; }

    void Start()
    {
        this.creatureListUI.ConfigureAndHide();
        this.creatureDetailsUI.ConfigureAndHide();
    }

    public void ToggleDisplay(CreatureData[] creatureDataList)
    {
        if (this.isVisible)
        {
            this.Hide();
        }
        else
        {
            this.creatureListUI.Show();

            foreach (var creatureData in creatureDataList)
            {
                this.creatureListUI.AddCreatureData(creatureData, () =>
                {
                    this.DisplayDetails(creatureData);
                });
            }
        }
    }

    public void Hide()
    {
        this.creatureListUI.Hide();
        this.creatureDetailsUI.Hide();
    }

    public void DisplayDetails(CreatureData data)
    {
        this.creatureDetailsUI.Show();

        this.creatureDetailsUI.DisplayCreatureDetails(data);
    }
}