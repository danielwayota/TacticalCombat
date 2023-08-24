using UnityEngine;

public class CreatureCapturedMessage : Message
{
    public override MessageTag tag => MessageTag.CREATURE_CAPTURED;

    public Creature emitter { get; protected set; }
    public Creature receiver { get; protected set; }

    public CreatureCapturedMessage(Creature emitter, Creature receiver)
    {
        this.emitter = emitter;
        this.receiver = receiver;
    }
}