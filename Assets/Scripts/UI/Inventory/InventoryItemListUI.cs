using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemListUI : MonoBehaviour
{
    public DynamicItemUIList dynItemList;

    public bool isVisible { get => this.gameObject.activeSelf; }

    public void ConfigureAndHide()
    {
        this.dynItemList.ConfigureAndHide();
        this.Hide();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        this.dynItemList.HideAll();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        this.dynItemList.HideAll();
    }

    public void AddItem(ItemStack stack, UnityAction onUseClick)
    {
        InventoryItemListItemUI itemUI = this.dynItemList.GetNextItemAndActivate<InventoryItemListItemUI>();
        itemUI.Configure(stack, onUseClick);
    }
}