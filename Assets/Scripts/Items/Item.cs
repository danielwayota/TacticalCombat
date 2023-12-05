using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string itemName = "- No name -";

    public abstract void Use(CreatureData targetCreature);
}
