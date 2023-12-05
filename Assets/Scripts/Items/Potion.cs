using UnityEngine;

[CreateAssetMenu(fileName = "Potion", menuName = "Items/Potion", order = 0)]
public class Potion : Item
{
    public int healValue = 10;

    public override void Use(CreatureData targetCreature)
    {
        targetCreature.stats.hp = Mathf.Clamp(
            targetCreature.stats.hp + this.healValue,
            1,
            targetCreature.stats.maxhp
        );
    }
}