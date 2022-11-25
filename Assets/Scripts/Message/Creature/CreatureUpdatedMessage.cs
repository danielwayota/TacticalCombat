public class CreatureUpdatedMessage : CreatureBaseMessage
{
    public override MessageTag tag => MessageTag.CREATURE_UPDATED;

    public CreatureUpdatedMessage(Creature creature) : base(creature)
    {
    }
}