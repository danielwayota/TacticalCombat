using UnityEngine;

public class TeamUI : MonoBehaviour
{
    public TeamCreatureListUI creatureListUI;
    public TeamCreatureDetailsUI creatureDetailsUI;

    void Start()
    {
        this.creatureListUI.ConfigureAndHide();
        this.creatureDetailsUI.ConfigureAndHide();
    }

    public void ToggleDisplay(CreatureData[] creatureDataList)
    {
        if (this.creatureListUI.isVisible)
        {
            this.creatureListUI.Hide();
            this.creatureDetailsUI.Hide();
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

    public void DisplayDetails(CreatureData data)
    {
        this.creatureDetailsUI.Show();

        this.creatureDetailsUI.DisplayCreatureDetails(data);
    }
}