using UnityEngine;

public class ItemGive : MonoBehaviour
{
    public Item item;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        OverworldManager.current.AddItemToInventory(this.item, this.amount);

        Destroy(this.gameObject);
    }
}