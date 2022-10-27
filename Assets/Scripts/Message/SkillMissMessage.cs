public class SkillMissMessage : Message
{
    public override MessageTag tag => MessageTag.SKILL_MISS;

    public Skill skill { get; protected set; }
    public Creature receiver { get; protected set; }

    public SkillMissMessage(Skill skill, Creature receiver)
    {
        this.skill = skill;
        this.receiver = receiver;
    }

    public override string ToString()
    {
        return $"Skill: {this.skill.skillName} MISS";
    }
}