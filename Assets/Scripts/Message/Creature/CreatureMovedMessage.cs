using UnityEngine;

public class CreatureMovedMessage : CreatureBaseMessage
{
    public override MessageTag tag => MessageTag.CREATURE_MOVED;

    public int pathLength { get; protected set; }

    public CreatureMovedMessage(Creature creature, int pathLength) : base(creature)
    {
        this.pathLength = pathLength;
    }
}