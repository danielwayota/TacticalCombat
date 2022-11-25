public class CreatureSelectedMessage : CreatureBaseMessage
{
    public override MessageTag tag => MessageTag.CREATURE_SELECTED;

    public CreatureSelectedMessage(Creature creature) : base(creature)
    {
    }
}