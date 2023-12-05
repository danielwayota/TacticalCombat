using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryItemListUI itemListUI;
    public InventoryUseUI useItemUI;

    public bool isVisible { get => this.itemListUI.isVisible || this.useItemUI.isVisible; }

    void Start()
    {
        this.itemListUI.ConfigureAndHide();
        this.useItemUI.ConfigureAndHide();
    }

    public void Hide()
    {
        this.itemListUI.Hide();
        this.useItemUI.Hide();
    }

    public void ToggleDisplay(List<ItemStack> inventory, CreatureData[] creatures)
    {
        if (this.isVisible)
        {
            this.Hide();
        }
        else
        {
            this.ConfigureInventory(inventory, creatures);
        }
    }

    private void ConfigureInventory(List<ItemStack> inventory, CreatureData[] creatures)
    {
        this.itemListUI.Show();

        foreach (var stack in inventory)
        {
            this.itemListUI.AddItem(stack, () =>
            {
                this.useItemUI.Show();
                this.useItemUI.Configure(stack);

                foreach (var creature in creatures)
                {
                    this.useItemUI.AddCreature(creature, () =>
                    {
                        stack.item.Use(creature);
                        stack.amount--;

                        if (stack.isEmpty)
                        {
                            inventory.Remove(stack);
                        }

                        this.ConfigureInventory(inventory, creatures);
                    });
                }
            });
        }

        if (this.useItemUI.isVisible)
        {
            this.useItemUI.Refresh();
        }
    }
}