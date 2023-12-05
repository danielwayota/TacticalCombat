using UnityEngine;
using UnityEngine.UI;

public class ItemGainUI : MonoBehaviour
{
    public Text itemNameLabel;
    public Text itemAmountLabel;

    public void Configure(ItemStack stack)
    {
        this.itemNameLabel.text = stack.item.itemName;
        this.itemAmountLabel.text = "x" + stack.amount.ToString();
    }
}