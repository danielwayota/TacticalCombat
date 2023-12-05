using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUseOnCreatureUI : MonoBehaviour
{
    public Text creatureNameLabel;
    public HealthBarUI healthBarUI;
    public Button useOnCreatureBtn;

    private CreatureData targetCreature;

    public void Configure(CreatureData targetCreature, UnityAction onUseClick)
    {
        this.targetCreature = targetCreature;
        this.useOnCreatureBtn.interactable = true;

        this.Refresh();

        this.useOnCreatureBtn.onClick.RemoveAllListeners();
        this.useOnCreatureBtn.onClick.AddListener(onUseClick);
    }

    public void Refresh()
    {
        this.creatureNameLabel.text = this.targetCreature.id;
        this.healthBarUI.SetHealth(this.targetCreature.stats.hp, this.targetCreature.stats.maxhp);
    }

    public void DisableUseButton()
    {
        this.useOnCreatureBtn.interactable = false;
    }
}