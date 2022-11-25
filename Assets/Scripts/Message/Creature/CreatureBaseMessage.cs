public abstract class CreatureBaseMessage : Message
{
    public Creature creature { get; protected set; }

    public CreatureBaseMessage(Creature creature)
    {
        this.creature = creature;
    }
}