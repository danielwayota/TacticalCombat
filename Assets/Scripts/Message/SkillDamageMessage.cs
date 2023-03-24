public class SkillHealthModMessage : Message
{
    public override MessageTag tag => MessageTag.SKILL_HEALTH_MOD;

    public Skill skill { get; protected set; }
    public Creature receiver { get; protected set; }

    public int healthModAmount { get; protected set; }
    public bool critical { get; protected set; }

    public SkillHealthModMessage(Skill skill, Creature receiver, int healthModAmount, bool crit)
    {
        this.skill = skill;
        this.receiver = receiver;
        this.healthModAmount = healthModAmount;
        this.critical = crit;
    }

    public override string ToString()
    {
        string critText = this.critical ? "CRIT" : "";
        return $"{this.skill.skillName} to {this.receiver.name} with {this.healthModAmount} {critText}";
    }
}