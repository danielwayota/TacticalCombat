using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryUseUI : MonoBehaviour
{
    public Text itemInUseLabel;
    public Text itemInUseAmountLabel;

    public DynamicItemUIList dynTargetCreatureList;

    public bool isVisible { get => this.gameObject.activeSelf; }

    private ItemStack currentItemStack;
    private List<InventoryUseOnCreatureUI> targetCreaturesUIs;

    public void ConfigureAndHide()
    {
        this.dynTargetCreatureList.ConfigureAndHide();
        this.Hide();

        this.targetCreaturesUIs = new List<InventoryUseOnCreatureUI>();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        this.dynTargetCreatureList.HideAll();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        this.dynTargetCreatureList.HideAll();
    }

    public void Configure(ItemStack itemStack)
    {
        this.targetCreaturesUIs.Clear();
        this.currentItemStack = itemStack;

        this.Refresh();
    }

    public void Refresh()
    {
        this.itemInUseLabel.text = this.currentItemStack.item.itemName;
        this.itemInUseAmountLabel.text = "x" + this.currentItemStack.amount.ToString();

        foreach (var targetCreatureUI in this.targetCreaturesUIs)
        {
            targetCreatureUI.Refresh();
            if (this.currentItemStack.isEmpty)
            {
                targetCreatureUI.DisableUseButton();
            }
        }
    }

    public void AddCreature(CreatureData creature, UnityAction onUseClick)
    {
        InventoryUseOnCreatureUI ui = this.dynTargetCreatureList.GetNextItemAndActivate<InventoryUseOnCreatureUI>();
        ui.Configure(creature, onUseClick);

        this.targetCreaturesUIs.Add(ui);
    }
}