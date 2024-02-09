using UnityEngine;

public class AdventureTreasureNode : AdventureMapNode
{
    private ItemStack treasure;

    public void Configure(ItemStack stack)
    {
        this.treasure = stack;
    }

    public override void Visit()
    {
        OverworldManager.current.AddItemToInventory(this.treasure.item, this.treasure.amount);

        Debug.Log($"Item give: {this.treasure.item} : {this.treasure.amount}");

        this.MarkAsVisited();
    }
}