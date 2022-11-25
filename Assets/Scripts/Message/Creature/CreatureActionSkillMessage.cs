public class CreatureActionSkillMessage : CreatureBaseMessage
{
    public override MessageTag tag => MessageTag.ACTION_CREATURE_SKILL;

    public Skill skill { get; protected set; }

    public CreatureActionSkillMessage(Creature creature, Skill skill) : base(creature)
    {
        this.skill = skill;
    }
}