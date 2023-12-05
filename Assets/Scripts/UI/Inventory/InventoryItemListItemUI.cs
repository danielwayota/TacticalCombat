using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryItemListItemUI : MonoBehaviour
{
    public Text itemNameLabel;
    public Text itemAmountLabel;

    public Button useBtn;

    public void Configure(ItemStack stack, UnityAction onUseClick)
    {
        this.itemNameLabel.text = stack.item.itemName;
        this.itemAmountLabel.text = "x" + stack.amount.ToString();

        this.useBtn.onClick.RemoveAllListeners();
        this.useBtn.onClick.AddListener(onUseClick);
    }
}