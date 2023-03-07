using UnityEngine;

public class CreatureMovedMessage : CreatureBaseMessage
{
    public override MessageTag tag => MessageTag.CREATURE_MOVED;

    public CreatureMovedMessage(Creature creature) : base(creature)
    {

    }
}