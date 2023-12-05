using UnityEngine;

public class ItemStack
{
    public Item item;
    public int amount = 1;

    private int maxAmount = 99;

    public bool hasSpace { get => this.amount < this.maxAmount; }
    public bool isEmpty { get => this.amount <= 0; }

    public ItemStack(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}