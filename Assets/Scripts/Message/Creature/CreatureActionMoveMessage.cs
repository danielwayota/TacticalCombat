public class CreatureActionMoveMessage : CreatureBaseMessage
{
    public override MessageTag tag => MessageTag.ACTION_CREATURE_MOVE;

    public CreatureActionMoveMessage(Creature creature) : base(creature)
    {
    }
}