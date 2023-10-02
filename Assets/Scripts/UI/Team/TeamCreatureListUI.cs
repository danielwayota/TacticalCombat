using UnityEngine;
using UnityEngine.Events;

public class TeamCreatureListUI : MonoBehaviour
{
    public DynamicItemUIList dynCreatureList;

    public bool isVisible { get => this.gameObject.activeSelf; }

    public void ConfigureAndHide()
    {
        this.dynCreatureList.ConfigureAndHide();
        this.Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.dynCreatureList.HideAll();
        this.gameObject.SetActive(false);
    }

    public void AddCreatureData(CreatureData creatureData, UnityAction onClick)
    {
        TeamCreatureListItemUI item = this.dynCreatureList.GetNextItemAndActivate<TeamCreatureListItemUI>();
        item.Configure(creatureData, onClick);
    }
}