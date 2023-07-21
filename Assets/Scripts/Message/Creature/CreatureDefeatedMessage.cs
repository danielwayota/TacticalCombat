using UnityEngine;

public class CreatureDefeatedMessage : Message
{
    public override MessageTag tag => MessageTag.CREATURE_DEFEATED;

    public Creature emitter { get; protected set; }
    public Creature receiver { get; protected set; }

    public Skill skill { get; protected set; }

    public CreatureDefeatedMessage(Creature emitter, Creature receiver, Skill skill)
    {
        this.emitter = emitter;
        this.receiver = receiver;
        this.skill = skill;
    }
}